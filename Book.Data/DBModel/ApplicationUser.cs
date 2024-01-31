using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Data.DBModel
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? Email { get; set; }

        public string? Phoneno { get; set; }
        public string? Gender { get; set; }
        public string? Role { get; set; }
    }
}
