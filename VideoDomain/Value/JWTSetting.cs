using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoDomain.Value
{
    public class JWTSetting
    {
        public string SecKey { get; set; }
        public int ExpireSeconds { get; set; }
    }
}
