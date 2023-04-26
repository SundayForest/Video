using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoDomain.Value
{
    public record JwtOption(string SecKey,int ExpireSeconds);
}
