using System;
using System.Collections.Generic;
using System.Web.UI;
// using System.Xml;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Web; // Needed for HttpContext, Request
using System.Linq; // Needed for Count()

// Assume a Book model exists here or is referenced, matching the service model
// If not, you might need to create one or continue using anonymous types carefully.
// For clarity, let's define a simple Book class for deserialization in LoadQuickStats
// Note: Ensure this matches the structure returned by BookService.GetAllBooks()
// If LMS.BookStorage.Models or LMS.BookSearch.Models can be referenced, use that.
// Otherwise, define it here:
namespace LibraryManagementSystem.Models // Or an appropriate namespace
{
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
        public string CoverImageUrl { get; set; } // Even if not used here, include for complete mapping
    }
}


namespace LibraryManagementSystem.Staff
{
    // DEVELOPER: Sawyer Kesti - Staff dashboard (Updated for Service Integration)
    public partial class Dashboard : Page
    {
        private static readonly HttpClient client = new HttpClient(); // Reuse HttpClient instance
        private string _bookServiceBaseUrl;
        private string _searchServiceBaseUrl; // Add URL for Search Service

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is authenticated and in Staff role
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Staff"))
            {
                Response.Redirect("~/Staff/Login.aspx", false); // Use false to stop execution here
                Context.ApplicationInstance.CompleteRequest(); // Ensure response is sent
                return;
            }

            DetermineServiceUrls(); // Set the base URLs

            if (!IsPostBack)
            {
                // Display username
                UserNameLiteral.Text = User.Identity.Name;

                // Load quick stats asynchronously
                RegisterAsyncTask(new PageAsyncTask(LoadQuickStats));

                // Load activity log (still uses sample data)
                LoadActivityLog();
            }
        }

        private void DetermineServiceUrls()
        {
            string host = Request.Url.Host.ToLower();
            // --- IMPORTANT: Replace X with your actual site number for Webstrar ---
            string siteNumber = "X"; // <--- CONFIGURATION NEEDED HERE

            // --- Adjust Ports if your services run on different ports locally ---
            string localBookServicePort = "44301"; // Default from original code
            string localSearchServicePort = "44302"; // *** ASSUMPTION: Search service runs on a different port locally. Adjust if necessary. ***

            if (host.Contains("localhost"))
            {
                _bookServiceBaseUrl = $"http://localhost:{localBookServicePort}/BookService.svc";
                _searchServiceBaseUrl = $"http://localhost:{localSearchServicePort}/SearchService.svc"; // Assumed port
            }
            else // Assuming Webstrar deployment
            {
                // --- ASSUMPTION: Both services are deployed under the same 'page0' path on Webstrar. Adjust if structure differs. ---
                string baseWebstrarUrl = $"http://webstrar{siteNumber}.fulton.asu.edu/page0"; // Adjust 'page0' if needed
                _bookServiceBaseUrl = $"{baseWebstrarUrl}/BookService.svc";
                _searchServiceBaseUrl = $"{baseWebstrarUrl}/SearchService.svc";
            }
        }

        private async Task LoadQuickStats()
        {
            // --- Get Total Books from BookService ---
            try
            {
                string getAllBooksUrl = $"{_bookServiceBaseUrl}/GetAllBooks";
                HttpResponseMessage response = await client.GetAsync(getAllBooksUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    // Use the defined Book class for deserialization
                    var books = JsonConvert.DeserializeObject<List<LibraryManagementSystem.Models.Book>>(jsonResponse);
                    TotalBooksLiteral.Text = books?.Count.ToString("N0") ?? "0"; // Format number with commas
                }
                else
                {
                    TotalBooksLiteral.Text = "Error";
                    // Log the error: response.StatusCode, response.ReasonPhrase
                }
            }
            catch (Exception ex)
            {
                TotalBooksLiteral.Text = "Unavailable";
                // Log the exception ex
                // Consider displaying a more user-friendly message or logging detail
                System.Diagnostics.Debug.WriteLine($"Error loading total books: {ex.Message}");
            }

            // --- Placeholder for other stats - Requires other services (Borrowing, Member) ---
            // In a real implementation, call respective services here asynchronously
            BorrowedBooksLiteral.Text = "278 (Sample)"; // Placeholder
            MembersLiteral.Text = "356 (Sample)";      // Placeholder
            OverdueLiteral.Text = "12 (Sample)";       // Placeholder
        }

