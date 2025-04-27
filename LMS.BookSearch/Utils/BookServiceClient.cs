using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using LMS.BookSearch.Models;

namespace LMS.BookSearch.Utils
{
    public class BookServiceClient : IDisposable
    {
        private readonly BasicHttpBinding _binding;
        private readonly EndpointAddress _endpoint;
        private readonly ChannelFactory<IBookStorageService> _factory;

        public BookServiceClient()
        {
            // For local development
            _binding = new BasicHttpBinding();

            // For local development, use:
            _endpoint = new EndpointAddress("http://localhost:44301/BookService.svc");

            // For Webstrar deployment, use:
            // Replace X with your site number
            // _endpoint = new EndpointAddress("http://webstrar94.fulton.asu.edu/page0/BookService.svc");

            _factory = new ChannelFactory<IBookStorageService>(_binding, _endpoint);
        }

        public List<Book> GetAllBooks()
        {
            try
            {
                var channel = _factory.CreateChannel();

                try
                {
                    // Convert the returned books to our model
                    var storageBooks = channel.GetAllBooks();
                    var books = new List<Book>();

                    foreach (var book in storageBooks)
                    {
                        books.Add(new Book
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
                        });
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

    // Define the service contract without referencing LMS.BookStorage
    [ServiceContract]
    public interface IBookStorageService
    {
        [OperationContract]
        System.Collections.Generic.List<BookStorageServiceBook> GetAllBooks();

        [OperationContract]
        BookStorageServiceBook GetBookById(string id);
    }

    // Create a mirror class to match the structure from BookStorage service
    [DataContract]
    public class BookStorageServiceBook
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Author { get; set; }

        [DataMember]
        public string ISBN { get; set; }

        [DataMember]
        public string Category { get; set; }

        [DataMember]
        public int PublicationYear { get; set; }

        [DataMember]
        public string Publisher { get; set; }

        [DataMember]
        public int CopiesAvailable { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string CoverImageUrl { get; set; }
    }
}