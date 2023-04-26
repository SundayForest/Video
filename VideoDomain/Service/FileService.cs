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
            await Console.Out.WriteLineAsync("hashbyte:" + hashbtye.Length);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashbtye.Length; i++) {
                sb.Append(hashbtye[i]);
            }
            string? hash = sb.ToString();
            await Console.Out.WriteLineAsync("hash:" + hash);
            DateTime time = DateTime.Today;
            if (hash == null)
            {
                return null;
            }
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
            else
            {
                await Console.Out.WriteLineAsync(res.ToString());
                return res;
            }


        }
        public async Task<Page<List<TheFile>>> PageFileAsync(int index,int size,int tagId,DayType dayType) {
            var data = await fileRepository.PageFileOrderAsync(index,size,tagId,dayType);
            int total = await cachingRepository.FindTagSizeAsync(tagId);
            return new(index, size, total, data);
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
             await hubRepository.SendDonwnMess(username,filehash);
            return true;
        }

        public async Task<List<TheFile>> FindAuthWorksAsync(long id)
        {
            bool flag = await userRespository.IsAuthExist(id);
            if (flag)
            {
                var list = await fileRepository.FindAuthWorksAsync(id);
                return list;
            }
            else {
                return new List<TheFile>();
            }

        }

        public async Task<TheFile?> UpdateFileInfoAsync(string fileHash,List<Tag>? tags,string? des,string? filename, string userId)
        {
                var temp = await fileRepository.FindFileAsync(fileHash);
                if (temp != null)
                {
                    string uid = temp.Auth.UserId.ToString();
                    if (uid.Equals(userId))
                    {
                        if (tags != null)
                        {
                            foreach (var tag in tags) { 
                                temp.Tags.Add(tag);
                            }
                        }
                        if (des != null) { 
                            temp.UpdateDescription(des);
                        }
                        if(filename != null)
                        {
                            temp.UpdateName(filename);
                        }
                    }
                }
            return null;
        }

        public async Task<List<Tag>> FindAllTagAsync()
        {
            return await fileRepository.GetAllTag();
        }
    }
}
