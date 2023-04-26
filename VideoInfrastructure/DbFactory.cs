using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoInfrastructure
{
    public class DbFactory : IDesignTimeDbContextFactory<VideoDbContext>
    {
        public VideoDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<VideoDbContext> builder = new DbContextOptionsBuilder<VideoDbContext>();
            builder.UseSqlServer("data source=DESKTOP-OH6STF2\\MYSQLSERVER;initial catalog=DbWorkByPan;Trusted_Connection=true;Integrated Security = True;Encrypt = True; Trust Server Certificate = True");
            return new VideoDbContext(builder.Options);
        }
    }
}
