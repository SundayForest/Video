using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoDomain.Service.ServiceInterface
{
    public interface IFileService
    {
        Task<TheFile?> SaveFileAsync(Stream file, Stream file2, User auth, string file1Name, string file2Name);
        Task<Page<List<TheFile>>> PageFileAsync(int page, int size, int tagId,DayType type);
        Task<bool> WantDownFile(string username, string filehash);
        Task<List<TheFile>> FindAuthWorksAsync(long id);
        Task<TheFile?> UpdateFileInfoAsync(string fileHash, List<Tag>? tags, string? des, string? filename, string userId);
        Task<List<Tag>> FindAllTagAsync();
    }
}
