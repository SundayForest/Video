﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoDomain.Entity
{
    public record AuthInfo
    {
        public long Id { get; set; }    
        public User User { get; set; }
        public long UserId { get; set; }
        public int Fan { get; private set; }
        public List<User> Fans { get; private set; }
        public List<TheFile> Works { get; private set; }
        private AuthInfo() { }
        public static AuthInfo InitAuthInfo(User user) {
            AuthInfo authInfo = new AuthInfo();
            authInfo.User = user;
            authInfo.Fan = 0;
            return authInfo;
        }
    }
}
