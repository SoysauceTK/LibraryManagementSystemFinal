using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web;

namespace LibraryManagementSystem.Service_Layer.Book_Storage_Service
{
    public class BookStorage : IBookStorage
    {
        private readonly JsonDataAccess<Book> _bookData;

        public BookStorage()
        {
            _bookData = new JsonDataAccess<Book>(HttpContext.Current.Server.MapPath("~/App_Data/books.json"));
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
