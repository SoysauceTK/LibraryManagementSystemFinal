using System;
using System.Collections.Generic;
using System.ServiceModel;
using LMS.BookSearch.Models;
using LMS.BookStorage.Models;

namespace LMS.BookSearch.Utils
{
    public class BookServiceClient : IDisposable
    {
        private readonly ServiceModel.BasicHttpBinding _binding;
        private readonly EndpointAddress _endpoint;
        private readonly ServiceModel.ChannelFactory<IBookStorageService> _factory;

        public BookServiceClient()
        {
            // For local development
            _binding = new ServiceModel.BasicHttpBinding();

            // For local development, use:
            _endpoint = new EndpointAddress("http://localhost:44301/BookService.svc");

            // For Webstrar deployment, use:
            // Replace X with your site number
            // _endpoint = new EndpointAddress("http://webstrarX.fulton.asu.edu/page0/BookService.svc");

            _factory = new ServiceModel.ChannelFactory<IBookStorageService>(_binding, _endpoint);
        }

        public List<Book> GetAllBooks()
        {
            try
            {
                var channel = _factory.CreateChannel();

                try
                {
                    var storageBooks = channel.GetAllBooks();
                    var books = new List<Book>();

                    foreach (var book in storageBooks)
                    {
                        books.Add(ConvertToSearchBook(book));
                    }

                    return books;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error calling Book Storage Service: {ex.Message}");
                    ((ICommunicationObject)channel).Abort();

                    // Return sample data if service fails
                    return GetSampleBooks();
                }
                finally
                {
                    if (channel != null)
                    {
                        try
                        {
                            ((ICommunicationObject)channel).Close();
                        }
                        catch
                        {
                            ((ICommunicationObject)channel).Abort();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating channel to Book Storage Service: {ex.Message}");
                return GetSampleBooks();
            }
        }

        private Book ConvertToSearchBook(LMS.BookStorage.Models.Book storageBook)
        {
            return new Book
            {
                Id = storageBook.Id,
                Title = storageBook.Title,
                Author = storageBook.Author,
                ISBN = storageBook.ISBN,
                Category = storageBook.Category,
                PublicationYear = storageBook.PublicationYear,
                Publisher = storageBook.Publisher,
                CopiesAvailable = storageBook.CopiesAvailable,
                Description = storageBook.Description,
                CoverImageUrl = storageBook.CoverImageUrl
            };
        }

        private List<Book> GetSampleBooks()
        {
            // Return some sample data if the service is unavailable
            return new List<Book>
            {
                new Book {
                    Id = "1",
                    Title = "The Great Gatsby",
                    Author = "F. Scott Fitzgerald",
                    ISBN = "9780743273565",
                    Category = "Fiction",
                    PublicationYear = 1925,
                    Publisher = "Scribner",
                    CopiesAvailable = 5,
                    Description = "A novel about the American Dream"
                },
                new Book {
                    Id = "2",
                    Title = "To Kill a Mockingbird",
                    Author = "Harper Lee",
                    ISBN = "9780061120084",
                    Category = "Fiction",
                    PublicationYear = 1960,
                    Publisher = "Harper Perennial",
                    CopiesAvailable = 3,
                    Description = "A novel about racial injustice"
                },
                new Book {
                    Id = "3",
                    Title = "1984",
                    Author = "George Orwell",
                    ISBN = "9780451524935",
                    Category = "Science Fiction",
                    PublicationYear = 1949,
                    Publisher = "Signet Classic",
                    CopiesAvailable = 7,
                    Description = "A dystopian novel about totalitarianism"
                }
            };
        }

        public void Dispose()
        {
            if (_factory != null)
            {
                try
                {
                    _factory.Close();
                }
                catch
                {
                    _factory.Abort();
                }
            }
        }
    }

    [ServiceContract]
    public interface IBookStorageService
    {
        [OperationContract]
        List<LMS.BookStorage.Models.Book> GetAllBooks();

        [OperationContract]
        LMS.BookStorage.Models.Book GetBookById(string id);
    }
}