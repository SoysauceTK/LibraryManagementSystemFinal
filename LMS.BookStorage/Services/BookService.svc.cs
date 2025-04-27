using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using LMS.BookStorage.Models;
using LMS.BookStorage.Utils;

namespace LMS.BookStorage
{
    [ServiceContract]
    public interface IBookService
    {
        [OperationContract]
        List<Book> GetAllBooks();

        [OperationContract]
        Book GetBookById(string id);

        [OperationContract]
        Book AddBook(Book book);

        [OperationContract]
        Book UpdateBook(Book book);

        [OperationContract]
        bool DeleteBook(string id);

        [OperationContract]
        List<Book> GetBooksByCategory(string category);

        [OperationContract]
        bool UpdateInventory(InventoryUpdate update);
        
        [OperationContract]
        void SetDataPath(string path);
    }

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class BookService : IBookService
    {
        private XmlDataAccess<Book> _bookData;
        private const string DEFAULT_FILE_NAME = "books.xml";

        public BookService()
        {
            // Default initialization - client should call SetDataPath first for proper setup
            string filePath = DataConfiguration.GetDataFilePath(DEFAULT_FILE_NAME);
            _bookData = new XmlDataAccess<Book>(filePath);
        }

        public void SetDataPath(string path)
        {
            // Update the data path and reinitialize the data access
            DataConfiguration.DataPath = path;
            string filePath = DataConfiguration.GetDataFilePath(DEFAULT_FILE_NAME);
            _bookData = new XmlDataAccess<Book>(filePath);
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