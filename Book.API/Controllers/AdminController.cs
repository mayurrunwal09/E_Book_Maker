/*using System.IO;
using System.Threading.Tasks;
using Book.Business.Contract;
using Book.Data.Contract.ViewModels.Books;
using Book.Data.DBModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Xceed.Words.NET;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using DocumentFormat.OpenXml.Bibliography;

namespace Book.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IBookManager _bookManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(IBookManager bookManager, IWebHostEnvironment webHostEnvironment)
        {
            _bookManager = bookManager;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet(nameof(GetBooksById))]
        public async Task<ActionResult<BookResponseVM>> GetBooksById(long? Id)
        {
            if (!Id.HasValue)
            {
                return BadRequest("Invalid book ID");
            }

            var bookDetails = await _bookManager.GetBookById(Id);

            if (bookDetails.Status == BookStatus.Complete)
            {
                var pdfContent = await GeneratePdfContent(bookDetails);
                var wordContent = GenerateWordContent(bookDetails);

                SaveToFileSystem(pdfContent, "pdf", bookDetails.Id);
                SaveToFileSystem(wordContent, "docx", bookDetails.Id);

                return Ok(new { BookDetails = bookDetails, PdfFilePath = GetFilePath("pdf", bookDetails.Id), WordFilePath = GetFilePath("docx", bookDetails.Id) });
            }

            return Ok(bookDetails);
        }
        [HttpGet(nameof(GetBooks))]
        public async Task<List<BookResponseVM>> GetBooks()
        {
            var result = await _bookManager.GetBookList();
            return result;
        }

        [HttpPost(nameof(GenerateFilesByBookId))]
        public async Task<ActionResult> GenerateFilesByBookId(long? Id)
        {
            if (!Id.HasValue)
            {
                return BadRequest("Invalid book ID");
            }

            var bookDetails = await _bookManager.GetBookById(Id);

            if (bookDetails.Status == BookStatus.Complete)
            {
                var pdfContent = await GeneratePdfContent(bookDetails);
                var wordContent = GenerateWordContent(bookDetails);

                SaveToFileSystem(pdfContent, "pdf", bookDetails.Id);
                SaveToFileSystem(wordContent, "docx", bookDetails.Id);

                return Ok(new { PdfFilePath = GetFilePath("pdf", bookDetails.Id), WordFilePath = GetFilePath("docx", bookDetails.Id) });
            }

            return Ok("Files not generated. Book status is not complete.");
        }

        private async Task<byte[]> GeneratePdfContent(BookResponseVM bookDetails)
        {
            int numberOfPages = bookDetails.Pages ?? 0;

            using (var memoryStream = new MemoryStream())
            {
                var document = new Document();
                for (int i = 1; i <= numberOfPages; i++)
                {
                    var section = document.AddSection();
                    var paragraph = section.AddParagraph($"Page {i}");
                    paragraph.AddLineBreak();
                    paragraph.AddFormattedText($"Title: {bookDetails.Title}", TextFormat.Bold);
                    paragraph.AddLineBreak();
                    paragraph.AddFormattedText($"Pages: {bookDetails.Pages}", TextFormat.Bold);
                    paragraph.AddLineBreak();
                    paragraph.AddFormattedText($"Context: {bookDetails.Context}", TextFormat.Italic);
               
                }

                PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();
                pdfRenderer.Document = document;
                pdfRenderer.RenderDocument();
                pdfRenderer.PdfDocument.Save(memoryStream, false);

                return memoryStream.ToArray();
            }
        }

        private byte[] GenerateWordContent(BookResponseVM bookDetails)
        {
            int numberOfPages = bookDetails.Pages ?? 0;

            using (var memoryStream = new MemoryStream())
            {
                using (var doc = DocX.Create(memoryStream))
                {
                    for (int i = 1; i <= numberOfPages; i++)
                    {
                        doc.InsertParagraph($"Page {i}");
                        doc.InsertParagraph($"Title: {bookDetails.Title}").Bold();
                        doc.InsertParagraph($"Pages: {bookDetails.Pages}").Bold();
                        doc.InsertParagraph($"Context: {bookDetails.Context}").Italic();
                       
                    }

                    doc.Save();
                }

                return memoryStream.ToArray();
            }
        }



        private void SaveToFileSystem(byte[] content, string extension, long bookId)
        {
            var rootPath = _webHostEnvironment.ContentRootPath;
            var filePath = Path.Combine(rootPath, $"Book_{bookId}.{extension}");

            System.IO.File.WriteAllBytes(filePath, content);
        }

        private string GetFilePath(string extension, long bookId)
        {
            var rootPath = _webHostEnvironment.ContentRootPath;
            return Path.Combine(rootPath, $"Book_{bookId}.{extension}");
        }
    }
}
*/








