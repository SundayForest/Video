


namespace VideoInfrastructure
{
    public class VideoDbContext :IdentityDbContext<User, Role, long>
    {
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TheFile> TheFiles { get; set; }
        public DbSet<UserLoginHistory> UserLoginHistorys { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<AuthInfo> AuthInfos { get; set; }
        public VideoDbContext(DbContextOptions<VideoDbContext> options)
    : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
