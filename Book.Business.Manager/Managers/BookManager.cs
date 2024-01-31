using Book.Business.Contract;
using Book.Data.Contract.RepositoryInterfaces;
using Book.Data.Contract.ViewModels.Books;
using Book.Data.Contract.ViewModels.Common;
using Book.Data.DBModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Test.Common.Helpers;

namespace Book.Business.Manager.Managers
{
    public class BookManager : IBookManager
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        public BookManager(IBookRepository bookRepository, IAuthorRepository authorRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }
        public async Task<string> AddBook(BookRequestVM request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request Object can not be null");
            }

            var canCreateNewBook = await CanAuthorCreateNewBook(request.AuthorId);
            if (!canCreateNewBook)
            {
                return "Author cannot create a new book at the moment. Complete or delete existing books.";
            }

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                Books books = new Books();

                books.Title = request.Title;
                books.Context = request.Context;
                books.Pages = request.Pages;
                books.AuthorId = request.AuthorId;
                books.Format = request.Format;
                books.Status = BookStatus.Complete;
                books.Created = Helper.GetCurrentUTCDatetime();

                _bookRepository.Add(books);
                await _bookRepository.SaveChangesAysnc();

                transactionScope.Complete();
                transactionScope.Dispose();

                return "Book Added Successfully";
            }
        }

        private async Task<bool> CanAuthorCreateNewBook(int authorId)
        {
            
            var incompleteBooksCount = await _bookRepository.Find(b => b.AuthorId == authorId && b.Status == BookStatus.Incomplete).CountAsync();

            return incompleteBooksCount < 3;
        }

        public async Task<string> DeleteBook(BookRequestVM request)
        {
            if(request == null)
            {
                throw new ArgumentNullException( nameof(request),"Request Object can not be null"); 
            }
            var objBooks =  await _bookRepository.FindOneAysnc(d=>d.Id == request.Id);
            if (objBooks == null)
            {
                throw new InvalidOperationException($"Book with ID {request.Id} not found.");
            }
            _bookRepository.Delete(objBooks);
            await _bookRepository.SaveChangesAysnc();

            return "Book Deleted Successfully";

        }

        public async Task<BookResponseVM> GetBookById(long? Id)
        {
            if(Id == null)
            {
                throw new ArgumentNullException( nameof(Id));
            }
          var objBooks = await _bookRepository.Find(d=>d.Id == Id)
                .Include(d=>d.Author).FirstOrDefaultAsync();

            if(objBooks == null)
            {
                throw new InvalidOperationException();
            }
            BookResponseVM bookRequestVM = new BookResponseVM()
            {
                Id = objBooks.Id,
                Title = objBooks.Title,
                Context = objBooks.Context,
                Email = objBooks.Author.Email,
                Pages = objBooks.Pages,
                AuthorId = objBooks.AuthorId,
                Format = objBooks.Format,
                Status = objBooks.Status,
            };
            return bookRequestVM;
        }

        public async Task<List<BookResponseVM>> GetBookList()
        {
            var bookList = await _bookRepository.All()
                .Include(d=>d.Author).ToListAsync();

            var resposneList = bookList.Select(book => new BookResponseVM
            {
                Id=book.Id,
                Title = book.Title,
                Context = book.Context,
                Pages = book.Pages,
                AuthorId = book.AuthorId,
                Format = book.Format,
                Status = book.Status,   

            }).ToList();
            return resposneList;
        }

        public async Task<List<DDModel>> GetEmployeeDropDownList()
        {
            var Book = await _bookRepository.All().ToListAsync();
            List<DDModel> bookList = Book.Select(d => new DDModel
            {
                Value = d.Id,
                Label = d.Title,
            }).ToList();
            return bookList;
        }

        public async Task<string> UpdateBook(BookRequestVM request)
        {
          if(request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
          var existingEmployee = await _bookRepository.FindOneAysnc(d=>d.Id == request.Id);

            if(existingEmployee == null)
            {
                throw new InvalidOperationException();
            }
            existingEmployee.Title = request.Title;
            existingEmployee.Context = request.Context;
            existingEmployee.Pages = request.Pages;
            existingEmployee.AuthorId = request.AuthorId;
            existingEmployee.Format = request.Format;
            existingEmployee.Status  = request.Status;
            existingEmployee.Updated = Helper.GetCurrentUTCDatetime();


            await _bookRepository.SaveChangesAysnc();
            return "Book Updated Successfully";
        }
        public async Task<string> PauseBook(long bookId)
        {
            var existingBook = await _bookRepository.FindOneAysnc(book => book.Id == bookId);

            if (existingBook == null)
            {
                throw new InvalidOperationException($"Book with ID {bookId} not found.");
            }

         
            if (existingBook.Status == BookStatus.Incomplete)
            {
                return "Book is already paused.";
            }

       
            existingBook.Status = BookStatus.Incomplete;

           
            existingBook.Updated = Helper.GetCurrentUTCDatetime();

            await _bookRepository.SaveChangesAysnc();

            return "Book Paused Successfully";
        }

        public async Task<string> ResumeBook(long bookId)
        {
            var existingBook = await _bookRepository.FindOneAysnc(book => book.Id == bookId);

            if (existingBook == null)
            {
                throw new InvalidOperationException($"Book with ID {bookId} not found.");
            }

            
            if (existingBook.Status == BookStatus.Complete)
            {
                return "Book is already resumed.";
            }

            
            existingBook.Status = BookStatus.Complete;

            
            existingBook.Updated = Helper.GetCurrentUTCDatetime();

            await _bookRepository.SaveChangesAysnc();

            return "Book Resumed Successfully";
        }

    }
}
