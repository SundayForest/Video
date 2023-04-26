using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoDomain.Service.ServiceInterface
{
    public interface IUserService
    {
        Task<string?> LoginAsync(string user, string password);
        Task<bool> ChangePwdAsync(string userName, string oldPwd, string newPwd);
        Task<User?> RegisterAsync(string username, string pwd);
        Task UserDownAsync(string userName, TheFile file);
        Task<User?> FindUserAsync(string username);
        Task<HashSet<AuthInfo>> FindAttentionsAsync(string username);
        Task<bool> AddAttention(string username,long authId);
    }
}
