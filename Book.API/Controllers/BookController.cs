using Book.Business.Contract;
using Book.Data.Contract.RepositoryInterfaces;
using Book.Data.Contract.ViewModels.Books;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Book.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookManager _bookManager;
        private readonly IBookRepository _bookRepository;
        public BookController(IBookRepository bookRepository, IBookManager bookManager)
        {
            _bookRepository = bookRepository;
            _bookManager = bookManager;
        }
        [HttpPost(nameof(AddBook))]
        public async Task<String> AddBook(BookRequestVM request)
        {
            var result = await _bookManager.AddBook(request);
            return result;
        }
        [HttpPost(nameof(UpdateBook))]
        public async Task<string> UpdateBook(BookRequestVM request)
        {
            var result = await  _bookManager.UpdateBook(request);
            return result;
        }
        [HttpPost(nameof(DeleteBook))]
        public async Task<string > DeleteBook(BookRequestVM request)
        {
            var result = await _bookManager.DeleteBook(request);
            return result;
        }

        [HttpGet(nameof(GetBooks))]
        public async Task<List<BookResponseVM>> GetBooks()
        {
            var result = await _bookManager.GetBookList();
            return result;
        }

        [HttpGet(nameof(GetBooksById))]
        public async Task<BookResponseVM> GetBooksById(long? Id)
        {
            var result = await _bookManager.GetBookById(Id);
            return result;
        }
        [HttpPost(nameof(PauseBook))]
        public async Task<string> PauseBook(long bookId)
        {
            var result = await _bookManager.PauseBook(bookId);
            return result;
        }

        [HttpPost(nameof(ResumeBook))]
        public async Task<string> ResumeBook(long bookId)
        {
            var result = await _bookManager.ResumeBook(bookId);
            return result;
        }



    }
}
