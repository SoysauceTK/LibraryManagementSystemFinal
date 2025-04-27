using System;
using System.ServiceModel;
using LMS.BorrowingService.Models;

namespace LMS.BorrowingService.Services
{
    /// <summary>
    /// Client proxy class for interacting with the BookStorage service
    /// </summary>
    public class BookServiceClient : IDisposable
    {
        private BookServiceReference.BookServiceClient _client;

        /// <summary>
        /// Constructor that initializes the client with default configuration
        /// </summary>
        public BookServiceClient()
        {
            _client = CreateBookServiceClient();
        }

        private BookServiceReference.BookServiceClient CreateBookServiceClient()
        {
            // Create a direct endpoint address and binding instead of relying on config
            var endpoint = new EndpointAddress("http://webstrar94.fulton.asu.edu/Page8/BookService.svc");
            var binding = new BasicHttpBinding
            {
                SendTimeout = TimeSpan.FromSeconds(30),
                ReceiveTimeout = TimeSpan.FromSeconds(30),
                MaxReceivedMessageSize = 2147483647,
                MaxBufferSize = 2147483647
            };

            return new BookServiceReference.BookServiceClient(binding, endpoint);
        }

        /// <summary>
        /// Gets a book by its ID
        /// </summary>
        /// <param name="id">The book ID to retrieve</param>
        /// <returns>The book if found, null otherwise</returns>
        public Book GetBookById(string id)
        {
            try
            {
                var book = _client.GetBookById(id);
                if (book == null)
                {
                    throw new InvalidOperationException($"Book with ID {id} not found");
                }
                
                return ConvertToLocalBookModel(book);
            }
            catch (EndpointNotFoundException ex)
            {
                throw new ServiceException("Book service is not available", ex);
            }
            catch (CommunicationException ex)
            {
                throw new ServiceException("Communication error with book service", ex);
            }
            catch (TimeoutException ex)
            {
                throw new ServiceException("Book service request timed out", ex);
            }
            catch (Exception ex)
            {
                throw new ServiceException($"Error getting book by ID: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates the inventory for a book
        /// </summary>
        /// <param name="update">The inventory update containing the book ID and quantity change</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool UpdateInventory(InventoryUpdate update)
        {
            try
            {
                var serviceUpdate = new BookServiceReference.InventoryUpdate
                {
                    BookId = update.BookId,
                    QuantityChange = update.QuantityChange
                };
                
                return _client.UpdateInventory(serviceUpdate);
            }
            catch (EndpointNotFoundException ex)
            {
                throw new ServiceException("Book service is not available", ex);
            }
            catch (CommunicationException ex)
            {
                throw new ServiceException("Communication error with book service", ex);
            }
            catch (TimeoutException ex)
            {
                throw new ServiceException("Book service request timed out", ex);
            }
            catch (Exception ex)
            {
                throw new ServiceException($"Error updating inventory: {ex.Message}", ex);
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
                Title = book.Title?.Trim() ?? "Unknown Title",
                Author = book.Author?.Trim() ?? "Unknown Author",
                ISBN = book.ISBN,
                Category = book.Category,
                PublicationYear = book.PublicationYear,
                Publisher = book.Publisher,
                CopiesAvailable = book.CopiesAvailable,
                Description = book.Description,
                CoverImageUrl = book.CoverImageUrl
            };
        }

        public void Dispose()
        {
            try
            {
                if (_client != null && _client.State != CommunicationState.Faulted)
                {
                    _client.Close();
                }
            }
            catch
            {
                _client.Abort();
            }
            finally
            {
                _client = null;
            }
        }
    }

    public class ServiceException : Exception
    {
        public ServiceException(string message) : base(message) { }
        public ServiceException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// This namespace contains proxy types generated from the service reference
    /// For demonstration purposes, these are defined here
    /// In a real implementation, these would be generated by the service reference
    /// </summary>
    namespace BookServiceReference
    {
        [ServiceContract]
        public interface IBookService
        {
            [OperationContract]
            Book GetBookById(string id);

            [OperationContract]
            bool UpdateInventory(InventoryUpdate update);
        }

        /// <summary>
        /// Book model from the BookStorage service
        /// </summary>
        public class Book
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
            public string ISBN { get; set; }
            public string Category { get; set; }
            public int PublicationYear { get; set; }
            public string Publisher { get; set; }
            public int CopiesAvailable { get; set; }
            public string Description { get; set; }
            public string CoverImageUrl { get; set; }
        }

        /// <summary>
        /// Inventory update model from the BookStorage service
        /// </summary>
        public class InventoryUpdate
        {
            public string BookId { get; set; }
            public int QuantityChange { get; set; }
        }

        /// <summary>
        /// Client proxy for the BookStorage service
        /// In a real implementation, this would be generated from the service reference
        /// </summary>
        public class BookServiceClient : ClientBase<IBookService>, IBookService
        {
            public BookServiceClient(string endpointConfigurationName) 
                : base(endpointConfigurationName)
            {
            }

            public Book GetBookById(string id)
            {
                return Channel.GetBookById(id);
            }

            public bool UpdateInventory(InventoryUpdate update)
            {
                return Channel.UpdateInventory(update);
            }
        }
    }
} 