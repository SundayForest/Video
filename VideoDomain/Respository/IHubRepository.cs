using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoDomain.Respository
{
    public interface IHubRepository
    {
        Task SendDonwnMess(string username,string filehash);
    }
}
