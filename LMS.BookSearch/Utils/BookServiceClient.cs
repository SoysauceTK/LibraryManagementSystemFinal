using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using LMS.BookSearch.Models;

namespace LMS.BookSearch.Utils
{
    public class BookServiceClient
    {
        private readonly string _serviceUrl;

        public BookServiceClient()
        {
            // For local development, use:
            _serviceUrl = "http://localhost:44301/BookService.svc";

            // For Webstrar deployment, use:
            // Replace X with your site number
            // _serviceUrl = "http://webstrarX.fulton.asu.edu/page0/BookService.svc";
        }

        public List<Book> GetAllBooks()
        {
            try
            {
                WebClient client = new WebClient();
                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Accept", "application/json");

                string response = client.DownloadString(_serviceUrl + "/GetAllBooks");

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Book>));
                using (MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(response)))
                {
                    return (List<Book>)serializer.ReadObject(ms);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calling Book Storage Service: {ex.Message}");

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
        }

        public Book GetBookById(string id)
        {
            try
            {
                WebClient client = new WebClient();
                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Accept", "application/json");

                string response = client.DownloadString(_serviceUrl + $"/book/{id}");

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Book));
                using (MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(response)))
                {
                    return (Book)serializer.ReadObject(ms);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calling Book Storage Service: {ex.Message}");
                return null;
            }
        }
    }
}