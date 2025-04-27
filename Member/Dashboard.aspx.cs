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
                                BorrowDate = b.BorrowDate,
                                DueDate = b.DueDate
                            }).ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the error but continue to show fallback data
                        System.Diagnostics.Debug.WriteLine($"Error with BorrowingService: {ex.Message}");
                        LogError("BorrowingService unavailable", ex);
                    }
                }

                // If we couldn't get data or no books are borrowed, use sample data
                if (borrowList.Count == 0)
                {
                    borrowList = GetSampleBorrowedBooks();
                }

                BorrowedBooksGridView.DataSource = borrowList;
                BorrowedBooksGridView.DataBind();
            }
            catch (Exception ex)
            {
                // Log error and use sample data as fallback
                System.Diagnostics.Debug.WriteLine($"Error loading borrowed books: {ex.Message}");
                LogError("Error loading borrowed books", ex);
                BorrowedBooksGridView.DataSource = GetSampleBorrowedBooks();
                BorrowedBooksGridView.DataBind();
            }
        }

        private BorrowingServiceReference.BorrowingServiceClient CreateBorrowingServiceClient()
        {
            var client = new BorrowingServiceReference.BorrowingServiceClient("BasicHttpBinding_IBorrowingService");
            client.Endpoint.Binding.SendTimeout = TimeSpan.FromSeconds(30);
            client.Endpoint.Binding.ReceiveTimeout = TimeSpan.FromSeconds(30);
            
            return client;
        }

        private List<BorrowedBook> GetSampleBorrowedBooks()
        {
            return new List<BorrowedBook>
            {
                new BorrowedBook
                {
                    Id = "1",
                    Title = "The Great Gatsby",
                    Author = "F. Scott Fitzgerald",
                    BorrowDate = DateTime.Now.AddDays(-14),
                    DueDate = DateTime.Now.AddDays(7)
                },
                new BorrowedBook
                {
                    Id = "2",
                    Title = "To Kill a Mockingbird",
                    Author = "Harper Lee",
                    BorrowDate = DateTime.Now.AddDays(-7),
                    DueDate = DateTime.Now.AddDays(14)
                }
            };
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
                
                using (var borrowingClient = CreateBorrowingServiceClient())
                {
                    if (e.CommandName == "Borrow")
                    {
                        // Call borrowing service to check out the book
                        await borrowingClient.CheckoutBookAsync(bookId, username, username);
                        
                        // Refresh borrowed books list
                        await LoadBorrowedBooksAsync();
                        
                        // Show success message
                        ShowAlert("Book successfully borrowed.", "success");
                    }
                    else if (e.CommandName == "Renew")
                    {
                        // For now, renewal is just handled with an alert since it's not implemented in the service
                        ShowAlert("Renewal feature is not yet implemented.", "warning");
                    }
                    else if (e.CommandName == "Return")
                    {
                        // Call borrowing service to return the book
                        await borrowingClient.ReturnBookAsync(bookId);
                        
                        // Refresh borrowed books list
                        await LoadBorrowedBooksAsync();
                        
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