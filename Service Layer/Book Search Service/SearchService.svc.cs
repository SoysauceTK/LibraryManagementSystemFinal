using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web.Hosting;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;

namespace LibraryManagementSystem.Service_Layer.Book_Search_Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class SearchService : ISearchService
    {
        private List<Book> _books;
        private readonly string _dataFilePath;
        private readonly object _lock = new object();

        public SearchService()
        {
            _dataFilePath = HostingEnvironment.MapPath("~/App_Data/books.json");
            ReloadBooks();
        }

        public void ReloadBooks()
        {
            lock (_lock)
            {
                try
                {
                    if (!File.Exists(_dataFilePath))
                    {
                        Trace.TraceWarning("Books data file not found, using empty list");
                        _books = new List<Book>();
                        return;
                    }

                    var json = File.ReadAllText(_dataFilePath);
                    _books = JsonConvert.DeserializeObject<List<Book>>(json) ?? new List<Book>();
                    Trace.TraceInformation($"Successfully loaded {_books.Count} books");
                }
                catch (Exception ex)
                {
                    Trace.TraceError($"Error loading books: {ex}");
                    _books = new List<Book>();
                }
            }
        }

        [WebGet(UriTemplate = "search?q={query}", ResponseFormat = WebMessageFormat.Json)]
        public List<Book> SearchBooks(string query)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                    return new List<Book>();

                query = query.ToLower();
                return _books.Where(b =>
                    (b.Title?.ToLower().Contains(query) ?? false) ||
                    (b.Author?.ToLower().Contains(query) ?? false) ||
                    (b.ISBN?.ToLower().Contains(query) ?? false) ||
                    (b.Category?.ToLower().Contains(query) ?? false) ||
                    (b.Description?.ToLower().Contains(query) ?? false)
                ).ToList();
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Search error: {ex}");
                throw new WebFaultException<string>("Search failed", HttpStatusCode.InternalServerError);
            }
        }

        [WebGet(UriTemplate = "advanced?title={title}&author={author}&category={category}&year={year}",
                ResponseFormat = WebMessageFormat.Json)]
        public List<Book> AdvancedSearch(string title, string author, string category, string year)
        {
            try
            {
                IEnumerable<Book> results = _books;

                if (!string.IsNullOrEmpty(title))
                    results = results.Where(b => b.Title?.ToLower().Contains(title.ToLower()) ?? false);

                if (!string.IsNullOrEmpty(author))
                    results = results.Where(b => b.Author?.ToLower().Contains(author.ToLower()) ?? false);

                if (!string.IsNullOrEmpty(category))
                    results = results.Where(b => b.Category?.ToLower().Contains(category.ToLower()) ?? false);

                if (!string.IsNullOrEmpty(year) && int.TryParse(year, out int yearValue))
                    results = results.Where(b => b.PublicationYear == yearValue);

                return results.ToList();
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Advanced search error: {ex}");
                throw new WebFaultException<string>("Advanced search failed", HttpStatusCode.InternalServerError);
            }
        }

        [WebGet(UriTemplate = "recommendations/{bookId}", ResponseFormat = WebMessageFormat.Json)]
        public List<Book> GetRecommendations(string bookId)
        {
            try
            {
                var book = _books.FirstOrDefault(b => b.Id == bookId);
                if (book == null)
                    return new List<Book>();

                var recommendations = _books.Where(b =>
                    b.Id != bookId &&
                    b.Category?.Equals(book.Category, StringComparison.OrdinalIgnoreCase) == true)
                    .Take(5)
                    .ToList();

                if (recommendations.Count < 5)
                {
                    var authorBooks = _books.Where(b =>
                        b.Id != bookId &&
                        b.Author?.Equals(book.Author, StringComparison.OrdinalIgnoreCase) == true &&
                        !recommendations.Any(r => r.Id == b.Id))
                        .Take(5 - recommendations.Count);

                    recommendations.AddRange(authorBooks);
                }

                return recommendations;
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Recommendations error: {ex}");
                throw new WebFaultException<string>("Failed to get recommendations", HttpStatusCode.InternalServerError);
            }
        }

        [WebGet(UriTemplate = "popular", ResponseFormat = WebMessageFormat.Json)]
        public List<Book> GetPopularBooks()
        {
            try
            {
                return _books
                    .OrderByDescending(b => b.CopiesAvailable)
                    .Take(10)
                    .ToList();
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Popular books error: {ex}");
                throw new WebFaultException<string>("Failed to get popular books", HttpStatusCode.InternalServerError);
            }
        }

        [WebGet(UriTemplate = "categories", ResponseFormat = WebMessageFormat.Json)]
        public List<string> GetAllCategories()
        {
            try
            {
                return _books
                    .Select(b => b.Category)
                    .Where(c => !string.IsNullOrEmpty(c))
                    .Distinct()
                    .ToList();
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Categories error: {ex}");
                throw new WebFaultException<string>("Failed to get categories", HttpStatusCode.InternalServerError);
            }
        }
    }
}