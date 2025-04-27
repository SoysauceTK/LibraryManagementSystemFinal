using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using LMS.Security;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Services
{
    public partial class TryIt : Page
    {
        private string _service;
        private string _operation;
        private string _serviceUrl;

        protected void Page_Load(object sender, EventArgs e)
        {
            _service = Request.QueryString["service"];
            _operation = Request.QueryString["op"];

            if (string.IsNullOrEmpty(_service) || string.IsNullOrEmpty(_operation))
            {
                Response.Redirect("~/Default.aspx");
                return;
            }

            ServiceNameLiteral.Text = _service;
            OperationNameLiteral.Text = _operation;

            // Set service URL based on configuration (local vs. Webstrar)
            SetServiceUrl();
            ServiceUrlLiteral.Text = _serviceUrl;

            if (!IsPostBack)
            {
                // Generate input parameters based on operation
                GenerateParameterControls();
            }
        }

        private void SetServiceUrl()
        {
            // Get host from current request to determine if we're local or on Webstrar
            string host = Request.Url.Host.ToLower();
            string siteNumber = "X"; // Replace X with your actual site number in production

            if (host.Contains("localhost"))
            {
                // Local development
                if (_service == "BookService")
                {
                    _serviceUrl = "http://localhost:44301/BookService.svc";
                }
                else if (_service == "SearchService")
                {
                    _serviceUrl = "http://localhost:44302/SearchService.svc";
                }
                else if (_service == "SecurityService")
                {
                    _serviceUrl = ""; // SecurityHelper is a DLL, not a service
                }
            }
            else
            {
                // Webstrar deployment
                if (_service == "BookService")
                {
                    _serviceUrl = $"http://webstrar{siteNumber}.fulton.asu.edu/page0/BookService.svc";
                }
                else if (_service == "SearchService")
                {
                    _serviceUrl = $"http://webstrar{siteNumber}.fulton.asu.edu/page1/SearchService.svc";
                }
                else if (_service == "SecurityService")
                {
                    _serviceUrl = ""; // SecurityHelper is a DLL, not a service
                }
            }
        }

        private void GenerateParameterControls()
        {
            ParametersPlaceholder.Controls.Clear();

            if (_service == "BookService")
            {
                switch (_operation)
                {
                    case "GetAllBooks":
                        // No parameters needed
                        ParametersPlaceholder.Controls.Add(new LiteralControl("<p>This operation has no parameters.</p>"));
                        break;
                    case "GetBookById":
                        AddTextBoxParameter("id", "Book ID");
                        break;
                    case "AddBook":
                        AddTextBoxParameter("title", "Title");
                        AddTextBoxParameter("author", "Author");
                        AddTextBoxParameter("isbn", "ISBN");
                        AddDropDownParameter("category", "Category", new[] { "Fiction", "Non-Fiction", "Science Fiction", "Mystery", "Biography", "History" });
                        AddTextBoxParameter("publicationYear", "Publication Year");
                        AddTextBoxParameter("publisher", "Publisher");
                        AddTextBoxParameter("copiesAvailable", "Copies Available");
                        AddTextBoxParameter("description", "Description", true);
                        break;
                        // Add other BookService operations here
                }
            }
            else if (_service == "SearchService")
            {
                switch (_operation)
                {
                    case "SearchBooks":
                        AddTextBoxParameter("query", "Search Query");
                        break;
                    case "AdvancedSearch":
                        AddTextBoxParameter("title", "Title");
                        AddTextBoxParameter("author", "Author");
                        AddTextBoxParameter("category", "Category");
                        AddTextBoxParameter("year", "Publication Year");
                        break;
                    case "GetRecommendations":
                        AddTextBoxParameter("bookId", "Book ID");
                        break;
                        // Add other SearchService operations here
                }
            }
            else if (_service == "SecurityService")
            {
                switch (_operation)
                {
                    case "HashPassword":
                        AddTextBoxParameter("password", "Password");
                        break;
                        // Add other SecurityService operations here
                }
            }
        }

        private void AddTextBoxParameter(string id, string label, bool multiline = false)
        {
            Panel panel = new Panel { CssClass = "form-group" };
            Label lbl = new Label
            {
                AssociatedControlID = id,
                Text = label,
                CssClass = "control-label"
            };

            TextBox txt = new TextBox
            {
                ID = id,
                CssClass = "form-control"
            };

            if (multiline)
            {
                txt.TextMode = TextBoxMode.MultiLine;
                txt.Rows = 3;
            }

            panel.Controls.Add(lbl);
            panel.Controls.Add(txt);

            ParametersPlaceholder.Controls.Add(panel);
        }

        private void AddDropDownParameter(string id, string label, string[] options)
        {
            Panel panel = new Panel { CssClass = "form-group" };
            Label lbl = new Label
            {
                AssociatedControlID = id,
                Text = label,
                CssClass = "control-label"
            };

            DropDownList ddl = new DropDownList
            {
                ID = id,
                CssClass = "form-control"
            };

            foreach (string option in options)
            {
                ddl.Items.Add(new ListItem(option, option));
            }

            panel.Controls.Add(lbl);
            panel.Controls.Add(ddl);

            ParametersPlaceholder.Controls.Add(panel);
        }

        protected async void ExecuteButton_Click(object sender, EventArgs e)
        {
            try
            {
                object result = null;

                // Execute the appropriate service method
                if (_service == "BookService")
                {
                    using (HttpClient client = new HttpClient())
                    {
                        switch (_operation)
                        {
                            case "GetAllBooks":
                                string allBooksUrl = $"{_serviceUrl}/GetAllBooks";
                                HttpResponseMessage allBooksResponse = await client.GetAsync(allBooksUrl);
                                allBooksResponse.EnsureSuccessStatusCode();
                                string allBooksJson = await allBooksResponse.Content.ReadAsStringAsync();
                                // Use List<dynamic> to avoid ambiguous reference
                                result = JsonConvert.DeserializeObject<List<dynamic>>(allBooksJson);
                                break;

                            case "GetBookById":
                                string id = GetParameterValue("id");
                                string bookUrl = $"{_serviceUrl}/book/{id}";
                                HttpResponseMessage bookResponse = await client.GetAsync(bookUrl);
                                if (bookResponse.IsSuccessStatusCode)
                                {
                                    string bookJson = await bookResponse.Content.ReadAsStringAsync();
                                    // Use dynamic to avoid ambiguous reference
                                    result = JsonConvert.DeserializeObject<dynamic>(bookJson);
                                }
                                else
                                {
                                    result = new { Error = $"Book with ID {id} not found" };
                                }
                                break;

                            case "AddBook":
                                // Create a dynamic object instead of using Book class
                                var newBook = new
                                {
                                    Title = GetParameterValue("title"),
                                    Author = GetParameterValue("author"),
                                    ISBN = GetParameterValue("isbn"),
                                    Category = GetDropDownValue("category"),
                                    PublicationYear = int.Parse(GetParameterValue("publicationYear")),
                                    Publisher = GetParameterValue("publisher"),
                                    CopiesAvailable = int.Parse(GetParameterValue("copiesAvailable")),
                                    Description = GetParameterValue("description")
                                };

                                string addBookUrl = $"{_serviceUrl}/add";
                                string newBookJson = JsonConvert.SerializeObject(newBook);
                                HttpContent content = new StringContent(newBookJson, Encoding.UTF8, "application/json");
                                HttpResponseMessage addResponse = await client.PostAsync(addBookUrl, content);
                                addResponse.EnsureSuccessStatusCode();
                                string responseJson = await addResponse.Content.ReadAsStringAsync();
                                // Use dynamic to avoid ambiguous reference
                                result = JsonConvert.DeserializeObject<dynamic>(responseJson);
                                break;
                        }
                    }
                }
                else if (_service == "SearchService")
                {
                    using (HttpClient client = new HttpClient())
                    {
                        switch (_operation)
                        {
                            case "SearchBooks":
                                string query = GetParameterValue("query");
                                string searchUrl = $"{_serviceUrl}/search?q={Uri.EscapeDataString(query)}";
                                HttpResponseMessage searchResponse = await client.GetAsync(searchUrl);
                                searchResponse.EnsureSuccessStatusCode();
                                string searchJson = await searchResponse.Content.ReadAsStringAsync();
                                // Use List<dynamic> to avoid ambiguous reference
                                result = JsonConvert.DeserializeObject<List<dynamic>>(searchJson);
                                break;

                            case "AdvancedSearch":
                                string title = GetParameterValue("title");
                                string author = GetParameterValue("author");
                                string category = GetParameterValue("category");
                                string yearStr = GetParameterValue("year");

                                string advancedUrl = $"{_serviceUrl}/advanced?title={Uri.EscapeDataString(title)}&author={Uri.EscapeDataString(author)}&category={Uri.EscapeDataString(category)}&year={Uri.EscapeDataString(yearStr)}";
                                HttpResponseMessage advResponse = await client.GetAsync(advancedUrl);
                                advResponse.EnsureSuccessStatusCode();
                                string advJson = await advResponse.Content.ReadAsStringAsync();
                                // Use List<dynamic> to avoid ambiguous reference
                                result = JsonConvert.DeserializeObject<List<dynamic>>(advJson);
                                break;

                            case "GetRecommendations":
                                string bookId = GetParameterValue("bookId");
                                string recUrl = $"{_serviceUrl}/recommendations/{bookId}";
                                HttpResponseMessage recResponse = await client.GetAsync(recUrl);
                                recResponse.EnsureSuccessStatusCode();
                                string recJson = await recResponse.Content.ReadAsStringAsync();
                                // Use List<dynamic> to avoid ambiguous reference
                                result = JsonConvert.DeserializeObject<List<dynamic>>(recJson);
                                break;
                        }
                    }
                }
                else if (_service == "SecurityService")
                {
                    // SecurityHelper is a direct DLL call, not a service
                    switch (_operation)
                    {
                        case "HashPassword":
                            string password = GetParameterValue("password");
                            string hashedPassword = SecurityHelper.HashPassword(password);
                            result = new
                            {
                                OriginalPassword = password,
                                HashedPassword = hashedPassword
                            };
                            break;
                    }
                }

                // Display the result as JSON
                resultJson.InnerText = JsonConvert.SerializeObject(result, Formatting.Indented);
            }
            catch (Exception ex)
            {
                resultJson.InnerText = $"Error: {ex.Message}";
                if (ex.InnerException != null)
                {
                    resultJson.InnerText += $"\nInner Exception: {ex.InnerException.Message}";
                }
            }
        }

        private string GetParameterValue(string id)
        {
            Control control = ParametersPlaceholder.FindControl(id);
            if (control is TextBox)
            {
                return ((TextBox)control).Text;
            }
            return string.Empty;
        }

        private string GetDropDownValue(string id)
        {
            Control control = ParametersPlaceholder.FindControl(id);
            if (control is DropDownList)
            {
                return ((DropDownList)control).SelectedValue;
            }
            return string.Empty;
        }

        // Helper class to represent a book locally in this class only
        [Serializable]
        private class BookViewModel
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
}