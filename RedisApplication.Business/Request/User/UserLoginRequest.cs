﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisApplication.Business.Request.User
{
    public class UserLoginRequest
    {
        public string EMail { get; set; }
        public string Password { get; set; }
    }
}
