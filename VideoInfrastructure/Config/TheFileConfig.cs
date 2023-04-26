
namespace VideoInfrastructure.Config
{
    public class TheFileConfig : IEntityTypeConfiguration<TheFile>
    {
        public void Configure(EntityTypeBuilder<TheFile> builder)
        {
            builder.ToTable("T_TheFile");
            builder.HasKey(f=>f.FileHash);
            builder.HasMany<Tag>(f=>f.Tags).WithMany(t=>t.TheFiles).UsingEntity("T_Tags_Files");
            builder.HasQueryFilter(f=>f.IsDeleted);
            builder.HasIndex(f=>f.FileHash);
            builder.HasOne<AuthInfo>(f=>f.Auth).WithMany(a=>a.Works).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
