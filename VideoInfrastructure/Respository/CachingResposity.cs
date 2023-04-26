using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoDomain.Respository;
using VideoDomain.Value;

namespace VideoInfrastructure.Respository
{
    public class CachingResposity : ICachingRepository
    {
        public Task AddOrSetFileHitChangeAsync(string filehash)
        {
            throw new NotImplementedException();
        }

        public Task<HashSet<AuthInfo>?> FindAttentionsAsync(long userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<TheFile>?> FindAuthWorksAsync(long authId)
        {
            throw new NotImplementedException();
        }

        public Task<int?> FindFileCommentSizeAsync(string filehash)
        {
            throw new NotImplementedException();
        }

        public Task<int?> FindFileHitChangeAsync(string filehash)
        {
            throw new NotImplementedException();
        }

        public Task<List<TheFile>?> FindFilePage(int tagId, DayType dayType)
        {
            throw new NotImplementedException();
        }

        public Task<int?> FindTagSizeAsync(int tagId)
        {
            throw new NotImplementedException();
        }

        public Task<string?> FindTokenAsync(string username)
        {
            throw new NotImplementedException();
        }

        public Task SetAttentionsAsync(long userId, HashSet<AuthInfo> auths)
        {
            throw new NotImplementedException();
        }

        public Task SetAuthWorksAsync(long authId, List<TheFile> files)
        {
            throw new NotImplementedException();
        }

        public Task SetFileCommentSizeAsync(string filehash, int size)
        {
            throw new NotImplementedException();
        }

        public Task SetFilePage(int tagId, DayType dayType, List<TheFile> files)
        {
            throw new NotImplementedException();
        }

        public Task SetOrChangeTagSizeAsync(int tagId, int size)
        {
            throw new NotImplementedException();
        }

        public Task SetTokenAsync(string username, string tokens)
        {
            throw new NotImplementedException();
        }
    }
}
