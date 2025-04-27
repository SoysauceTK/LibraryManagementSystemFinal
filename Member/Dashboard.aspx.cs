using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Xml;
using LMS.BookSearch.Models;

namespace LibraryManagementSystem.Member
{
    // DEVELOPER: Aarya Baireddy - Member dashboard
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

                // Load borrowed books
                LoadBorrowedBooks();

                // Load recommended books
                LoadRecommendedBooks();
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
            }
        }

        private void LoadBorrowedBooks()
        {
            // This would normally query the database or service for borrowed books
            // For now, we'll use sample data
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

        private void LoadRecommendedBooks()
        {
            // This would normally call the SearchService to get recommendations
            // For now, we'll use sample data
            List<Book> recommendedBooks = GetSampleRecommendedBooks();
            RecommendedBooksRepeater.DataSource = recommendedBooks;
            RecommendedBooksRepeater.DataBind();
        }

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
    }

    public class BorrowedBook
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
    }
}