﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Data.Contract.ViewModels.Author
{
    public class AuthorResponseVM
    {
        public int Id {  get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public string? MobileNo { get; set; }
        public int? Age { get; set; }
    }
}
