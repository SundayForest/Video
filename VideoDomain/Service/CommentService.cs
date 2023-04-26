using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoDomain.Service.ServiceInterface;

namespace VideoDomain.Service
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository commentRepository;
        private readonly IFileRepository fileRepository;
        private readonly IUserRespository userRespository;
        private readonly ICachingRepository cachingRepository;
        public CommentService(ICommentRepository commentRepository, IFileRepository fileRepository, IUserRespository userRespository, ICachingRepository cachingRepository)
        {
            this.commentRepository = commentRepository;
            this.fileRepository = fileRepository;
            this.userRespository = userRespository;
            this.cachingRepository = cachingRepository;
        }

        /*
         * 获得文件评论数量时，保存到缓存中，随机保存一段时间，过期之后才更新
         */
        public async Task<Page<List<Comment>>> PageWithFileComment(int index, int size, string filehash)
        {
            var data = await commentRepository.PageWithFileComment(index, size, filehash);
            int total = await cachingRepository.FindFileCommentSizeAsync(filehash);
            if (total == -1)
            {
                total = await commentRepository.GetFileCommentSizeAsync(filehash);
                await cachingRepository.SetFileCommentSizeAsync(filehash,total);
            }
            return new(index,size,total,data);
        }
        /*
         * 用户的数量感觉比较多？要是保存到缓存里面会不会太多？存疑
         */
        public async Task<Page<List<Comment>>> PageWithSayerComment(int index, int size, Guid userId)
        {
            return await commentRepository.PageWithSayerComment(index,size,userId);
        }

        public async Task<bool> PublishComment(string filehash, string content, string username)
        {
            var file = await fileRepository.FindFileAsync(filehash);
            if (file != null) {
                var user = await userRespository.FindUserAsync(username);
                if(user != null)
                {
                    await commentRepository.SaveCommentAsync(file,content,user);
                    return true;
                }
            }
            return false;
        }
    }
}
