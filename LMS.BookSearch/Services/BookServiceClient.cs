using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using LMS.BookSearch.Models;

namespace LMS.BookSearch.Services
{
    /// <summary>
    /// Client proxy class for interacting with the BookStorage service
    /// </summary>
    public class BookServiceClient
    {
        private BookServiceReference.BookServiceClient _client;

        public BookServiceClient()
        {
            // Initialize the WCF client using the endpoint configuration
            _client = new BookServiceReference.BookServiceClient("BasicHttpBinding_IBookService");
        }

        /// <summary>
        /// Gets all books from the BookStorage service
        /// </summary>
        public List<Book> GetAllBooks()
        {
            try
            {
                // Get books from the service
                var books = _client.GetAllBooks();
                
                // Convert to local Book model
                return ConvertToLocalBookModels(books);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting books: {ex.Message}");
                return new List<Book>();
            }
        }

        /// <summary>
        /// Gets a book by its ID
        /// </summary>
        public Book GetBookById(string id)
        {
            try
            {
                var book = _client.GetBookById(id);
                if (book == null)
                    return null;
                
                return ConvertToLocalBookModel(book);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting book by ID: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Gets books by category
        /// </summary>
        public List<Book> GetBooksByCategory(string category)
        {
            try
            {
                var books = _client.GetBooksByCategory(category);
                return ConvertToLocalBookModels(books);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting books by category: {ex.Message}");
                return new List<Book>();
            }
        }

        /// <summary>
        /// Converts BookStorage service Book model to local Book model
        /// </summary>
        private Book ConvertToLocalBookModel(BookServiceReference.Book book)
        {
            if (book == null) return null;

            return new Book
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                Category = book.Category,
                PublicationYear = book.PublicationYear,
                Publisher = book.Publisher,
                CopiesAvailable = book.CopiesAvailable,
                Description = book.Description,
                CoverImageUrl = book.CoverImageUrl
            };
        }

        /// <summary>
        /// Converts a list of BookStorage service Book models to local Book models
        /// </summary>
        private List<Book> ConvertToLocalBookModels(IEnumerable<BookServiceReference.Book> books)
        {
            var result = new List<Book>();
            
            if (books == null) 
                return result;
                
            foreach (var book in books)
            {
                result.Add(ConvertToLocalBookModel(book));
            }
            
            return result;
        }
    }
} 