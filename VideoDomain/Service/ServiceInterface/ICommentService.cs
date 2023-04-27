using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoDomain.Service.ServiceInterface
{
    public interface ICommentService
    {
        Task<bool> PublishComment(string filehash, string content, string username);
        Task<Page<List<Comment>>> PageWithFileComment(int index, int size, string filehash);
        Task<Page<List<Comment>>> PageWithSayerComment(int index, int size, long userId);
    }
}
