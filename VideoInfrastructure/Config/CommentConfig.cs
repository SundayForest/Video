using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoInfrastructure.Config
{
    public class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("T_Comments");
            builder.HasOne<User>(c=>c.Sayer).WithMany().HasForeignKey(c=>c.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<TheFile>(c=>c.TheFile).WithMany().HasForeignKey(c=>c.FileHash).OnDelete(DeleteBehavior.Restrict);
            builder.HasQueryFilter(c => c.IsDeleted == false);
        }
    }
}
