using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoDomain.Respository;

namespace VideoInfrastructure.Respository
{
    public class CachingResposity : ICachingRepository
    {
        public Task<int> FindFileCommentSizeAsync(string filehash)
        {
            throw new NotImplementedException();
        }

        public Task<int> FindTagSizeAsync(int tagId)
        {
            throw new NotImplementedException();
        }

        public Task<string> FindTokenAsync(string username)
        {
            throw new NotImplementedException();
        }

        public Task SetFileCommentSizeAsync(string filehash, int size)
        {
            throw new NotImplementedException();
        }

        public Task SetTagSizeAsync(int tagId, int size)
        {
            throw new NotImplementedException();
        }
    }
}
