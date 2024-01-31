using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Data.DBModel
{
    public class Author : BaseEntityClass
    {
        public Author()
        {
            books = new HashSet<Books>();
        }

        public string? FirstName {  get; set; }
        public string? LastName { get; set; }
        public string? Email {  get; set; }
        public string? Gender { get; set; }
        public string? MobileNo {  get; set; }
        public int? Age { get; set; }

        public ICollection<Books> books { get; set; }
    }
}
