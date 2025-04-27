using LMS.BorrowingService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;

namespace LibraryManagementSystem.Services
{
    public partial class TryBorrowingService : Page
    {
        // Reference to the BorrowingService
        private BorrowingServiceClient borrowingService;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Initialize the borrowing service client
            borrowingService = new BorrowingServiceClient();

            if (!IsPostBack)
            {
                // Set default values
                txtMemberId.Text = "1"; // Default member ID for testing
                txtHistoryMemberId.Text = "1"; // Default history member ID
            }
        }

        protected void btnBorrow_Click(object sender, EventArgs e)
        {
            try
            {
                string bookId = txtBookId.Text.Trim();
                string memberId = txtMemberId.Text.Trim();

                // Call the BorrowBook method from the service
                string result = borrowingService.BorrowBook(bookId, memberId);

                // Display the result
                ShowResult(result, true);
            }
            catch (Exception ex)
            {
                ShowResult("Error borrowing book: " + ex.Message, false);
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            try
            {
                string borrowId = txtBorrowId.Text.Trim();

                // Call the ReturnBook method from the service
                string result = borrowingService.ReturnBook(borrowId);

                // Display the result
                ShowResult(result, true);
            }
            catch (Exception ex)
            {
                ShowResult("Error returning book: " + ex.Message, false);
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

                // Call the GetBorrowingHistory method from the service
                BorrowRecord[] history = borrowingService.GetBorrowingHistory(memberId);

                // Display the records in the GridView
                DisplayBorrowingRecords(history);
            }
            catch (Exception ex)
            {
                ShowResult("Error retrieving borrowing history: " + ex.Message, false);
            }
        }

        protected void btnViewAllBorrowing_Click(object sender, EventArgs e)
        {
            try
            {
                // Call the GetCurrentBorrowings method from the service
                BorrowRecord[] currentBorrowings = borrowingService.GetCurrentBorrowings();

                // Display the records in the GridView
                DisplayBorrowingRecords(currentBorrowings);
            }
            catch (Exception ex)
            {
                ShowResult("Error retrieving current borrowings: " + ex.Message, false);
            }
        }

        private void DisplayBorrowingRecords(BorrowRecord[] records)
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
            foreach (BorrowRecord record in records)
            {
                DataRow row = dt.NewRow();
                row["BorrowId"] = record.BorrowId;
                row["BookId"] = record.BookId;
                row["BookTitle"] = record.BookTitle;
                row["MemberId"] = record.MemberId;
                row["BorrowDate"] = record.BorrowDate;
                row["DueDate"] = record.DueDate;
                
                if (record.ReturnDate != DateTime.MinValue)
                {
                    row["ReturnDate"] = record.ReturnDate;
                }
                else
                {
                    row["ReturnDate"] = DBNull.Value;
                }
                
                row["Status"] = record.Status;
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

        protected override void OnUnload(EventArgs e)
        {
            // Close the service client to release resources
            if (borrowingService != null)
            {
                try
                {
                    borrowingService.Close();
                }
                catch
                {
                    borrowingService.Abort();
                }
            }

            base.OnUnload(e);
        }
    }
} 