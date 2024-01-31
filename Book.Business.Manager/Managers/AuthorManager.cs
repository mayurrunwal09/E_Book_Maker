using Book.Business.Contract;
using Book.Common.Model;
using Book.Data.Contract.RepositoryInterfaces;
using Book.Data.Contract.ViewModels.Author;
using Book.Data.Contract.ViewModels.Common;
using Book.Data.DBModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Test.Common.Helpers;

namespace Book.Business.Manager.Managers
{
    public class AuthorManager : IAuthorManager
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;
        public AuthorManager(IAuthorRepository authorRepository, IBookRepository bookRepository)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
        }
        public async Task<APIResponse<string>> AddAuthor(AuthorRequestVM request)
        {
            if (request == null)
            {
                return new APIResponse<string>(HttpStatusCode.OK, APIStatus.Failure, null!, string.Format(ConstantHelper.DataNotFound), null!);
            }

            var canCreateNewBook = await CanAuthorCreateNewBook(request.Id);
            if (!canCreateNewBook)
            {
                return new APIResponse<string>(HttpStatusCode.OK, APIStatus.Failure, null!, "Author cannot create a new book at the moment. Complete or delete existing books.", null!);
            }

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                Author author = new Author();

                author.FirstName = request.FirstName;
                author.LastName = request.LastName;
                author.Email = request.Email;
                author.Gender = request.Gender;
                author.MobileNo = request.MobileNo;
                author.Age = request.Age;
                author.Created = Helper.GetCurrentUTCDatetime();

                _authorRepository.Add(author);
                await _authorRepository.SaveChangesAysnc();

                transactionScope.Complete();
                transactionScope.Dispose();

                return new APIResponse<string>(HttpStatusCode.OK, APIStatus.Success, null!, string.Format(ConstantHelper.Save, ConstantHelper.Author), null!);
            }
        }

        private async Task<bool> CanAuthorCreateNewBook(int authorId)
        {
            var incompleteBooksCount = await _bookRepository.Find(b => b.AuthorId == authorId && b.Status == BookStatus.Incomplete).CountAsync();

            return incompleteBooksCount < 3;
        }

        public async Task<APIResponse<string>> DeleteAuthor(AuthorRequestVM request)
        {
            if (request == null)
            {
                return new APIResponse<string>(HttpStatusCode.OK, APIStatus.Failure, null!, string.Format(ConstantHelper.DataNotFound), null!);
            }

            var objAuthor = await _authorRepository.Find(d=>d.Id == request.Id).FirstOrDefaultAsync();
            if(objAuthor == null)
            {
                throw new InvalidOperationException();
            }

            _authorRepository.Delete(objAuthor);
            await _authorRepository.SaveChangesAysnc();

            return new APIResponse<string>(HttpStatusCode.OK, APIStatus.Success, null!, string.Format(ConstantHelper.Delete, ConstantHelper.Author), null!);

        }

        public async Task<APIResponse<AuthorResponseVM>> GetAuthorById(int? Id)
        {
            if(Id == null)
            {
                return new APIResponse<AuthorResponseVM>(HttpStatusCode.OK, APIStatus.Failure, null!, string.Format(ConstantHelper.DataNotFound), null!);

            }
            var objAuthor = await _authorRepository.Find(x=>x.Id == Id).FirstOrDefaultAsync();
            if(objAuthor == null)
            {
                throw new ArgumentNullException(nameof(Id));
            }
            AuthorResponseVM authorDetails = new AuthorResponseVM()
            {
                Id = objAuthor.Id,
                FirstName = objAuthor.FirstName,
                LastName = objAuthor.LastName,
                Email = objAuthor.Email,
                Gender = objAuthor.Gender,
                MobileNo = objAuthor.MobileNo,
                Age = objAuthor.Age,
            };
            return new APIResponse<AuthorResponseVM>(HttpStatusCode.OK,APIStatus.Success,authorDetails, null!,null!);
        }

        public async Task<APIResponse<List<AuthorResponseVM>>> GetAuthorList()
        {
           var authorList = await _authorRepository.All().ToListAsync();
            if(authorList == null)
            {
                return new APIResponse<List<AuthorResponseVM>>(HttpStatusCode.OK, APIStatus.Failure, null!, string.Format(ConstantHelper.DataNotFound), null!);

            }
            var responseList = authorList.Select(author => new AuthorResponseVM
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                Email = author.Email,
                Gender = author.Gender,
                MobileNo = author.MobileNo,
                Age = author.Age,
            }).ToList();
            return new APIResponse<List<AuthorResponseVM>>(HttpStatusCode.OK, APIStatus.Success, responseList, null!, null!);

        }

        public async Task<APIResponse<List<DDModel>>> GetAuthorDropDownList()
        {
            var authors = await _authorRepository.All().ToListAsync();

            List<DDModel> authorList = authors.Select(s => new DDModel
            {
                Value = s.Id,
                Label = s.FirstName,
            }).ToList();

            return new APIResponse<List<DDModel>>(HttpStatusCode.OK, APIStatus.Success, authorList, null!, null!);
        }

        public async Task<APIResponse<string>> UpdateAuthor(AuthorRequestVM request)
        {
           if(request == null)
            {
                return new APIResponse<string>(HttpStatusCode.OK, APIStatus.Failure, null!, string.Format(ConstantHelper.DataNotFound), null!);

            }

           var authorDetails = await _authorRepository.FindOneAysnc(d=>d.Id == request.Id);
            if(authorDetails == null)
            {
                throw new ArgumentNullException(nameof(authorDetails));

            }
            Author author = new Author();
            authorDetails.Id = request.Id;
            authorDetails.FirstName = request.FirstName;
            authorDetails.LastName = request.LastName;
            authorDetails.Email = request.Email;
            authorDetails.Gender = request.Gender;
            authorDetails.MobileNo = request.MobileNo;
            authorDetails.Age = request.Age;

            _authorRepository.Update(authorDetails);
            await _authorRepository.SaveChangesAysnc();

            return new APIResponse<string>(HttpStatusCode.OK, APIStatus.Success, null!, string.Format(ConstantHelper.Update, ConstantHelper.Author), null!);
        }

    }
}
