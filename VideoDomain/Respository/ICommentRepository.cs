using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoDomain.Respository
{
    public interface ICommentRepository
    {
        Task SaveCommentAsync(TheFile file,string content,User sayer);
        Task<List<Comment>> PageWithFileComment(int index,int size,string filehash);
        Task<Page<List<Comment>>> PageWithSayerComment(int index,int size,Guid userId);
        Task<int> GetFileCommentSizeAsync(string filehash);
    }
}
