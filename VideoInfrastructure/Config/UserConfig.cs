

namespace VideoInfrastructure.Config
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("T_Users");
            builder.HasKey(u => u.Id).IsClustered(false);
            builder.HasIndex(u=>u.UserName);
            builder.HasMany<AuthInfo>(u=>u.Attentions).WithMany(a=>a.Fans)
                .UsingEntity("T_Attentions");
        }
    }
}
