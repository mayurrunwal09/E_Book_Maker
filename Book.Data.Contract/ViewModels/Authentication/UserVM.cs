﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Data.Contract.ViewModels.Authentication
{
    public class UserVM
    {
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? Phoneno { get; set; }
        public string? Gender { get; set; }
        public string? Email { get; set; }
        public string ? Role { get; set; }
      //  public string? Password { get; set; }
    }
}
