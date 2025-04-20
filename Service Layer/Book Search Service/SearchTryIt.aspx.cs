using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using LibraryManagementSystem.Service_Layer.Book_Search_Service;

namespace LibraryManagementSystem.Service_Layer.Book_Search_Service
{
    public partial class SearchTryIt : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Disable any authentication redirects for this page
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            if (!IsPostBack)
            {
                string op = Request.QueryString["op"];
                if (!string.IsNullOrEmpty(op))
                {
                    // Set the appropriate controls based on the operation
                    switch (op)
                    {
                        case "SearchBooks":
                            txtSearchQuery.Focus();
                            break;
                        case "AdvancedSearch":
                            // Focus on the first advanced search field
                            txtTitle.Focus();
                            break;
                        case "GetRecommendations":
                            // You might want to add a control for bookId
                            break;
                    }
                }
            }
        }

        protected void btnBasicSearch_Click(object sender, EventArgs e)
        {
            var searchService = new SearchService();
            var results = searchService.SearchBooks(txtSearchQuery.Text);
            DisplayResults(results);
        }

        protected void btnAdvancedSearch_Click(object sender, EventArgs e)
        {
            var searchService = new SearchService();
            var results = searchService.AdvancedSearch(
                txtTitle.Text,
                txtAuthor.Text,
                txtCategory.Text,
                txtYear.Text);
            DisplayResults(results);
        }

        protected void btnGetPopular_Click(object sender, EventArgs e)
        {
            var searchService = new SearchService();
            var results = searchService.GetPopularBooks();
            DisplayResults(results);
        }

        protected void btnGetCategories_Click(object sender, EventArgs e)
        {
            var searchService = new SearchService();
            var categories = searchService.GetAllCategories();
            litResults.Text = "<h5>Categories:</h5><ul>" +
                string.Join("", categories.ConvertAll(c => $"<li>{c}</li>")) + "</ul>";
        }

        private void DisplayResults(List<Book> books)
        {
            if (books == null || books.Count == 0)
            {
                litResults.Text = "<p>No results found.</p>";
                return;
            }

            litResults.Text = "<table class='table'><tr><th>Title</th><th>Author</th><th>Category</th><th>Year</th></tr>" +
                string.Join("", books.ConvertAll(b =>
                    $"<tr><td>{b.Title}</td><td>{b.Author}</td><td>{b.Category}</td><td>{b.PublicationYear}</td></tr>")) +
                "</table>";
        }
    }
}