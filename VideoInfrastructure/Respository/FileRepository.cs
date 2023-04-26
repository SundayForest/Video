using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VideoDomain;
using Microsoft.AspNetCore.Hosting;
using VideoDomain.Value;
using Microsoft.AspNetCore.Http;

namespace VideoInfrastructure.Respository
{
    public class FileRepository : IFileRepository
    {
        private readonly VideoDbContext db;
        private readonly IWebHostEnvironment hostEnv;
        private readonly IHttpContextAccessor httpContextAccessor;
        public FileRepository(VideoDbContext db, IWebHostEnvironment hostEnv, IHttpContextAccessor httpContextAccessor)
        {
            this.db = db;
            this.hostEnv = hostEnv;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<Tag?> CreateTagAsync(string name)
        {
            if (await db.Tags.AnyAsync(t => t.Name.Equals(name))) {
                return null ;
            }
            Tag tag = Tag.InitTag(name);
            return tag;
        }

        public Task<List<TheFile>> FindAuthWorksAsync(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<TheFile?> FindFileAsync(string hash)
        {
            var theFiles = db.TheFiles.Where(f=>f.FileHash.Equals(hash)).Include(f=>f.Auth).Include(f=>f.Tags).ToArray();
            if (theFiles.Length == 0) { 
                return null ;
            }
            return theFiles.FirstOrDefault();

        }

        public async Task<TheFile?> FindFileWithTagAsync(string hash)
        {
            var theFiles = db.TheFiles.Where(f => f.FileHash.Equals(hash)).Include(f=>f.Tags).ToArray();
            if (theFiles.Length == 0)
            {
                return null;
            }
            return theFiles.FirstOrDefault();
        }

        public async Task<Tag?> FindTagWithFilesOrderAsync(int tagId)
        {
            var tags = db.Tags.Where(t => t.Id == tagId).Include(t => t.TheFiles).ToArray();
            if (tags.Length == 0)
            {
                return null;
            }
            var tag = tags.FirstOrDefault();
            tag!.TheFiles.OrderBy(f=>f.Hit);
            return tag;
            
        }

        public async Task<List<Tag>> GetAllTag()
        {
            return db.Tags.ToList();
        }

        public async Task<List<TheFile>> PageFileOrderAsync(int page, int pageSize, int tagId, DayType dayType)
        {
            var tag = await db.Tags.Where(t=>t.Id == tagId).Include(t=>t.TheFiles).FirstOrDefaultAsync();
            if (tag == null) {
                return new List<TheFile>();
            }
            IOrderedEnumerable<TheFile>? list;
            if (dayType == DayType.All)
            {
                list = tag.TheFiles.OrderBy(f => f.CreateTime);
            }
            else if (dayType == DayType.Year)
            {
                DateTime dateTime = DateTime.Now.AddYears(-1);
                list = tag.TheFiles.Where(f => f.CreateTime > dateTime).OrderBy(f => f.Hit);
            }
            else if (dayType == DayType.Month)
            {
                DateTime dateTime = DateTime.Now.AddMonths(-1);
                list = tag.TheFiles.Where(f => f.CreateTime > dateTime).OrderBy(f => f.Hit);
            }
            else {
                DateTime dateTime = DateTime.Now.AddDays(-1);
                list = tag.TheFiles.Where(f => f.CreateTime > dateTime).OrderBy(f => f.Hit);
            }
            return list.Skip(page * pageSize).Take(pageSize).ToList();
        }

        public async Task<Uri> SaveAsync(string key, Stream stream)
        {
            string workingDir = Path.Combine(hostEnv.ContentRootPath, "wwwroot");
            string fullPath = Path.Combine(workingDir, key);
            string? fullDir = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(fullDir))
            {
                Directory.CreateDirectory(fullDir);
            }
            var req = httpContextAccessor.HttpContext!.Request;
            string url = req.Scheme + "://" + req.Host + "/" + key;
            if (File.Exists(fullPath))
            {
                return new Uri(url);
            }
            using Stream outStream = File.OpenWrite(fullPath);
            await stream.CopyToAsync(outStream);
            return new Uri(url);
        }
    }
}
