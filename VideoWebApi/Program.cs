using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VideoDomain;
using VideoDomain.Entity;
using VideoDomain.Respository;
using VideoDomain.Service;
using VideoInfrastructure;
using VideoInfrastructure.Respository;
using Microsoft.AspNetCore.StaticFiles;
using VideoDomain.Service.ServiceInterface;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.SignalR;
using System.Data;
using VideoDomain.Value;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    var scheme = new OpenApiSecurityScheme()
    {
        Description = "Authorization header. \r\nExample: 'Bearer 12345abcdef'",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Authorization"
        },
        Scheme = "oauth2",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
    };
    c.AddSecurityDefinition("Authorization", scheme);
    var requirement = new OpenApiSecurityRequirement();
    requirement[scheme] = new List<string>();
    c.AddSecurityRequirement(requirement);
});



builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IUserRespository, UserRespository>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IHubRepository, HubRespository>();
builder.Services.AddScoped<ICachingRepository, CachingResposity>();



builder.Services.AddDataProtection();
builder.Services.AddIdentityCore<User>(opt =>
{
    opt.Lockout.MaxFailedAccessAttempts = 10;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(1);
    opt.Password.RequireDigit = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequiredLength = 6;
    opt.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
    opt.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
});
var idBuilder = new IdentityBuilder(typeof(User), typeof(Role), builder.Services);
idBuilder.AddEntityFrameworkStores<VideoDbContext>()
    .AddDefaultTokenProviders().AddUserManager<UserManager<User>>()
    .AddRoleManager<RoleManager<Role>>();


builder.Services.AddDbContext<VideoDbContext>(opt => {
    string dbStr = builder.Configuration.GetSection("dbStr").Value;
    opt.UseSqlServer(dbStr);
});

builder.Services.AddHttpContextAccessor();
builder.Services.Configure<JWTSetting>(builder.Configuration.GetSection("JWT"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt => {
        var setting = builder.Configuration.GetSection("JWT").Get<JWTSetting>();
        byte[] keyBytes = Encoding.UTF8.GetBytes(setting.SecKey);
        var secKey = new SymmetricSecurityKey(keyBytes);
        opt.TokenValidationParameters = new()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = secKey
        };
        opt.Events = new JwtBearerEvents()
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/Hubs")))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddSignalR();
builder.Services.AddCors(opt => {
    opt.AddDefaultPolicy(b =>
    {
        //WithOrigins(new string[] { "http://localhost:3000/" })
        b.AllowAnyMethod().AllowAnyHeader().AllowCredentials().AllowAnyOrigin();
    });
});


var app = builder.Build();

app.UseStaticFiles();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapHub<MyHub>("/Hubs");
app.MapControllers();

app.Run();
