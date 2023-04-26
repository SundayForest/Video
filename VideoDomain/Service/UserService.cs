

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using VideoDomain.Service.ServiceInterface;

namespace VideoDomain.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRespository userRespository;
        private readonly ICachingRepository cachingRepository;
        private readonly IOptions<JWTSetting> optJWT;

        public UserService(IUserRespository userRespository, IOptions<JWTSetting> optJWT, ICachingRepository cachingRepository)
        {
            this.userRespository = userRespository;
            this.optJWT = optJWT;
            this.cachingRepository = cachingRepository;
        }

        public async Task<string?> LoginAsync(string user, string password) {
            var u = await userRespository.FindUserAsync(user);
            if (u == null) {
                return null;
            }
            var res = await userRespository.CheckPwd(u, password);
            if (res)
            {
                string? token = await cachingRepository.FindTokenAsync(user);
                if (token == null)
                {
                    await userRespository.AddNewLoginHistoryAsync(u.Id);
                    return await BuildTokenAsync(u);
                }
                else {
                    return token;
                }
            }
            else {
                return null;
            }
        }
        public async Task<bool> ChangePwdAsync(string userName, string oldPwd, string newPwd) {
            var u = await userRespository.FindUserAsync(userName);
            if (u == null) {
                return false;
            }
            var res = await userRespository.CheckPwd(u, oldPwd);
            if (res)
            {
                await userRespository.ChangePwdAsync(u,newPwd);
            }
            return res;
        }
        public async Task<User?> RegisterAsync(string username,string pwd) {
            var user = await userRespository.FindUserAsync(username);
            if (user == null)
            {
                User u = new User(username);
                await userRespository.AddUserAsync(username,pwd);
                return u;
            }
            else {
                return null;
            }
        }
        public async Task UserDownAsync(string userName,TheFile file) { 
            var user = await userRespository.FindUserAsync(userName);
            if(user == null)
            {
                return;
            }
            await cachingRepository.AddOrSetFileHitChangeAsync(file.FileHash);
            
        }
        private async Task<string> BuildTokenAsync(User user) { 
            var roles = await userRespository.FindRolesAsync(user);
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name,user.UserName));
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role,role));
            }
            TimeSpan ExpiryDuration = TimeSpan.FromSeconds(optJWT.Value.ExpireSeconds);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(optJWT.Value.SecKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(claims: claims,
                expires: DateTime.Now.Add(ExpiryDuration), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
        public async Task<User?> FindUserAsync(string username)
        {
            return await userRespository.FindUserAsync(username);
        }
        public async Task<HashSet<AuthInfo>?> FindAttentionsAsync(string username) {
            var user = await userRespository.FindUserAsync(username);
            if (user == null)
            {
                return null;
            }
            var attentions = await cachingRepository.FindAttentionsAsync(user.Id);
            if (attentions == null)
            {
                attentions = await userRespository.FindAttentionsAsync(user);
                //更新缓存
                await cachingRepository.SetAttentionsAsync(user.Id, attentions);
            }
            return attentions;
        }

        public async Task<bool> AddAttention(string username, long authId)
        {
            var auth = await userRespository.FindAuthAsync(authId);
            if (auth == null) {
                return false;
            }
            //不复用 findattentions（） ，否则可能会更新 2 次缓存
            var user = await userRespository.FindUserAsync(username);
            if (user == null)
            {
                return false;
            }
            var attentions = await cachingRepository.FindAttentionsAsync(user.Id);
            if (attentions == null)
            {
                attentions = await userRespository.FindAttentionsAsync(user);
                if(attentions.Contains(auth))
                {
                    //已经关注过，直接返回
                    return false;
                }
                //更新数据库和缓存
                user.AddAttention(auth);
                attentions.Add(auth);
                await cachingRepository.SetAttentionsAsync(user.Id,attentions);
            }
            return true;
        }
    }
}
