using Book.Business.Contract;
using Book.Business.Manager.Managers;
using Book.Common.Model;
using Book.Data.Contract.RepositoryInterfaces;
using Book.Data.Contract.ViewModels.Author;
using Book.Data.Contract.ViewModels.Books;
using Book.Data.Contract.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Book.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorManager _authorManager;
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IBookManager _bookManager;

        public AuthorController(IAuthorRepository authorRepository,IBookManager bookManager,IAuthorManager authorManager, IBookRepository bookRepository)
        {
            _authorManager = authorManager;
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
            _bookManager = bookManager;
        }
        [HttpPost(nameof(AddAuthor))]
        public async Task<APIResponse<string>> AddAuthor(AuthorRequestVM authorRequestVM)
        {
            var result = await _authorManager.AddAuthor(authorRequestVM);
            return result;
        }
        [HttpPost(nameof(UpdateAuthor))]
        public async Task<APIResponse<String>> UpdateAuthor(AuthorRequestVM authorRequestVM)
        {
            var result = await _authorManager.UpdateAuthor(authorRequestVM);
            return result;
        }

        [HttpPost(nameof(DeleteAuthor))]
        public async Task<APIResponse<string>> DeleteAuthor(AuthorRequestVM authorRequestVM)
        {
            var result = await _authorManager.DeleteAuthor(authorRequestVM);
            return result;
        }

        [HttpGet(nameof(GetAuthor))]
       // [Authorize(Roles = "Admin")]
        public async Task<APIResponse<List<AuthorResponseVM>>> GetAuthor()
        {
            var result = await _authorManager.GetAuthorList();
            return result;
        }
        [HttpGet(nameof(GetAuthorById))]
       // [Authorize]
        public async Task<APIResponse<AuthorResponseVM>> GetAuthorById(int? Id)
        {
            var result = await _authorManager.GetAuthorById(Id);
            return result;

        }
        [HttpGet(nameof(GetAuthorDropDownList))]
        public async Task<APIResponse<List<DDModel>>> GetAuthorDropDownList()
        {
            return await _authorManager.GetAuthorDropDownList();
        }
        [HttpPost(nameof(AddBook))]
        public async Task<string> AddBook(BookRequestVM request)
        {
            var result = await _bookManager.AddBook(request);
            return result;
        }


    }
}
