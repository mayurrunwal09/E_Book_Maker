﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Data.Contract.ViewModels.Authentication
{
    public class LoginResponse
    {
        public string? AccessToken { get; set; }
        public string? UserId { get; set; }
        public string? Name { get; set; }
    }
}
