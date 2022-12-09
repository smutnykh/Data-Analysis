﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelnessAPI.Models.User
{
    public class UserRegisterViewModel
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
    }
}
