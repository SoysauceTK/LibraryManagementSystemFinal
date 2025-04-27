using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Xml;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Linq;
using System.IO;

namespace LibraryManagementSystem.Member
{
    public partial class Dashboard : Page
    {
        // Controls from ASPX file
        protected System.Web.UI.WebControls.Panel AlertPanel;
        protected System.Web.UI.WebControls.Literal AlertMessage;
        protected System.Web.UI.WebControls.GridView BorrowHistoryGridView;
        
        // List to store borrow history
        private static Dictionary<string, List<BorrowHistory>> _userBorrowHistory = new Dictionary<string, List<BorrowHistory>>();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Member/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                // Load user data
                LoadUserData();

                // Load borrowed books using borrowing service
                RegisterAsyncTask(new PageAsyncTask(LoadBorrowedBooksAsync));

                // Load real book recommendations using async task
                RegisterAsyncTask(new PageAsyncTask(LoadRecommendedBooksAsync));
                
                // Load borrow history
                LoadBorrowHistory();
            }
        }

        private void LoadUserData()
        {
            try
            {
                string username = User.Identity.Name;
                UserNameLiteral.Text = username;
                UsernameLabel.Text = username;

                // Load user details from XML
                XmlDocument doc = new XmlDocument();
                string path = Server.MapPath("~/App_Data/Member.xml");
                doc.Load(path);

                XmlNode userNode = doc.SelectSingleNode($"//user[username='{username}']");
                if (userNode != null)
                {
                    // Display user details
                    EmailLabel.Text = userNode.SelectSingleNode("email")?.InnerText ?? "N/A";

                    string registrationDateStr = userNode.SelectSingleNode("registrationDate")?.InnerText;
                    if (!string.IsNullOrEmpty(registrationDateStr) && DateTime.TryParse(registrationDateStr, out DateTime registrationDate))
                    {
                        MemberSinceLabel.Text = registrationDate.ToShortDateString();
                    }
                    else
                    {
                        MemberSinceLabel.Text = "N/A";
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error
                System.Diagnostics.Debug.WriteLine($"Error loading user data: {ex.Message}");
                LogError("Error loading user data", ex);
            }
        }

        private async Task LoadBorrowedBooksAsync()
        {
            try
            {
                string username = User.Identity.Name;
                var borrowList = new List<BorrowedBook>();

                using (var borrowingClient = CreateBorrowingServiceClient())
                {
                    try
                    {
                        // Get current borrows from the borrowing service
                        var borrowRecords = await borrowingClient.GetCurrentBorrowsByUserAsync(username);

                        if (borrowRecords != null && borrowRecords.Length > 0)
                        {
                            // Map service data to our UI model
                            borrowList = borrowRecords.Select(b => new BorrowedBook
                            {
                                Id = b.Id,
                                Title = b.BookTitle,
                                Author = "Unknown", // BorrowRecord doesn't have Author property
                                BorrowDate = b.BorrowDate,
                                DueDate = b.DueDate
                            }).ToList();
                            
                            // Always try to get author information as it's not in BorrowRecord
                            await EnrichBorrowsWithAuthorInfo(borrowList);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the error
                        System.Diagnostics.Debug.WriteLine($"Error with BorrowingService: {ex.Message}");
                        LogError("BorrowingService unavailable", ex);
                    }
                }

                BorrowedBooksGridView.DataSource = borrowList;
                BorrowedBooksGridView.DataBind();
            }
            catch (Exception ex)
            {
                // Log error
                System.Diagnostics.Debug.WriteLine($"Error loading borrowed books: {ex.Message}");
                LogError("Error loading borrowed books", ex);
                
                // Show empty grid
                BorrowedBooksGridView.DataSource = new List<BorrowedBook>();
                BorrowedBooksGridView.DataBind();
            }
        }
        
        // Helper method to enrich borrows with author information
        private async Task EnrichBorrowsWithAuthorInfo(List<BorrowedBook> borrows)
        {
            foreach (var borrow in borrows.Where(b => b.Author == "Unknown"))
            {
                try
                {
                    using (var bookClient = CreateBookServiceClient())
                    {
                        var book = await bookClient.GetBookByIdAsync(borrow.Id);
                        if (book != null && !string.IsNullOrEmpty(book.Author))
                        {
                            borrow.Author = book.Author;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Just log and continue
                    System.Diagnostics.Debug.WriteLine($"Error getting author info: {ex.Message}");
                    LogError("Error getting author details", ex);
                }
            }
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

        private BookServiceReference.BookServiceClient CreateBookServiceClient()
        {
            var client = new BookServiceReference.BookServiceClient("BasicHttpBinding_IBookService");
            client.Endpoint.Binding.SendTimeout = TimeSpan.FromSeconds(30);
            client.Endpoint.Binding.ReceiveTimeout = TimeSpan.FromSeconds(30);
            
            return client;
        }

        private async Task LoadRecommendedBooksAsync()
        {
            try
            {
                // First try to use the SearchService
                using (var searchClient = CreateSearchServiceClient())
                {
                    try
                    {
                        // Get popular books from the search service
                        var popularBooks = await searchClient.GetPopularBooksAsync();

                        if (popularBooks != null && popularBooks.Length > 0)
                        {
                            RecommendedBooksRepeater.DataSource = popularBooks;
                            RecommendedBooksRepeater.DataBind();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the error but continue to try BookService instead
                        System.Diagnostics.Debug.WriteLine($"Error with SearchService: {ex.Message}");
                        LogError("SearchService unavailable", ex);
                    }
                }

                // If SearchService failed, try to get some books from BookService
                using (var bookClient = CreateBookServiceClient())
                {
                    var allBooks = await bookClient.GetAllBooksAsync();
                    
                    if (allBooks != null && allBooks.Length > 0)
                    {
                        // Get up to 5 random books as recommendations
                        var rand = new Random();
                        var recommendations = allBooks
                            .OrderBy(x => rand.Next())
                            .Take(5)
                            .ToArray();
                        
                        RecommendedBooksRepeater.DataSource = recommendations;
                        RecommendedBooksRepeater.DataBind();
                        return;
                    }
                }

                // If all else fails, use sample data
                var sampleBooks = GetSampleRecommendedBooks();
                RecommendedBooksRepeater.DataSource = sampleBooks;
                RecommendedBooksRepeater.DataBind();
            }
            catch (Exception ex)
            {
                // Log error
                System.Diagnostics.Debug.WriteLine($"Error loading recommended books: {ex.Message}");
                LogError("Error loading recommended books", ex);

                // On error, fall back to sample data
                var sampleBooks = GetSampleRecommendedBooks();
                RecommendedBooksRepeater.DataSource = sampleBooks;
                RecommendedBooksRepeater.DataBind();
            }
        }

        // Sample recommendations as fallback if the service is unavailable
        private List<Book> GetSampleRecommendedBooks()
        {
            return new List<Book>
            {
                new Book
                {
                    Id = "3",
                    Title = "1984",
                    Author = "George Orwell",
                    Category = "Science Fiction",
                    Description = "A dystopian novel about totalitarianism"
                },
                new Book
                {
                    Id = "4",
                    Title = "Pride and Prejudice",
                    Author = "Jane Austen",
                    Category = "Classic",
                    Description = "A romantic novel of manners"
                },
                new Book
                {
                    Id = "5",
                    Title = "The Hobbit",
                    Author = "J.R.R. Tolkien",
                    Category = "Fantasy",
                    Description = "A fantasy novel and children's book"
                }
            };
        }

        // This method handles book actions (borrow, renew, return)
        protected async void BookAction_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            try
            {
                string bookId = e.CommandArgument.ToString();
                string username = User.Identity.Name;
                string bookTitle = "";
                bool gotBookDetails = false;
                
                // Get book title for history - try multiple times if needed
                using (var bookClient = CreateBookServiceClient())
                {
                    try
                    {
                        var book = await bookClient.GetBookByIdAsync(bookId);
                        if (book != null)
                        {
                            bookTitle = book.Title?.Trim();
                            gotBookDetails = !string.IsNullOrEmpty(bookTitle);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error getting book details: {ex.Message}");
                        LogError("Error getting book details", ex);
                    }
                }

                // If we couldn't get the book title, try one more time using the borrowing service
                if (!gotBookDetails)
                {
                    try
                    {
                        using (var borrowingClient = CreateBorrowingServiceClient())
                        {
                            var borrowRecord = await borrowingClient.GetBorrowRecordAsync(bookId);
                            if (borrowRecord != null && !string.IsNullOrEmpty(borrowRecord.BookTitle))
                            {
                                bookTitle = borrowRecord.BookTitle.Trim();
                                gotBookDetails = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error getting borrow record: {ex.Message}");
                        LogError("Error getting borrow record", ex);
                    }
                }
                
                using (var borrowingClient = CreateBorrowingServiceClient())
                {
                    if (e.CommandName == "Borrow")
                    {
                        // Call borrowing service to check out the book
                        var borrowResult = await borrowingClient.CheckoutBookAsync(bookId, username, username);
                        
                        // If we still don't have the title but got it from checkout result
                        if (!gotBookDetails && borrowResult != null && !string.IsNullOrEmpty(borrowResult.BookTitle))
                        {
                            bookTitle = borrowResult.BookTitle.Trim();
                        }
                        
                        // Add to history
                        AddBorrowHistory(username, bookId, bookTitle, "Borrowed");
                        
                        // Refresh borrowed books list
                        await LoadBorrowedBooksAsync();
                        
                        // Refresh history
                        LoadBorrowHistory();
                        
                        // Show success message
                        ShowAlert("Book successfully borrowed.", "success");
                    }
                    else if (e.CommandName == "Renew")
                    {
                        // Instead of just showing alert, we'll log a new borrow
                        var renewResult = await borrowingClient.CheckoutBookAsync(bookId, username, username);
                        
                        // If we still don't have the title but got it from renew result
                        if (!gotBookDetails && renewResult != null && !string.IsNullOrEmpty(renewResult.BookTitle))
                        {
                            bookTitle = renewResult.BookTitle.Trim();
                        }
                        
                        // Add to history
                        AddBorrowHistory(username, bookId, bookTitle, "Renewed");
                        
                        // Refresh borrowed books list
                        await LoadBorrowedBooksAsync();
                        
                        // Refresh history
                        LoadBorrowHistory();
                        
                        // Show success message
                        ShowAlert("Book successfully renewed.", "success");
                    }
                    else if (e.CommandName == "Return")
                    {
                        // Call borrowing service to return the book
                        var returnResult = await borrowingClient.ReturnBookAsync(bookId);
                        
                        // If we still don't have the title but got it from return result
                        if (!gotBookDetails && returnResult != null && !string.IsNullOrEmpty(returnResult.BookTitle))
                        {
                            bookTitle = returnResult.BookTitle.Trim();
                        }
                        
                        // Add to history
                        AddBorrowHistory(username, bookId, bookTitle, "Returned");
                        
                        // Refresh borrowed books list
                        await LoadBorrowedBooksAsync();
                        
                        // Refresh history
                        LoadBorrowHistory();
                        
                        // Show success message
                        ShowAlert("Book successfully returned.", "success");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                System.Diagnostics.Debug.WriteLine($"Error in BookAction_Command: {ex.Message}");
                LogError("Error performing book action", ex);
                
                // Show error message
                ShowAlert($"Error: {ex.Message}", "danger");
            }
        }
        
        private void AddBorrowHistory(string username, string bookId, string title, string actionType)
        {
            if (string.IsNullOrEmpty(title))
            {
                title = $"Book ID: {bookId}";
            }
            
            var historyEntry = new BorrowHistory
            {
                BookId = bookId,
                Title = title,
                ActionType = actionType,
                ActionDate = DateTime.Now
            };
            
            if (!_userBorrowHistory.ContainsKey(username))
            {
                _userBorrowHistory[username] = new List<BorrowHistory>();
            }
            
            _userBorrowHistory[username].Add(historyEntry);
            
            // Limit history to last 20 entries
            if (_userBorrowHistory[username].Count > 20)
            {
                _userBorrowHistory[username] = _userBorrowHistory[username]
                    .OrderByDescending(h => h.ActionDate)
                    .Take(20)
                    .ToList();
            }
        }
        
        private void LoadBorrowHistory()
        {
            string username = User.Identity.Name;
            
            if (_userBorrowHistory.ContainsKey(username))
            {
                BorrowHistoryGridView.DataSource = _userBorrowHistory[username]
                    .OrderByDescending(h => h.ActionDate)
                    .ToList();
            }
            else
            {
                BorrowHistoryGridView.DataSource = new List<BorrowHistory>();
            }
            
            BorrowHistoryGridView.DataBind();
        }
        
        // Helper method to show alerts
        private void ShowAlert(string message, string type)
        {
            AlertMessage.Text = message;
            AlertPanel.CssClass = $"alert alert-{type} alert-dismissible fade show";
            AlertPanel.Visible = true;
        }

        private void LogError(string message, Exception ex)
        {
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
        
        // Helper method to determine due status
        protected string GetDueStatus(DateTime dueDate)
        {
            var daysRemaining = (dueDate - DateTime.Now).TotalDays;
            
            if (dueDate < DateTime.Now)
            {
                return $"<span class='badge badge-danger'>Overdue</span>";
            }
            else if (daysRemaining <= 3)
            {
                return $"<span class='badge badge-warning'>Due soon</span>";
            }
            else
            {
                return $"<span class='badge badge-success'>On time</span>";
            }
        }
    }

    public class BorrowedBook
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
    }
    
    public class BorrowHistory
    {
        public string BookId { get; set; }
        public string Title { get; set; }
        public DateTime ActionDate { get; set; }
        public string ActionType { get; set; } // "Borrowed", "Renewed", "Returned"
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
}