using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VideoDomain.Respository;
using VideoDomain.Value;

namespace VideoInfrastructure.Respository
{
    public class CachingResposity : ICachingRepository
    {
        private readonly CacheHelp cacheHelp;

        public CachingResposity(CacheHelp cacheHelp)
        {
            this.cacheHelp = cacheHelp;
        }

        public async Task AddOrSetFileHitChangeAsync(string filehash)
        {
            await cacheHelp.AddWithTimeAsync(ConstValue.FILE_HIT +  filehash,ConstValue.TWO_DAY);
        }

        public async Task<HashSet<AuthInfo>?> FindAttentionsAsync(long userId)
        {
            var json =  await cacheHelp.GetStringWithTimeAsync(ConstValue.USER_ATTENTION +  userId.ToString(),ConstValue.TWO_DAY);
            if(json == null)
            {
                return null;
            }
            return JsonSerializer.Deserialize<HashSet<AuthInfo>>(json);
        }

        public async Task<List<TheFile>?> FindAuthWorksAsync(long authId)
        {
            var json = await cacheHelp.GetStringWithTimeAsync(ConstValue.AUTH_WORKS +  authId.ToString(),ConstValue.TWO_DAY);
            if (json == null) { 
                return null;
            }
            return JsonSerializer.Deserialize<List<TheFile>>(json);
        }

        public async Task<List<Comment>?> FindFileCommentAsync(string filehash)
        {
            var json = await cacheHelp.GetStringWithTimeAsync(ConstValue.FILE_COMMENT + filehash,ConstValue.TWO_DAY);
            if (json == null)
            {
                return null;
            }
            return JsonSerializer.Deserialize<List<Comment>>(json);
        }

        public async Task<int> FindFileCommentSizeAsync(string filehash)
        {
            var str = await cacheHelp.GetStringWithTimeAsync(ConstValue.FILE_COMMENT_SIZE + filehash,ConstValue.TWO_DAY);
            int num = 0;
            if (str != null)
            {
                int.TryParse(str,out num);
            }
            return num;
        }

        public async Task<int?> FindFileHitChangeAsync(string filehash)
        {
            return await cacheHelp.GetNumAsync(ConstValue.FILE_HIT + filehash);
        }

        public async Task<List<TheFile>?> FindFilePage(int tagId, DayType dayType)
        {
            var json = await cacheHelp.GetStringAsync(ConstValue.TOP_TAG_FILE + dayType + ":" + tagId );
            if (json == null) {
                return null;
            }
            return JsonSerializer.Deserialize<List<TheFile>>(json);
        }

        public async Task<int> FindTagSizeAsync(int tagId)
        {
            var str = await cacheHelp.GetStringAsync(ConstValue.TAGS_FILE_SORTED_LENGTH + tagId);
            int num = 0;
            if (str != null)
            {
                int.TryParse(str, out num);
            }
            return num;
        }

        public async Task<string?> FindTokenAsync(string username)
        {
            return await cacheHelp.GetStringAsync(ConstValue.USER_TOKEN_KEY + username);
        }

        public async Task<List<Comment>?> FindUserCommentAsync(string userId)
        {
            var json = await cacheHelp.GetStringWithTimeAsync(ConstValue.USER_COMMENT + userId, ConstValue.TWO_DAY);
            if(json == null) { return null;}
            return JsonSerializer.Deserialize<List<Comment>>(json);
        }

        public async Task<int> FindUserCommentSizeAsync(string userId)
        {
            var str = await cacheHelp.GetStringWithTimeAsync(ConstValue.USER_COMMENT_SIZE + userId, ConstValue.TWO_DAY);
            int num = 0;
            if (str != null)
            {
                int.TryParse(str, out num);
            }
            return num;
        }

        public async Task<long> GetSortedLength(string key)
        {
            return await cacheHelp.GetSortedLengthAsync(ConstValue.TAGS_FILE_SORTED_LENGTH +  key);
        }

        public async Task SetAttentionsAsync(long userId, HashSet<AuthInfo> auths)
        {
            string json = JsonSerializer.Serialize(auths);
            await cacheHelp.SetStringWithTimeAsync(ConstValue.USER_ATTENTION + userId, json, ConstValue.TWO_DAY);
        }

        public async Task SetAuthWorksAsync(long authId, List<TheFile> files)
        {
            string json = JsonSerializer.Serialize(files);
            await cacheHelp.SetStringWithTimeAsync(ConstValue.AUTH_WORKS + authId,json,ConstValue.TWO_DAY);
        }

        public async Task SetFileCommentAsync(string filehash, List<Comment> coms)
        {
            string json = JsonSerializer.Serialize(coms);
            await cacheHelp.SetStringWithTimeAsync(ConstValue.FILE_COMMENT + filehash,json,ConstValue.TWO_DAY);
        }

        public async Task SetFileCommentSizeAsync(string filehash, int size)
        {
            await cacheHelp.SetStringWithTimeAsync(ConstValue.FILE_COMMENT_SIZE + filehash,size.ToString(),ConstValue.TWO_DAY);
        }

        public async Task SetFilePage(int tagId, DayType dayType, List<TheFile> files)
        {
            string json = JsonSerializer.Serialize(files);
            await cacheHelp.SetStringAsync(ConstValue.TOP_TAG_FILE + dayType + ":" + tagId, json);
        }

        public async Task SetOrChangeTagSizeAsync(int tagId, int size)
        {
            if (size == 1 || size == -1)
            {
                await cacheHelp.AddAsync(ConstValue.TAGS_FILE_SORTED_LENGTH + tagId);
            }
            else
            {
                await cacheHelp.SetStringAsync(ConstValue.TAGS_FILE_SORTED_LENGTH + tagId,size.ToString());
            }
        }

        public async Task SetTokenAsync(string username, string tokens)
        {
            await cacheHelp.SetStringWithTimeAsync(ConstValue.USER_TOKEN_KEY + username,tokens,ConstValue.TEN_MINS);
        }

        public async Task SetUserCommentAsync(string userId, List<Comment> coms)
        {
            string json = JsonSerializer.Serialize(coms);
            await cacheHelp.SetStringWithTimeAsync(ConstValue.USER_COMMENT + userId,json,ConstValue.TWO_DAY);
        }

        public async Task SetUserCommentSizeAsync(string userId, int size)
        {
            await cacheHelp.SetStringWithTimeAsync(ConstValue.USER_COMMENT_SIZE + userId,size.ToString(),ConstValue.TWO_DAY);
        }
    }
}
