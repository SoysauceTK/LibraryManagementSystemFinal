// SearchService.svc.cs - Create in LMS.BookSearch project
// DEVELOPER: Sawyer Kesti - Book Search Service - [Date]
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace LMS.BookSearch
{
    [ServiceContract]
    public interface ISearchService
    {
        [OperationContract]
        [WebGet(UriTemplate = "search?q={query}", ResponseFormat = WebMessageFormat.Json)]
        List<Book> SearchBooks(string query);

        [OperationContract]
        [WebGet(UriTemplate = "advanced?title={title}&author={author}&category={category}&year={year}",
            ResponseFormat = WebMessageFormat.Json)]
        List<Book> AdvancedSearch(string title, string author, string category, string year);

        [OperationContract]
        [WebGet(UriTemplate = "recommendations/{bookId}", ResponseFormat = WebMessageFormat.Json)]
        List<Book> GetRecommendations(string bookId);

        [OperationContract]
        [WebGet(UriTemplate = "popular", ResponseFormat = WebMessageFormat.Json)]
        List<Book> GetPopularBooks();

        [OperationContract]
        [WebGet(UriTemplate = "categories", ResponseFormat = WebMessageFormat.Json)]
        List<string> GetAllCategories();
    }
}