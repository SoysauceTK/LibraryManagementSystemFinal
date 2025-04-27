using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Xml;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Linq;

namespace LibraryManagementSystem.Member
{
    public partial class Dashboard : Page
    {
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

                // Load borrowed books (still sample data for now)
                LoadBorrowedBooks();

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
            }
        }

        private void LoadBorrowedBooks()
        {
            // This would normally query a BorrowingService for borrowed books
            // For now, we'll use sample data until that service is created
            List<BorrowedBook> borrowedBooks = GetSampleBorrowedBooks();
            BorrowedBooksGridView.DataSource = borrowedBooks;
            BorrowedBooksGridView.DataBind();
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

        private async Task LoadRecommendedBooksAsync()
        {
            try
            {
                // Create SearchService client instead of BookService client
                using (var searchClient = CreateSearchServiceClient())
                {
                    // Get popular books from the search service
                    var popularBooks = await searchClient.GetPopularBooksAsync();

                    if (popularBooks != null && popularBooks.Length > 0)
                    {
                        RecommendedBooksRepeater.DataSource = popularBooks;
                        RecommendedBooksRepeater.DataBind();
                    }
                    else
                    {
                        // No books found, show sample recommendations as fallback
                        var sampleBooks = GetSampleRecommendedBooks();
                        RecommendedBooksRepeater.DataSource = sampleBooks;
                        RecommendedBooksRepeater.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error
                System.Diagnostics.Debug.WriteLine($"Error loading recommended books: {ex.Message}");

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

        // This method would handle the borrowing action
        protected void BookAction_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            if (e.CommandName == "Borrow")
            {
                string bookId = e.CommandArgument.ToString();
                // In a real application, this would call a BorrowingService to borrow the book
                // For now, we'll just show an alert
                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                    $"alert('You have requested to borrow book ID: {bookId}. This feature is not yet implemented.');", true);
            }
            else if (e.CommandName == "Renew" || e.CommandName == "Return")
            {
                string bookId = e.CommandArgument.ToString();
                // In a real application, this would call a BorrowingService to renew/return the book
                // For now, we'll just show an alert
                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                    $"alert('You have requested to {e.CommandName.ToLower()} book ID: {bookId}. This feature is not yet implemented.');", true);
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