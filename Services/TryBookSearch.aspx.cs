using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Newtonsoft.Json;
using System.ServiceModel;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Services
{
    public partial class TryBookSearch : System.Web.UI.Page
    {
        private SearchServiceReference.SearchServiceClient CreateSearchServiceClient()
        {
            var client = new SearchServiceReference.SearchServiceClient("BasicHttpBinding_ISearchService");
            client.Endpoint.Binding.SendTimeout = TimeSpan.FromSeconds(30);
            client.Endpoint.Binding.ReceiveTimeout = TimeSpan.FromSeconds(30);
            return client;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(SearchBooksAsync));
        }

        private async Task SearchBooksAsync()
        {
            try
            {
                string query = txtSearchQuery.Text.Trim();
                if (string.IsNullOrEmpty(query))
                {
                    litResults.Text = "Please enter a search query";
                    return;
                }

                using (var searchClient = CreateSearchServiceClient())
                {
                    var books = await searchClient.SearchBooksAsync(query);
                    DisplayResults(books);
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
        }

        protected void btnAdvancedSearch_Click(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(AdvancedSearchAsync));
        }

        private async Task AdvancedSearchAsync()
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

                using (var searchClient = CreateSearchServiceClient())
                {
                    var books = await searchClient.AdvancedSearchAsync(title, author, category, year);
                    DisplayResults(books);
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
        }

        protected void btnGetRecommendations_Click(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(GetRecommendationsAsync));
        }

        private async Task GetRecommendationsAsync()
        {
            try
            {
                string bookId = txtBookId.Text.Trim();
                if (string.IsNullOrEmpty(bookId))
                {
                    litResults.Text = "Please enter a book ID";
                    return;
                }

                using (var searchClient = CreateSearchServiceClient())
                {
                    var books = await searchClient.GetRecommendationsAsync(bookId);
                    DisplayResults(books);
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
        }

        protected void btnGetPopularBooks_Click(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(GetPopularBooksAsync));
        }

        private async Task GetPopularBooksAsync()
        {
            try
            {
                using (var searchClient = CreateSearchServiceClient())
                {
                    var books = await searchClient.GetPopularBooksAsync();
                    DisplayResults(books);
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
        }

        protected void btnGetAllCategories_Click(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(GetAllCategoriesAsync));
        }

        private async Task GetAllCategoriesAsync()
        {
            try
            {
                using (var searchClient = CreateSearchServiceClient())
                {
                    var categories = await searchClient.GetAllCategoriesAsync();
                    DisplayResults(categories);
                }
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