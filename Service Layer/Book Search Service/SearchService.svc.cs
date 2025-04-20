// SearchService.svc.cs - Create in LMS.BookSearch project
// DEVELOPER: Sawyer Kesti - Book Search Service - [Date]
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace LMS.BookSearch
{
    public class SearchService : ISearchService
    {
        private readonly BookServiceClient _bookServiceClient;

        public SearchService()
        {
            _bookServiceClient = new BookServiceClient();
        }

        public List<Book> SearchBooks(string query)
        {
            if (string.IsNullOrEmpty(query))
                return new List<Book>();

            query = query.ToLower();

            // Get all books from the book storage service
            var allBooks = _bookServiceClient.GetAllBooks();

            // Filter books based on query
            return allBooks.Where(b =>
                b.Title.ToLower().Contains(query) ||
                b.Author.ToLower().Contains(query) ||
                b.ISBN.ToLower().Contains(query) ||
                b.Category.ToLower().Contains(query) ||
                b.Description.ToLower().Contains(query)
            ).ToList();
        }

        public List<Book> AdvancedSearch(string title, string author, string category, string year)
        {
            var allBooks = _bookServiceClient.GetAllBooks();

            // Start with all books
            IEnumerable<Book> results = allBooks;

            // Apply filters that have values
            if (!string.IsNullOrEmpty(title))
                results = results.Where(b => b.Title.ToLower().Contains(title.ToLower()));

            if (!string.IsNullOrEmpty(author))
                results = results.Where(b => b.Author.ToLower().Contains(author.ToLower()));

            if (!string.IsNullOrEmpty(category))
                results = results.Where(b => b.Category.ToLower().Contains(category.ToLower()));

            if (!string.IsNullOrEmpty(year) && int.TryParse(year, out int yearValue))
                results = results.Where(b => b.PublicationYear == yearValue);

            return results.ToList();
        }

        public List<Book> GetRecommendations(string bookId)
        {
            var allBooks = _bookServiceClient.GetAllBooks();
            var book = allBooks.FirstOrDefault(b => b.Id == bookId);

            if (book == null)
                return new List<Book>();

            // Find books in the same category
            var recommendations = allBooks.Where(b =>
                b.Id != bookId &&
                b.Category.Equals(book.Category, StringComparison.OrdinalIgnoreCase))
                .Take(5)
                .ToList();

            // If we have fewer than 5 recommendations, add some by the same author
            if (recommendations.Count < 5)
            {
                var authorBooks = allBooks.Where(b =>
                    b.Id != bookId &&
                    b.Author.Equals(book.Author, StringComparison.OrdinalIgnoreCase) &&
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
            return _bookServiceClient.GetAllBooks().Take(10).ToList();
        }

        public List<string> GetAllCategories()
        {
            var allBooks = _bookServiceClient.GetAllBooks();
            return allBooks.Select(b => b.Category).Distinct().ToList();
        }
    }

    // This would be a client to communicate with the Book Storage Service
    public class BookServiceClient
    {
        public List<Book> GetAllBooks()
        {
            // In a real implementation, this would make a call to the Book Storage Service
            // For now, we'll just return a sample list of books
            return new List<Book>
            {
                new Book { Id = "1", Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", ISBN = "9780743273565", Category = "Fiction", PublicationYear = 1925, Publisher = "Scribner", CopiesAvailable = 5, Description = "A novel about the American Dream" },
                new Book { Id = "2", Title = "To Kill a Mockingbird", Author = "Harper Lee", ISBN = "9780061120084", Category = "Fiction", PublicationYear = 1960, Publisher = "Harper Perennial", CopiesAvailable = 3, Description = "A novel about racial injustice" },
                // Add more sample books
            };
        }
    }
}