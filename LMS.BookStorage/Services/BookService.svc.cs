using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using LMS.BookStorage.Models;
using LMS.BookStorage.Utils;

namespace LMS.BookStorage
{
    // DEVELOPER: Aarya Baireddy - Book Storage Service - [Date]
    [ServiceContract]
    public interface IBookService
    {
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        List<Book> GetAllBooks();

        [OperationContract]
        [WebGet(UriTemplate = "book/{id}", ResponseFormat = WebMessageFormat.Json)]
        Book GetBookById(string id);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "add", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Book AddBook(Book book);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Book UpdateBook(Book book);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "delete/{id}", ResponseFormat = WebMessageFormat.Json)]
        bool DeleteBook(string id);

        [OperationContract]
        [WebGet(UriTemplate = "category/{category}", ResponseFormat = WebMessageFormat.Json)]
        List<Book> GetBooksByCategory(string category);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "inventory/update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        bool UpdateInventory(InventoryUpdate update);
    }

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class BookService : IBookService
    {
        private readonly JsonDataAccess<Book> _bookData;

        public BookService()
        {
            _bookData = new JsonDataAccess<Book>("books.json");
        }

        public List<Book> GetAllBooks()
        {
            return _bookData.GetAll();
        }

        public Book GetBookById(string id)
        {
            return _bookData.GetById(b => b.Id == id);
        }

        public Book AddBook(Book book)
        {
            if (string.IsNullOrEmpty(book.Id))
            {
                book.Id = Guid.NewGuid().ToString();
            }

            _bookData.Insert(book);
            return book;
        }

        public Book UpdateBook(Book book)
        {
            _bookData.Update(book, b => b.Id == book.Id);
            return book;
        }

        public bool DeleteBook(string id)
        {
            try
            {
                _bookData.Delete(b => b.Id == id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Book> GetBooksByCategory(string category)
        {
            var books = _bookData.GetAll();
            return books.Where(b => b.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public bool UpdateInventory(InventoryUpdate update)
        {
            var book = GetBookById(update.BookId);
            if (book == null)
                return false;

            book.CopiesAvailable += update.QuantityChange;
            if (book.CopiesAvailable < 0)
                book.CopiesAvailable = 0;

            _bookData.Update(book, b => b.Id == book.Id);
            return true;
        }
    }
}