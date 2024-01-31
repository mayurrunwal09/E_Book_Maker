using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Data.DBModel
{
    public  class Books : BaseEntityClass
    {
        public string? Title { get; set; }
        public string? Context { get; set; }
        public int? Pages { get; set; }
        public int AuthorId { get; set; }
        public string? Format {  get; set; }
        public Author? Author { get; set; }
        public BookStatus Status { get; set; }
    }
    public enum BookStatus
    {
        Complete,
        Incomplete
    }

}
