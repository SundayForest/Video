using System.Collections.Generic;
using VideoDomain.Service.ServiceInterface;
using VideoDomain.Value;

namespace VideoDomain.Service
{
    public class FileService:IFileService
    { 
        private readonly IFileRepository fileRepository;
        private readonly IUserRespository userRespository;
        private readonly IHubRepository hubRepository;
        private readonly ICachingRepository cachingRepository;
        public FileService(IFileRepository fileRepository, IUserRespository userRespository, IHubRepository hubRepository, ICachingRepository cachingRepository)
        {
            this.fileRepository = fileRepository;
            this.userRespository = userRespository;
            this.hubRepository = hubRepository;
            this.cachingRepository = cachingRepository;
        }

        public async Task<TheFile?> SaveFileAsync(Stream file,Stream file2,User auth,string file1Name,string file2Name)
        {
            using SHA256 sha256 = SHA256.Create();
            var hashbtye = sha256.ComputeHash(file);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashbtye.Length; i++) {
                sb.Append(hashbtye[i]);
            }
            string? hash = sb.ToString();
            DateTime time = DateTime.Today;
            if (hash == null)
            {
                return null;
            }
            //判断是否该文件已经存在
            TheFile? res = await fileRepository.FindFileAsync(hash);
            if (res == null)
            {
                string key1 = $"{time.Year}/{time.Month}/{time.Day}/{hash}/{file1Name}";
                string key2 = $"{time.Year}/{time.Month}/{time.Day}/{hash}/{file2Name}";
                long size = file.Length;
                await Console.Out.WriteLineAsync("res == null");
                file.Position = 0;
                Uri uri = await fileRepository.SaveAsync(key1,file);
                Uri img = await fileRepository.SaveAsync(key2,file2);
                var a = auth.AuthInfo;
                return TheFile.InitFile(uri,img,a,file1Name,hash,size);
            }
            return res;
        }
        public async Task<Page<List<TheFile>>> PageFileAsync(int index,int size,int tagId,DayType dayType) {
            var data = await cachingRepository.FindFilePage(tagId,dayType);
            if (data != null && data.Count >= (index + 1) * size) {
                data = data.Skip(index * size).Take(size).ToList();
            }
            //缓存中的数量小于分页归属或者没有缓存
            else
            {
                data = await fileRepository.PageFileOrderAsync(index, size, tagId, dayType);
            }
            //总数量
            int? total = await cachingRepository.FindTagSizeAsync(tagId);
            if(!total.HasValue)
            {
                total = await fileRepository.FindTagSizeAsync(tagId);
                await cachingRepository.SetOrChangeTagSizeAsync(tagId, total.Value);
            }
            return new(index, size, total.Value, data);
        }
        public async Task<bool> WantDownFile(string username,string filehash) { 
            User? user = await userRespository.FindUserAsync(username);
            if(user == null)
            {
                return false;
            }
            TheFile? theFile = await fileRepository.FindFileAsync(filehash);
            if (theFile == null)
            {
                return false;
            }
            //增加或新增点击
            await cachingRepository.AddOrSetFileHitChangeAsync(filehash);
            //通知下载
            await hubRepository.SendDonwnMess(username,filehash);
            return true;
        }

        public async Task<List<TheFile>> FindAuthWorksAsync(long id)
        {
            //尝试获取缓存
            var list = await cachingRepository.FindAuthWorksAsync(id);
            if(list == null)
            {
                //没有则查找并生成缓存
                list = await fileRepository.FindAuthWorksAsync(id);
                await cachingRepository.SetAuthWorksAsync(id,list);
            }
            return list;
        }

        public async Task<TheFile?> UpdateFileInfoAsync(string fileHash,List<Tag>? tags,string? des,string? filename, string userId)
        {
            //判空
            var temp = await fileRepository.FindFileAsync(fileHash);
            if (temp != null)
            {
                //是否是本人
                string uid = temp.Auth.UserId.ToString();
                if (uid.Equals(userId))
                {
                    //如果有传入tag，那么原本的tag都无效，则缓存中-1
                    if (tags != null) {
                        foreach (var tag in temp.Tags) {
                            await cachingRepository.SetOrChangeTagSizeAsync(tag.Id,-1);
                        }
                    }
                    var allTag = await fileRepository.GetAllTag();
                    tags = await fileRepository.UpdateFileInfoAsync(temp,tags,des,filename,allTag);
                    //往缓存里增加tag里的文件数量
                    foreach (var tag in tags) {
                        await cachingRepository.SetOrChangeTagSizeAsync(tag.Id,1);
                    }
                }
            }
            return null;
        }

        public async Task<List<Tag>> FindAllTagAsync()
        {
            var res =  await fileRepository.GetAllTag();
            return res.ToList();
        }
    }
}