using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Book.Business.Contract;
using Book.Data.Contract.ViewModels.Books;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using Xceed.Words.NET;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using Book.Data.DBModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Book.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IBookManager _bookManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager; 

        public AdminController(IBookManager bookManager, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager)
        {
            _bookManager = bookManager;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        [HttpGet(nameof(GetBooks))]
        public async Task<List<BookResponseVM>> GetBooks()
        {
            var result = await _bookManager.GetBookList();
            return result;
        }

        [HttpGet(nameof(GetBooksById))]
        public async Task<ActionResult<BookResponseVM>> GetBooksById(long? Id)
        {
            if (!Id.HasValue)
            {
                return BadRequest("Invalid book ID");
            }

            var bookDetails = await _bookManager.GetBookById(Id);

        

            return Ok(bookDetails);
        }


        [HttpPost(nameof(GenerateFilesByBookId))]
        public async Task<ActionResult> GenerateFilesByBookId(long? Id)
        {
            if (!Id.HasValue)
            {
                return BadRequest("Invalid book ID");
            }

            var bookDetails = await _bookManager.GetBookById(Id);

            if (bookDetails.Status == BookStatus.Complete)
            {
                var authorUser = await FetchUserByAuthorId(bookDetails.AuthorId);

                var pdfContent = await GeneratePdfContent(bookDetails);
                var wordContent = GenerateWordContent(bookDetails);

                SaveToFileSystem(pdfContent, "pdf", bookDetails.Id);
                SaveToFileSystem(wordContent, "docx", bookDetails.Id);

                await SendEmailAsync(authorUser, pdfContent, wordContent);

                var pdfFilePath = GetFilePath("pdf", bookDetails.Id);
                var wordFilePath = GetFilePath("docx", bookDetails.Id);
                var response = new
                {
                    PdfFilePath = pdfFilePath,
                    WordFilePath = wordFilePath,
                    Message = "The file sent on the Author's Email."
                };
                return Ok(response);    
            }

            return Ok("Files not generated. Book status is not complete.");
        }

        private async Task<byte[]> GeneratePdfContent(BookResponseVM bookDetails)
        {
            int numberOfPages = bookDetails.Pages ?? 0;

            using (var memoryStream = new MemoryStream())
            {
                var document = new Document();
                for (int i = 1; i <= numberOfPages; i++)
                {
                    var section = document.AddSection();
                    var paragraph = section.AddParagraph($"Page {i}");
                    paragraph.AddLineBreak();
                    paragraph.AddFormattedText($"Title: {bookDetails.Title}", TextFormat.Bold);
                    paragraph.AddLineBreak();
                    paragraph.AddFormattedText($"Pages: {bookDetails.Pages}", TextFormat.Bold);
                    paragraph.AddLineBreak();
                    paragraph.AddFormattedText($"Context: {bookDetails.Context}", TextFormat.Italic);

                }

                PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();
                pdfRenderer.Document = document;
                pdfRenderer.RenderDocument();
                pdfRenderer.PdfDocument.Save(memoryStream, false);

                return memoryStream.ToArray();
            }
        }

        private byte[] GenerateWordContent(BookResponseVM bookDetails)
        {
            int numberOfPages = bookDetails.Pages ?? 0;

            using (var memoryStream = new MemoryStream())
            {
                using (var doc = DocX.Create(memoryStream))
                {
                    for (int i = 1; i <= numberOfPages; i++)
                    {
                        doc.InsertParagraph($"Page {i}");
                        doc.InsertParagraph($"Title: {bookDetails.Title}").Bold();
                        doc.InsertParagraph($"Pages: {bookDetails.Pages}").Bold();
                        doc.InsertParagraph($"Context: {bookDetails.Context}").Italic();

                    }

                    doc.Save();
                }

                return memoryStream.ToArray();
            }
        }

        private void SaveToFileSystem(byte[] content, string extension, long bookId)
        {
            var rootPath = _webHostEnvironment.ContentRootPath;
            var filePath = Path.Combine(rootPath, $"Book_{bookId}.{extension}");

            System.IO.File.WriteAllBytes(filePath, content);
        }

        private string GetFilePath(string extension, long bookId)
        {
            var rootPath = _webHostEnvironment.ContentRootPath;
            return Path.Combine(rootPath, $"Book_{bookId}.{extension}");
        }

        private async Task<ApplicationUser> FetchUserByAuthorId(int authorId)
        {
            var user = await _userManager.FindByIdAsync(authorId.ToString());

            if (user == null)
            {
               
                return null;
            }

            return user;
        }


        private async Task SendEmailAsync(ApplicationUser authorUser, byte[] pdfContent, byte[] wordContent)
        {
            if (authorUser == null || string.IsNullOrEmpty(authorUser.Email))
            {
                Console.Error.WriteLine("Error: Author details or Email address is null or empty.");
                return;
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Mayur Runwal", "mayurrunwal9@gmail.com")); 

            string toDisplayName = string.IsNullOrEmpty(authorUser.Name) ? "" : authorUser.Name;

            message.To.Add(new MailboxAddress(toDisplayName, authorUser.Email));
            message.Subject = "Book Details";

            var builder = new BodyBuilder();
            builder.TextBody = $"User Name: {authorUser.Name}\nEmail: {authorUser.Email}";

            // Attach PDF file
            builder.Attachments.Add("Book.pdf", pdfContent, ContentType.Parse("application/pdf"));

            // Attach Word file
            builder.Attachments.Add("Book.docx", wordContent, ContentType.Parse("application/vnd.openxmlformats-officedocument.wordprocessingml.document"));

            message.Body = builder.ToMessageBody();

            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, false);
                    await client.AuthenticateAsync("mayurrunwal9@gmail.com", "lhyb gcdp jfgl xxux"); 
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error sending email: {ex.Message}");
                throw; 
            }
        }

    }
}


