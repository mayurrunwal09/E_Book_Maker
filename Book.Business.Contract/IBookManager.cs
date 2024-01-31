using Book.Data.Contract.ViewModels.Books;
using Book.Data.Contract.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Business.Contract
{
    public interface IBookManager
    {
        Task<string> AddBook(BookRequestVM request);
        Task<string> DeleteBook(BookRequestVM request);
        Task<BookResponseVM> GetBookById(long? Id);
        Task<List<BookResponseVM>> GetBookList();
        Task<string> UpdateBook(BookRequestVM request);
        Task<List<DDModel>> GetEmployeeDropDownList();
        Task<string> PauseBook(long bookId);
        Task<string> ResumeBook(long bookId);
    }
}
