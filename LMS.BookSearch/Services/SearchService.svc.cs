// LMS.BookSearch/Services/SearchService.svc.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Runtime.Serialization;
using LMS.BookSearch.Models;
using LMS.BookSearch.Utils;
using System.IO;

namespace LMS.BookSearch
{
    [ServiceContract]
    public interface ISearchService
    {
        [OperationContract]
        List<Book> SearchBooks(string query);

        [OperationContract]
        List<Book> AdvancedSearch(string title, string author, string category, string year);

        [OperationContract]
        List<Book> GetRecommendations(string bookId);

        [OperationContract]
        List<Book> GetPopularBooks();

        [OperationContract]
        List<string> GetAllCategories();

        [OperationContract]
        void SetDataPath(string path);
    }

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class SearchService : ISearchService
    {
        private XmlDataAccess<Book> _bookData;
        private const string DEFAULT_FILE_NAME = "books.xml";

        public SearchService()
        {
            // Default initialization - client should call SetDataPath first for proper setup
            string filePath = DataConfiguration.GetDataFilePath(DEFAULT_FILE_NAME);
            _bookData = new XmlDataAccess<Book>(filePath);
        }

        public void SetDataPath(string path)
        {
            // Update the data path and reinitialize the data access
            DataConfiguration.DataPath = path;
            
            // Ensure directory exists
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error creating directory: {ex.Message}");
                    // Continue anyway, maybe the directory will be created by another method
                }
            }
            
            string filePath = DataConfiguration.GetDataFilePath(DEFAULT_FILE_NAME);
            _bookData = new XmlDataAccess<Book>(filePath);
        }

        public List<Book> SearchBooks(string query)
        {
            if (string.IsNullOrEmpty(query))
                return new List<Book>();

            query = query.ToLower();

            // Get all books directly from the XML file
            var allBooks = _bookData.GetAll();

            // Filter books based on query
            return allBooks.Where(b =>
                b.Title?.ToLower().Contains(query) == true ||
                b.Author?.ToLower().Contains(query) == true ||
                b.ISBN?.ToLower().Contains(query) == true ||
                b.Category?.ToLower().Contains(query) == true ||
                b.Description?.ToLower().Contains(query) == true
            ).ToList();
        }

        public List<Book> AdvancedSearch(string title, string author, string category, string year)
        {
            var allBooks = _bookData.GetAll();

            // Start with all books
            IEnumerable<Book> results = allBooks;

            // Apply filters that have values
            if (!string.IsNullOrEmpty(title))
                results = results.Where(b => b.Title?.ToLower().Contains(title.ToLower()) == true);

            if (!string.IsNullOrEmpty(author))
                results = results.Where(b => b.Author?.ToLower().Contains(author.ToLower()) == true);

            if (!string.IsNullOrEmpty(category))
                results = results.Where(b => b.Category?.ToLower().Contains(category.ToLower()) == true);

            if (!string.IsNullOrEmpty(year) && int.TryParse(year, out int yearValue))
                results = results.Where(b => b.PublicationYear == yearValue);

            return results.ToList();
        }

        public List<Book> GetRecommendations(string bookId)
        {
            var allBooks = _bookData.GetAll();
            var book = allBooks.FirstOrDefault(b => b.Id == bookId);

            if (book == null)
                return new List<Book>();

            // Find books in the same category
            var recommendations = allBooks.Where(b =>
                b.Id != bookId &&
                b.Category?.Equals(book.Category, StringComparison.OrdinalIgnoreCase) == true)
                .Take(5)
                .ToList();

            // If we have fewer than 5 recommendations, add some by the same author
            if (recommendations.Count < 5)
            {
                var authorBooks = allBooks.Where(b =>
                    b.Id != bookId &&
                    b.Author?.Equals(book.Author, StringComparison.OrdinalIgnoreCase) == true &&
                    !recommendations.Any(r => r.Id == b.Id))
                    .Take(5 - recommendations.Count);

                recommendations.AddRange(authorBooks);
            }

            return recommendations;
        }

        public List<Book> GetPopularBooks()
        {
            // In a real scenario, this would be based on borrow records
            // Here we're just returning the first 10 books as an example
            return _bookData.GetAll().Take(10).ToList();
        }

        public List<string> GetAllCategories()
        {
            var allBooks = _bookData.GetAll();
            return allBooks.Select(b => b.Category).Where(c => !string.IsNullOrEmpty(c)).Distinct().ToList();
        }
    }
}