        private void LoadActivityLog()
        {
            // --- This still uses sample data ---
            // In a real implementation, this would load from a dedicated logging service or database.
            // The LogBookAddition method currently updates a local list for demo purposes.
            // For now, check if the data source is already populated (e.g., by LogBookAddition)
            if (ActivityLogGridView.DataSource == null)
            {
                List<ActivityLogEntry> activityLog = new List<ActivityLogEntry>
                 {
                     // Sample entries... (can be kept or removed if LogBookAddition is the primary source)
                     new ActivityLogEntry { Date = DateTime.Now.AddHours(-3), Action = "Book Borrowed", Details = "To Kill a Mockingbird by Harper Lee", User = "john_doe" },
                     new ActivityLogEntry { Date = DateTime.Now.AddHours(-5), Action = "New Member", Details = "sarah_smith joined the library", User = "system" },
                     new ActivityLogEntry { Date = DateTime.Now.AddDays(-1), Action = "Book Returned", Details = "1984 by George Orwell", User = "mike_jones" }
                 };
                ActivityLogGridView.DataSource = activityLog;
            }
            // Ensure DataBind happens even if populated by LogBookAddition before initial load
            ActivityLogGridView.DataBind();
        }

        protected async void AddBookButton_Click(object sender, EventArgs e)
        {
            // Ensure service URLs are set (might be redundant if always called after Page_Load, but safe)
            DetermineServiceUrls();

            if (Page.IsValid)
            {
                try
                {
                    // Create a book object matching the BookService model definition
                    // Using an anonymous type here as before, ensure properties match LMS.BookStorage.Models.Book
                    var book = new
                    {
                        // Let the service generate the ID if AddBook handles null/empty IDs
                        // Id = Guid.NewGuid().ToString(), // Service should handle ID generation
                        Title = BookTitle.Text,
                        Author = BookAuthor.Text,
                        ISBN = BookISBN.Text,
                        Category = BookCategory.SelectedValue,
                        Description = BookDescription.Text,
                        CopiesAvailable = int.Parse(BookCopies.Text),
                        PublicationYear = int.Parse(BookPublicationYear.Text),
                        Publisher = "Unknown", // Provide default or add a field if needed
                        CoverImageUrl = (string)null // Add if you have this field
                    };

                    // Call the BookStorage service to add the book
                    string addBookUrl = $"{_bookServiceBaseUrl}/add"; // Use the correct base URL
                    string bookJson = JsonConvert.SerializeObject(book);
                    HttpContent content = new StringContent(bookJson, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(addBookUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        BookAddStatus.Text = "<div class='alert alert-success'>Book added successfully!</div>";

                        // Log the activity (currently updates local list)
                        LogBookAddition(book.Title, book.Author);

                        // Clear the form
                        ClearBookForm();

                        // Refresh quick stats (specifically total books count)
                        await LoadQuickStats(); // Re-load stats after adding
                    }
                    else
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        BookAddStatus.Text = $"<div class='alert alert-danger'>Error adding book: {response.StatusCode} - {response.ReasonPhrase}. Details: {errorContent}</div>";
                        // Log the error details: errorContent
                    }
                }
                catch (HttpRequestException httpEx)
                {
                    // Handle cases where the service is unreachable
                    BookAddStatus.Text = $"<div class='alert alert-warning'>Could not connect to Book Service. Please try again later. ({httpEx.Message})</div>";
                    // Log the exception httpEx
                }
                catch (JsonException jsonEx)
                {
                    BookAddStatus.Text = $"<div class='alert alert-danger'>Error processing service response: {jsonEx.Message}</div>";
                    // Log the exception jsonEx
                }
                catch (Exception ex) // Catch other potential errors (e.g., parsing)
                {
                    BookAddStatus.Text = $"<div class='alert alert-danger'>An unexpected error occurred: {ex.Message}</div>";
                    // Log the exception ex
                }
            }
        }

        private void ClearBookForm()
        {
            BookTitle.Text = string.Empty;
            BookAuthor.Text = string.Empty;
            BookISBN.Text = string.Empty;
            BookCategory.SelectedIndex = 0; // Reset to the first item
            BookPublicationYear.Text = string.Empty;
            BookCopies.Text = string.Empty;
            BookDescription.Text = string.Empty;
        }

