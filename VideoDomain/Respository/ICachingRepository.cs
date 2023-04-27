using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoDomain.Respository
{
    public interface ICachingRepository
    {
        //关于 Tag 下壁纸数量
        Task<int> FindTagSizeAsync(int tagId);
        Task SetOrChangeTagSizeAsync(int tagId,int size);

        //关于分页的缓存（缓存前500条）
        Task<List<TheFile>?> FindFilePage(int tagId, DayType dayType);
        Task SetFilePage(int tagId, DayType dayType, List<TheFile> files);

        //关于壁纸的评论数量
        Task<int> FindFileCommentSizeAsync(string filehash);
        Task SetFileCommentSizeAsync(string filehash,int size);

        //关于用户的评论数量
        Task<int> FindUserCommentSizeAsync(string userId);
        Task SetUserCommentSizeAsync(string userId, int size);

        //关于用户评论
        Task<List<Comment>?> FindUserCommentAsync(string userId);
        Task SetUserCommentAsync(string userId, List<string> coms);

        //关于壁纸评论
        Task<List<Comment>?> FindFileCommentAsync(string userId);
        Task SetFileCommentAsync(string userId, List<string> coms);

        //关于Token
        Task<string?> FindTokenAsync(string username);
        Task SetTokenAsync(string username,string tokens);


        //关于auth的作品
        Task<List<TheFile>?> FindAuthWorksAsync(long authId);
        Task SetAuthWorksAsync(long authId, List<TheFile> files);

        //关于用户的关注者
        Task<HashSet<AuthInfo>?> FindAttentionsAsync(long userId);
        Task SetAttentionsAsync(long userId,HashSet<AuthInfo> auths);

        //关于作品的点击量
        Task<int?> FindFileHitChangeAsync(string filehash);
        Task AddOrSetFileHitChangeAsync(string filehash);


        Task<long> GetSortedLength(string key);
    }
}
