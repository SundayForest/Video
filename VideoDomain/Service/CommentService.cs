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
            int? total = await cachingRepository.FindFileCommentSizeAsync(filehash);
            if (total == null)
            {
                total = await commentRepository.GetFileCommentSizeAsync(filehash);
                await cachingRepository.SetFileCommentSizeAsync(filehash,total.Value);
            }
            return new(index,size,total.Value,data);
        }
        //获得用户评论
        public async Task<Page<List<Comment>>> PageWithSayerComment(int index, int size, long userId)
        {
            //var coms = await cachingRepository.findc
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
