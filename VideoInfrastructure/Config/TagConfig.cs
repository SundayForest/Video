

namespace VideoInfrastructure.Config
{
    internal class TagConfig : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("T_Tag");
        }
    }
}
