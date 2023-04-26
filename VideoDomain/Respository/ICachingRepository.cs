using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoDomain.Respository
{
    public interface ICachingRepository
    {
        Task<int> FindTagSizeAsync(int tagId);
        Task SetTagSizeAsync(int tagId,int size);
        Task<int> FindFileCommentSizeAsync(string filehash);
        Task SetFileCommentSizeAsync(string filehash,int size);
        Task<string> FindTokenAsync(string username);


    }
}
