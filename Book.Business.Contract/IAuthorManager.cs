using Book.Common.Model;
using Book.Data.Contract.ViewModels.Author;
using Book.Data.Contract.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Business.Contract
{
    public interface IAuthorManager
    {
        Task<APIResponse<string>> AddAuthor(AuthorRequestVM request);
        Task<APIResponse<string>> UpdateAuthor(AuthorRequestVM request);
        Task<APIResponse<string>> DeleteAuthor(AuthorRequestVM request);
        Task<APIResponse<AuthorResponseVM>> GetAuthorById(int? Id);
        Task<APIResponse<List<AuthorResponseVM>>> GetAuthorList();
        Task<APIResponse<List<DDModel>>> GetAuthorDropDownList();

    }
}
