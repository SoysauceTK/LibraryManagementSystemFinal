using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LMS.BookSearch;
using LMS.BookSearch.Models;
using System.Text;
using Newtonsoft.Json;

namespace LibraryManagementSystem.Services
{
    public partial class TryBookSearch : System.Web.UI.Page
    {
        private SearchService _searchService;

        protected void Page_Load(object sender, EventArgs e)
        {
            _searchService = new SearchService();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string query = txtSearchQuery.Text.Trim();
                if (string.IsNullOrEmpty(query))
                {
                    litResults.Text = "Please enter a search query";
                    return;
                }

                var books = _searchService.SearchBooks(query);
                DisplayResults(books);
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
        }

        protected void btnAdvancedSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string title = txtTitle.Text.Trim();
                string author = txtAuthor.Text.Trim();
                string category = txtCategory.Text.Trim();
                string year = txtYear.Text.Trim();

                if (string.IsNullOrEmpty(title) && string.IsNullOrEmpty(author) && 
                    string.IsNullOrEmpty(category) && string.IsNullOrEmpty(year))
                {
                    litResults.Text = "Please enter at least one search parameter";
                    return;
                }

                var books = _searchService.AdvancedSearch(title, author, category, year);
                DisplayResults(books);
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
        }

        protected void btnGetRecommendations_Click(object sender, EventArgs e)
        {
            try
            {
                string bookId = txtBookId.Text.Trim();
                if (string.IsNullOrEmpty(bookId))
                {
                    litResults.Text = "Please enter a book ID";
                    return;
                }

                var books = _searchService.GetRecommendations(bookId);
                DisplayResults(books);
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
        }

        protected void btnGetPopularBooks_Click(object sender, EventArgs e)
        {
            try
            {
                var books = _searchService.GetPopularBooks();
                DisplayResults(books);
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
        }

        protected void btnGetAllCategories_Click(object sender, EventArgs e)
        {
            try
            {
                var categories = _searchService.GetAllCategories();
                DisplayResults(categories);
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
        }

        private void DisplayResults<T>(T data)
        {
            if (data == null)
            {
                litResults.Text = "No results found";
                return;
            }

            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            litResults.Text = HttpUtility.HtmlEncode(json);
        }

        private void DisplayError(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Error:");
            sb.AppendLine(ex.Message);
            
            if (ex.InnerException != null)
            {
                sb.AppendLine("Inner Exception:");
                sb.AppendLine(ex.InnerException.Message);
            }
            
            litResults.Text = HttpUtility.HtmlEncode(sb.ToString());
        }
    }
} 