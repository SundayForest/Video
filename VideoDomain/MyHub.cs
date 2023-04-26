
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoDomain
{
    public class MyHub : Hub
    {
        private readonly UserManager<User> userManager;

        public MyHub(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task SendDonwnMess(string usename,string filehash) { 
            User? user = await userManager.FindByNameAsync(usename);
            if (user == null)
            {
                return;
            }
            await this.Clients.User(user.Id.ToString()).SendAsync("FileDownLoad",filehash);
        }
    }
}
