using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.ServiceModel;

namespace LibraryManagementSystem.Services
{
    public partial class TryBorrowingService : Page
    {
        // Reference to the BorrowingService
        private BorrowingServiceReference.BorrowingServiceClient borrowingService;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set default values
                txtMemberId.Text = "1"; // Default member ID for testing
                txtHistoryMemberId.Text = "1"; // Default history member ID
            }
        }

        private BorrowingServiceReference.BorrowingServiceClient CreateBorrowingServiceClient()
        {
            var client = new BorrowingServiceReference.BorrowingServiceClient("BasicHttpBinding_IBorrowingService");
            client.Endpoint.Binding.SendTimeout = TimeSpan.FromSeconds(30);
            client.Endpoint.Binding.ReceiveTimeout = TimeSpan.FromSeconds(30);
            return client;
        }

        protected void btnBorrow_Click(object sender, EventArgs e)
        {
            try
            {
                string bookId = txtBookId.Text.Trim();
                string memberId = txtMemberId.Text.Trim();

                using (var borrowingService = CreateBorrowingServiceClient())
                {
                    // Call the CheckoutBook method from the service (previously BorrowBook)
                    var result = borrowingService.CheckoutBook(bookId, memberId, "TestUser");

                    // Display the result
                    ShowResult("Book borrowed successfully: " + result.BookTitle, true);
                }
            }
            catch (Exception ex)
            {
                ShowResult("Error borrowing book: " + GetUserFriendlyError(ex), false);
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            try
            {
                string borrowId = txtBorrowId.Text.Trim();

                using (var borrowingService = CreateBorrowingServiceClient())
                {
                    // Call the ReturnBook method from the service
                    var result = borrowingService.ReturnBook(borrowId);

                    // Display the result
                    ShowResult("Book returned successfully: " + result.BookTitle, true);
                }
            }
            catch (Exception ex)
            {
                ShowResult("Error returning book: " + GetUserFriendlyError(ex), false);
            }
        }

        protected void btnViewHistory_Click(object sender, EventArgs e)
        {
            try
            {
                string memberId = txtHistoryMemberId.Text.Trim();

                if (string.IsNullOrEmpty(memberId))
                {
                    ShowResult("Please enter a Member ID to view history.", false);
                    return;
                }

                using (var borrowingService = CreateBorrowingServiceClient())
                {
                    // Call the GetBorrowHistory method from the service (previously GetBorrowingHistory)
                    var history = borrowingService.GetBorrowHistory(memberId);

                    // Display the records in the GridView
                    DisplayBorrowingRecords(history);
                }
            }
            catch (Exception ex)
            {
                ShowResult("Error retrieving borrowing history: " + GetUserFriendlyError(ex), false);
            }
        }

        protected void btnViewAllBorrowing_Click(object sender, EventArgs e)
        {
            try
            {
                using (var borrowingService = CreateBorrowingServiceClient())
                {
                    // Call the GetCurrentBorrowsByUser method from the service (previously GetCurrentBorrowings)
                    // We'll pass an empty string to get all borrowings or implement a different approach if needed
                    var currentBorrowings = borrowingService.GetCurrentBorrowsByUser("");

                    // Display the records in the GridView
                    DisplayBorrowingRecords(currentBorrowings);
                }
            }
            catch (Exception ex)
            {
                ShowResult("Error retrieving current borrowings: " + GetUserFriendlyError(ex), false);
            }
        }

        private void DisplayBorrowingRecords(BorrowingServiceReference.BorrowRecord[] records)
        {
            if (records == null || records.Length == 0)
            {
                pnlResults.Visible = true;
                gvBorrowingHistory.Visible = false;
                litResults.Text = "No borrowing records found.";
                return;
            }

            // Create a DataTable to bind to the GridView
            DataTable dt = new DataTable();
            dt.Columns.Add("BorrowId", typeof(string));
            dt.Columns.Add("BookId", typeof(string));
            dt.Columns.Add("BookTitle", typeof(string));
            dt.Columns.Add("MemberId", typeof(string));
            dt.Columns.Add("BorrowDate", typeof(DateTime));
            dt.Columns.Add("DueDate", typeof(DateTime));
            dt.Columns.Add("ReturnDate", typeof(DateTime));
            dt.Columns.Add("Status", typeof(string));

            // Populate the DataTable
            foreach (BorrowingServiceReference.BorrowRecord record in records)
            {
                DataRow row = dt.NewRow();
                row["BorrowId"] = record.Id;
                row["BookId"] = record.BookId;
                row["BookTitle"] = record.BookTitle;
                row["MemberId"] = record.UserId;
                row["BorrowDate"] = record.BorrowDate;
                row["DueDate"] = record.DueDate;
                
                if (record.ReturnDate != null && record.ReturnDate != DateTime.MinValue)
                {
                    row["ReturnDate"] = record.ReturnDate;
                }
                else
                {
                    row["ReturnDate"] = DBNull.Value;
                }
                
                row["Status"] = record.IsReturned ? "Returned" : "Borrowed";
                dt.Rows.Add(row);
            }

            // Bind the DataTable to the GridView
            gvBorrowingHistory.DataSource = dt;
            gvBorrowingHistory.DataBind();
            gvBorrowingHistory.Visible = true;
            pnlResults.Visible = false;
        }

        private void ShowResult(string message, bool success)
        {
            pnlResults.Visible = true;
            pnlResults.CssClass = success ? "alert alert-success" : "alert alert-danger";
            litResults.Text = message;
            gvBorrowingHistory.Visible = false;
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
    }
} 