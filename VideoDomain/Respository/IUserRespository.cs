namespace VideoDomain.Respository
{
    public interface IUserRespository
    {
        Task<User> AddUserAsync(string userName, string password);
        Task<User?> FindUserAsync(string username);
        Task AddNewLoginHistoryAsync(long userId);
        Task<bool> CheckPwd(User user, string pwd);
        Task<IdentityResult> ChangePwdAsync(User user, string newPwd);
        Task AddHistoryAsync(User user,TheFile theFile);
        Task<IList<string>> FindRolesAsync(User user);
        Task<IdentityResult> AddRoleAsync(User user, string role);
        Task<HashSet<AuthInfo>> FindAttentionsAsync(User user);
        Task<AuthInfo> FindAuthAsync(long id);
    }
}
