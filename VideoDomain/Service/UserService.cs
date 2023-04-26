

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using VideoDomain.Service.ServiceInterface;

namespace VideoDomain.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRespository userRespository;
        private readonly ICachingRepository cachingRepository;
        private readonly IOptions<JWTSetting> optJWT;

        public UserService(IUserRespository userRespository, IOptions<JWTSetting> optJWT, ICachingRepository cachingRepository)
        {
            this.userRespository = userRespository;
            this.optJWT = optJWT;
            this.cachingRepository = cachingRepository;
        }

        public async Task<string?> LoginAsync(string user, string password) {
            var u = await userRespository.FindUserAsync(user);
            if (u == null) {
                return null;
            }
            var res = await userRespository.CheckPwd(u, password);
            if (res)
            {
                string? token = await cachingRepository.FindTokenAsync(user);
                if (token == null)
                {
                    await userRespository.AddNewLoginHistoryAsync(u.Id);
                    return await BuildTokenAsync(u);
                }
                else {
                    return token;
                }
            }
            else {
                return null;
            }
        }
        public async Task<bool> ChangePwdAsync(string userName, string oldPwd, string newPwd) {
            var u = await userRespository.FindUserAsync(userName);
            var res = await userRespository.CheckPwd(u, oldPwd);
            if (res)
            {
                await userRespository.ChangePwdAsync(u,newPwd);
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<User?> RegisterAsync(string username,string pwd) {
            var user = await userRespository.FindUserAsync(username);
            if (user == null)
            {
                User u = new User(username);
                await userRespository.AddUserAsync(username,pwd);
                return u;
            }
            else {
                return null;
            }
        }
        public async Task UserDownAsync(string userName,TheFile file) { 
            var user = await userRespository.FindUserAsync(userName);
            if(user == null)
            {
                return;
            }
            await userRespository.AddHistoryAsync(user, file);
             
        }
        private async Task<string> BuildTokenAsync(User user) { 
            var roles = await userRespository.FindRolesAsync(user);
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name,user.UserName));
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role,role));
            }
            TimeSpan ExpiryDuration = TimeSpan.FromSeconds(optJWT.Value.ExpireSeconds);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(optJWT.Value.SecKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(claims: claims,
                expires: DateTime.Now.Add(ExpiryDuration), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
        public async Task<User?> FindUserAsync(string username)
        {
            return await userRespository.FindUserAsync(username);
        }
        public async Task<List<AuthInfo>> FindAttentionsAsync(string username) {
            var user = await userRespository.FindUserAsync(username);
            if(user == null)
            {
                return null;
            }
            return await userRespository.FindAttentionsAsync(user);
        }

        public Task<bool> AddAttention(string username, long authId)
        {
            throw new NotImplementedException();
        }
    }
}
