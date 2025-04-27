using System;
using System.Collections.Generic;
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
            // Initialize the WCF client using the endpoint configuration
            _client = new BookServiceReference.BookServiceClient("BasicHttpBinding_IBookService");
            
            // Configure timeouts
            _client.Endpoint.Binding.SendTimeout = TimeSpan.FromSeconds(30);
            _client.Endpoint.Binding.ReceiveTimeout = TimeSpan.FromSeconds(30);
            
            // Configure message sizes if needed
            if (_client.Endpoint.Binding is BasicHttpBinding binding)
            {
                binding.MaxReceivedMessageSize = 2147483647;
                binding.MaxBufferSize = 2147483647;
            }
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
} 