using Book.Data.DBModel;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Data.Contract.ViewModels.Books
{
    public class BookResponseVM
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Context { get; set; }
        public int? Pages { get; set; }
        public string? Format { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public int AuthorId { get; set; }
        public BookStatus Status { get; set; }

    }
}