        private void LogBookAddition(string title, string author)
        {
            // --- Placeholder: Updates local sample data ---
            // Ideally, this would call a separate Logging Service.
            // For now, it adds to the GridView's current data source if it's the expected list type.
            var activityLog = ActivityLogGridView.DataSource as List<ActivityLogEntry>;
            if (activityLog == null)
            {
                // If the grid hasn't been bound yet or has a different source, create a new list.
                // This might happen if LoadActivityLog hasn't run or failed.
                activityLog = new List<ActivityLogEntry>();
            }

            activityLog.Insert(0, new ActivityLogEntry
            {
                Date = DateTime.Now,
                Action = "Book Added",
                Details = $"{title} by {author}",
                User = User.Identity.Name ?? "Unknown" // Use logged-in user
            });

            // Re-bind the grid view with the updated list
            ActivityLogGridView.DataSource = activityLog;
            ActivityLogGridView.DataBind();
        }

        // --- NEW: Search Functionality ---

        /// <summary>
        /// Handles the click event for the Search button.
        /// </summary>
        protected async void SearchButton_Click(object sender, EventArgs e)
        {
            string query = SearchTextBox.Text.Trim();
            SearchStatusPanel.Visible = true; // Show status panel

            if (string.IsNullOrWhiteSpace(query))
            {
                SearchStatusLiteral.Text = "<div class='alert alert-warning'>Please enter a search term.</div>";
                SearchResultsGridView.DataSource = null; // Clear results
                SearchResultsGridView.DataBind();
                return;
            }

            SearchStatusLiteral.Text = "<div class='alert alert-info'>Searching...</div>"; // Provide feedback
            await PerformSearchAsync(query);
        }

        /// <summary>
        /// Calls the Search Service and updates the UI with results or errors.
        /// </summary>
        /// <param name="query">The search term.</param>
        private async Task PerformSearchAsync(string query)
        {
            DetermineServiceUrls(); // Ensure URLs are current

            try
            {
                string encodedQuery = HttpUtility.UrlEncode(query); // IMPORTANT: Encode the query
                string searchUrl = $"{_searchServiceBaseUrl}/search?q={encodedQuery}";

                HttpResponseMessage response = await client.GetAsync(searchUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var results = JsonConvert.DeserializeObject<List<LibraryManagementSystem.Models.Book>>(jsonResponse);

                    SearchResultsGridView.DataSource = results;
                    SearchResultsGridView.DataBind();

                    if (results != null && results.Any())
                    {
                        SearchStatusLiteral.Text = $"<div class='alert alert-success'>Found {results.Count} book(s) matching '{HttpUtility.HtmlEncode(query)}'.</div>"; // Encode output query
                    }
                    else
                    {
                        // GridView's EmptyDataText will be shown
                        SearchStatusLiteral.Text = $"<div class='alert alert-warning'>No books found matching '{HttpUtility.HtmlEncode(query)}'.</div>"; // Encode output query
                    }
                }
                else
                {
                    // Service returned an error status code
                    string errorContent = await response.Content.ReadAsStringAsync();
                    SearchStatusLiteral.Text = $"<div class='alert alert-danger'>Error during search: {response.StatusCode} - {response.ReasonPhrase}. Details: {HttpUtility.HtmlEncode(errorContent)}</div>";
                    SearchResultsGridView.DataSource = null;
                    SearchResultsGridView.DataBind();
                    // Log the error: errorContent
                }
            }
            catch (HttpRequestException httpEx)
            {
                SearchStatusLiteral.Text = $"<div class='alert alert-danger'>Could not connect to Search Service. Please try again later. ({HttpUtility.HtmlEncode(httpEx.Message)})</div>";
                SearchResultsGridView.DataSource = null;
                SearchResultsGridView.DataBind();
                // Log the exception httpEx
            }
            catch (JsonException jsonEx)
            {
                SearchStatusLiteral.Text = $"<div class='alert alert-danger'>Error processing search results: {HttpUtility.HtmlEncode(jsonEx.Message)}</div>";
                SearchResultsGridView.DataSource = null;
                SearchResultsGridView.DataBind();
                // Log the exception jsonEx
            }
            catch (Exception ex)
            {
                SearchStatusLiteral.Text = $"<div class='alert alert-danger'>An unexpected error occurred during search: {HttpUtility.HtmlEncode(ex.Message)}</div>";
                SearchResultsGridView.DataSource = null;
                SearchResultsGridView.DataBind();
                // Log the exception ex
            }
        }

    }

    // Keep the ActivityLogEntry class definition
    public class ActivityLogEntry
    {
        public DateTime Date { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }
        public string User { get; set; }
    }
}