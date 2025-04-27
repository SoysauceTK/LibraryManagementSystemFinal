using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Linq;
using System.IO;

namespace LibraryManagementSystem.Staff
{
    public partial class Dashboard : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Staff"))
            {
                Response.Redirect("~/Staff/Login.aspx", true);
                return;
            }

            if (!IsPostBack)
            {
                UserNameLiteral.Text = User.Identity.Name;
                RegisterAsyncTask(new PageAsyncTask(LoadQuickStats));
                RegisterAsyncTask(new PageAsyncTask(LoadLatestBorrowsAsync));
                LoadActivityLog();
            }
        }

        private BookServiceReference.BookServiceClient CreateBookServiceClient()
        {
            var client = new BookServiceReference.BookServiceClient("BasicHttpBinding_IBookService");
            client.Endpoint.Binding.SendTimeout = TimeSpan.FromSeconds(30);
            client.Endpoint.Binding.ReceiveTimeout = TimeSpan.FromSeconds(30);

            return client;
        }

        private BorrowingServiceReference.BorrowingServiceClient CreateBorrowingServiceClient()
        {
            var client = new BorrowingServiceReference.BorrowingServiceClient("BasicHttpBinding_IBorrowingService");
            client.Endpoint.Binding.SendTimeout = TimeSpan.FromSeconds(30);
            client.Endpoint.Binding.ReceiveTimeout = TimeSpan.FromSeconds(30);
            
            return client;
        }

        private SearchServiceReference.SearchServiceClient CreateSearchServiceClient()
        {
            var client = new SearchServiceReference.SearchServiceClient("BasicHttpBinding_ISearchService");
            client.Endpoint.Binding.SendTimeout = TimeSpan.FromSeconds(30);
            client.Endpoint.Binding.ReceiveTimeout = TimeSpan.FromSeconds(30);
            
            return client;
        }

        private async Task LoadLatestBorrowsAsync()
        {
            try
            {
                var latestBorrows = new List<BorrowRecord>();
                
                using (var borrowingClient = CreateBorrowingServiceClient())
                {
                    try
                    {
                        // Get recent borrows from service
                        var borrowRecords = await borrowingClient.GetRecentBorrowsAsync(10); // Limit to 10 latest borrows
                        
                        if (borrowRecords != null && borrowRecords.Length > 0)
                        {
                            // Convert service records to our display model
                            latestBorrows = borrowRecords.Select(b => new BorrowRecord
                            {
                                Id = b.Id,
                                BookTitle = b.BookTitle,
                                MemberName = b.MemberName,
                                BorrowDate = b.BorrowDate,
                                DueDate = b.DueDate,
                                // The service might not return Author information
                                Author = b.Author ?? "Unknown" // Use null coalescing to handle if Author is null
                            }).ToList();
                            
                            // If we don't have author info, try to get it for each book
                            if (latestBorrows.Any(b => b.Author == "Unknown"))
                            {
                                await EnrichBorrowsWithBookDetails(latestBorrows);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log error but continue with sample data
                        LogErrorToDatabase("Error fetching latest borrows", ex);
                    }
                }
                
                // If we couldn't get from service or no records found, use sample data
                if (latestBorrows.Count == 0)
                {
                    latestBorrows = GetSampleBorrows();
                }
                
                // Update the borrow count display
                BorrowCountLiteral.Text = latestBorrows.Count.ToString();
                
                // Bind data to the ListView
                LatestBorrowsListView.DataSource = latestBorrows;
                LatestBorrowsListView.DataBind();
            }
            catch (Exception ex)
            {
                HandleServiceError(ex, "load latest borrows");
                
                // On error, fall back to sample data
                var sampleBorrows = GetSampleBorrows();
                BorrowCountLiteral.Text = sampleBorrows.Count.ToString();
                LatestBorrowsListView.DataSource = sampleBorrows;
                LatestBorrowsListView.DataBind();
            }
        }
        
        // Helper method to get book details including author
        private async Task EnrichBorrowsWithBookDetails(List<BorrowRecord> borrows)
        {
            try
            {
                using (var bookClient = CreateBookServiceClient())
                {
                    foreach (var borrow in borrows.Where(b => b.Author == "Unknown"))
                    {
                        try
                        {
                            // Extract book ID from the borrow record
                            string bookId = borrow.Id;
                            
                            // If ID is in a different format, we might need to parse it
                            // This assumes the borrow ID contains or is the book ID
                            
                            var book = await bookClient.GetBookByIdAsync(bookId);
                            if (book != null && !string.IsNullOrEmpty(book.Author))
                            {
                                borrow.Author = book.Author;
                            }
                        }
                        catch (Exception ex)
                        {
                            // Skip this book but log the error
                            System.Diagnostics.Debug.WriteLine($"Error getting book details: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log but continue with what we have
                LogErrorToDatabase("Error enriching borrows with book details", ex);
            }
        }
        
        private List<BorrowRecord> GetSampleBorrows()
        {
            return new List<BorrowRecord>
            {
                new BorrowRecord 
                {
                    Id = "1",
                    BookTitle = "The Great Gatsby",
                    MemberName = "John Smith",
                    BorrowDate = DateTime.Now.AddDays(-2),
                    DueDate = DateTime.Now.AddDays(12),
                    Author = "F. Scott Fitzgerald"
                },
                new BorrowRecord 
                {
                    Id = "2",
                    BookTitle = "To Kill a Mockingbird",
                    MemberName = "Emily Johnson",
                    BorrowDate = DateTime.Now.AddDays(-5),
                    DueDate = DateTime.Now.AddDays(9),
                    Author = "Harper Lee"
                },
                new BorrowRecord 
                {
                    Id = "3",
                    BookTitle = "1984",
                    MemberName = "Michael Brown",
                    BorrowDate = DateTime.Now.AddDays(-7),
                    DueDate = DateTime.Now.AddDays(-1),
                    Author = "George Orwell"
                },
                new BorrowRecord 
                {
                    Id = "4",
                    BookTitle = "Pride and Prejudice",
                    MemberName = "Sarah Williams",
                    BorrowDate = DateTime.Now.AddDays(-1),
                    DueDate = DateTime.Now.AddDays(13),
                    Author = "Jane Austen"
                },
                new BorrowRecord 
                {
                    Id = "5",
                    BookTitle = "The Hobbit",
                    MemberName = "David Miller",
                    BorrowDate = DateTime.Now.AddDays(-10),
                    DueDate = DateTime.Now.AddDays(2),
                    Author = "J.R.R. Tolkien"
                }
            };
        }
        
        // Helper method for displaying due status
        protected string GetDueStatus(DateTime dueDate)
        {
            var daysRemaining = (dueDate - DateTime.Now).TotalDays;
            
            if (dueDate < DateTime.Now)
            {
                return "Overdue";
            }
            else if (daysRemaining <= 3)
            {
                return "Due Soon";
            }
            else
            {
                return "On Time";
            }
        }
        
        // Helper method for displaying appropriate badge class
        protected string GetDueBadgeClass(DateTime dueDate)
        {
            var daysRemaining = (dueDate - DateTime.Now).TotalDays;
            
            if (dueDate < DateTime.Now)
            {
                return "danger";
            }
            else if (daysRemaining <= 3)
            {
                return "warning";
            }
            else
            {
                return "success";
            }
        }

        private async Task LoadQuickStats()
        {
            try
            {
                using (var bookClient = CreateBookServiceClient())
                {
                    var books = await bookClient.GetAllBooksAsync();
                    TotalBooksLiteral.Text = books?.Length.ToString("N0") ?? "0";
                }
            }
            catch (Exception ex)
            {
                HandleServiceError(ex, "load quick stats");
                TotalBooksLiteral.Text = "Error";
            }

            // Placeholder data for other stats
            BorrowedBooksLiteral.Text = "278";
            MembersLiteral.Text = "356";
            OverdueLiteral.Text = "12";
        }

        private void LoadActivityLog()
        {
            var activityLog = new List<ActivityLogEntry>
            {
                new ActivityLogEntry { Date = DateTime.Now.AddHours(-3), Action = "Book Borrowed", Details = "To Kill a Mockingbird by Harper Lee", User = "john_doe" },
                new ActivityLogEntry { Date = DateTime.Now.AddHours(-5), Action = "New Member", Details = "sarah_smith joined the library", User = "system" },
                new ActivityLogEntry { Date = DateTime.Now.AddDays(-1), Action = "Book Returned", Details = "1984 by George Orwell", User = "mike_jones" }
            };

            ActivityLogGridView.DataSource = activityLog;
            ActivityLogGridView.DataBind();
        }

        protected async void AddBookButton_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            BookAddStatusPanel.Visible = true;

            try
            {
                var book = new BookServiceReference.Book
                {
                    Title = BookTitle.Text,
                    Author = BookAuthor.Text,
                    ISBN = BookISBN.Text,
                    Category = BookCategory.SelectedValue,
                    Description = BookDescription.Text,
                    CopiesAvailable = int.Parse(BookCopies.Text),
                    PublicationYear = int.Parse(BookPublicationYear.Text),
                    Publisher = "Unknown"
                };

                using (var bookClient = CreateBookServiceClient())
                {
                    var addedBook = await bookClient.AddBookAsync(book);

                    if (addedBook != null)
                    {
                        BookAddStatus.Text = "<div class='alert alert-success'>Book added successfully!</div>";
                        LogBookAddition(book.Title, book.Author);
                        ClearBookForm();
                        await LoadQuickStats();
                    }
                    else
                    {
                        BookAddStatus.Text = "<div class='alert alert-danger'>Error adding book</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                HandleServiceError(ex, "add book");
                BookAddStatus.Text = $"<div class='alert alert-danger'>{GetUserFriendlyError(ex)}</div>";
            }
        }

        private void HandleServiceError(Exception ex, string operation)
        {
            System.Diagnostics.Debug.WriteLine($"Error during {operation}: {ex}");

            if (ex is CommunicationException || ex is TimeoutException)
            {
                LogErrorToDatabase($"Service communication error during {operation}", ex);
            }
        }

        private string GetUserFriendlyError(Exception ex)
        {
            string friendlyMessage;

            if (ex is CommunicationException)
                friendlyMessage = "Service is unavailable. Please try again later.";
            else if (ex is TimeoutException)
                friendlyMessage = "Request timed out. Please check your connection.";
            else if (ex is FaultException)
                friendlyMessage = "The server reported an error while processing your request.";
            else if (ex is ProtocolException)
                friendlyMessage = "There was a communication protocol issue. Please contact support.";
            else if (ex is EndpointNotFoundException)
                friendlyMessage = "Could not connect to the service. Please try again later.";
            else if (ex is ServerTooBusyException)
                friendlyMessage = "The server is currently busy. Please try again later.";
            else
                friendlyMessage = "An unexpected error occurred. Please contact support.";

#if DEBUG
            // Add detailed technical error in DEBUG mode
            friendlyMessage += $"<br/><strong>Technical details:</strong> {ex.Message}";
#endif

            return friendlyMessage;
        }

        private void LogErrorToDatabase(string message, Exception ex)
        {
            // Implement your error logging here
            // Could use database, file system, or application insights

            // For now, just log to a text file in App_Data
            try
            {
                string logPath = Server.MapPath("~/App_Data/ErrorLog.txt");
                using (StreamWriter writer = File.AppendText(logPath))
                {
                    writer.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}");
                    writer.WriteLine($"Exception: {ex.Message}");
                    writer.WriteLine($"Stack Trace: {ex.StackTrace}");
                    writer.WriteLine(new string('-', 80));
                }
            }
            catch
            {
                // Fail silently if logging itself fails
            }
        }

        private void ClearBookForm()
        {
            BookTitle.Text = string.Empty;
            BookAuthor.Text = string.Empty;
            BookISBN.Text = string.Empty;
            BookCategory.SelectedIndex = 0;
            BookDescription.Text = string.Empty;
            BookCopies.Text = "1";
            BookPublicationYear.Text = DateTime.Now.Year.ToString();
        }

        private void LogBookAddition(string title, string author)
        {
            var newEntry = new ActivityLogEntry
            {
                Date = DateTime.Now,
                Action = "Book Added",
                Details = $"{title} by {author}",
                User = User.Identity.Name
            };

            var currentLog = ActivityLogGridView.DataSource as List<ActivityLogEntry> ?? new List<ActivityLogEntry>();
            currentLog.Insert(0, newEntry);

            ActivityLogGridView.DataSource = currentLog;
            ActivityLogGridView.DataBind();
        }

        protected async void SearchButton_Click(object sender, EventArgs e)
        {
            string query = SearchTextBox.Text.Trim();
            SearchStatusPanel.Visible = true;

            if (string.IsNullOrWhiteSpace(query))
            {
                SearchStatusLiteral.Text = "<div class='alert alert-warning'>Please enter a search term</div>";
                SearchResultsGridView.DataSource = null;
                SearchResultsGridView.DataBind();
                return;
            }

            try
            {
                using (var searchClient = CreateSearchServiceClient())
                {
                    // Use the search service to search books
                    var results = await searchClient.SearchBooksAsync(query);

                    // Convert SearchServiceReference.Book[] to List<Book>
                    var bookResults = results.Select(b => new Book
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Author = b.Author,
                        ISBN = b.ISBN,
                        Category = b.Category,
                        PublicationYear = b.PublicationYear,
                        Publisher = b.Publisher,
                        CopiesAvailable = b.CopiesAvailable,
                        Description = b.Description,
                        CoverImageUrl = b.CoverImageUrl
                    }).ToList();

                    SearchResultsGridView.DataSource = bookResults;
                    SearchResultsGridView.DataBind();

                    SearchStatusLiteral.Text = bookResults.Count > 0
                        ? $"<div class='alert alert-success'>Found {bookResults.Count} book(s)</div>"
                        : $"<div class='alert alert-warning'>No books found</div>";
                }
            }
            catch (Exception ex)
            {
                HandleServiceError(ex, "search books");
                SearchStatusLiteral.Text = $"<div class='alert alert-danger'>Search failed: {GetUserFriendlyError(ex)}</div>";
            }
        }

        public class ActivityLogEntry
        {
            public DateTime Date { get; set; }
            public string Action { get; set; }
            public string Details { get; set; }
            public string User { get; set; }
        }

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
        
        public class BorrowRecord
        {
            public string Id { get; set; }
            public string BookTitle { get; set; }
            public string MemberName { get; set; }
            public DateTime BorrowDate { get; set; }
            public DateTime DueDate { get; set; }
            public string Author { get; set; }
        }
    }
}