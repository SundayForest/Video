using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoDomain.Respository;

namespace VideoInfrastructure.Respository
{
    public class HubRespository : IHubRepository
    {
        public Task SendDonwnMess(string username, string filehash)
        {
            throw new NotImplementedException();
        }
    }
}
