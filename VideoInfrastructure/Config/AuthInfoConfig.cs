using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoInfrastructure.Config
{
    public class AuthInfoConfig : IEntityTypeConfiguration<AuthInfo>
    {
        public void Configure(EntityTypeBuilder<AuthInfo> builder)
        {
            builder.ToTable("T_AuthInfos");
            builder.HasOne<User>(a=>a.User).WithOne(u=>u.AuthInfo).HasForeignKey<AuthInfo>(a=>a.UserId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
