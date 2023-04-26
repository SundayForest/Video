using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoDomain.Respository;

namespace VideoInfrastructure.Respository
{
    public class UserRespository : IUserRespository
    {
        private readonly VideoDbContext db;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;

        public UserRespository(VideoDbContext db, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public Task AddHistoryAsync(User user, TheFile theFile)
        {
            throw new NotImplementedException();
        }

        //public async Task AddHistoryAsync(User user, TheFile theFile)
        //{
        //    user.AddHistory(theFile);
        //}

        public async Task AddNewLoginHistoryAsync(Guid userId)
        {
            UserLoginHistory userLoginHistory = UserLoginHistory.Init(userId);
            db.UserLoginHistorys.Add(userLoginHistory); 
        }

        public async Task<IdentityResult> AddRoleAsync(User user, string role)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                Role r = new Role { Name = role };
                var result = await roleManager.CreateAsync(r);
                if (result.Succeeded == false)
                {
                    return result;
                }
            }
            return await userManager.AddToRoleAsync(user, role);
        }

        public async Task<User?> AddUserAsync(string userName, string password)
        {
            if (db.Users.Any(u => u.UserName.Equals(userName))) {
                return null;
            }
            User user = new User(userName);
            await userManager.CreateAsync(user, password);
            return user;
        }

        public async Task<IdentityResult> ChangePwdAsync(User user, string newPwd)
        {
            if (newPwd.Length == 0)
            {
                return IdentityResult.Failed(new IdentityError());
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var resetPwdResult = await userManager.ResetPasswordAsync(user, token ,newPwd) ;
            return resetPwdResult;
        }

        public async Task<bool> CheckPwd(User user, string pwd)
        {
            return await userManager.CheckPasswordAsync(user,pwd);
        }

        public Task<List<AuthInfo>> FindAttentionsAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<string>> FindRolesAsync(User user)
        {
            return await userManager.GetRolesAsync(user);
        }

        public async Task<User?> FindUserAsync(string userName)
        {
            User user =  await userManager.FindByNameAsync(userName);
            return user;    
        }

        public Task<bool> IsAuthExist(long id)
        {
            throw new NotImplementedException();
        }
    }
}
