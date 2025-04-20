// BookService.svc.cs - Create in LMS.BookStorage project
// DEVELOPER: Aarya Baireddy - Book Storage Service - [Date]
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace LibraryManagementSystem.Service_Layer
{
    [ServiceContract]
    public interface IBookStorage
    {
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        List<Book> GetAllBooks();

        [OperationContract]
        [WebGet(UriTemplate = "book/{id}", ResponseFormat = WebMessageFormat.Json)]
        Book GetBookById(string id);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "add", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Book AddBook(Book book);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Book UpdateBook(Book book);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "delete/{id}", ResponseFormat = WebMessageFormat.Json)]
        bool DeleteBook(string id);

        [OperationContract]
        [WebGet(UriTemplate = "category/{category}", ResponseFormat = WebMessageFormat.Json)]
        List<Book> GetBooksByCategory(string category);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "inventory/update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        bool UpdateInventory(InventoryUpdate update);
    }

    // Data contract for inventory updates
    [DataContract]
    public class InventoryUpdate
    {
        [DataMember]
        public string BookId { get; set; }

        [DataMember]
        public int QuantityChange { get; set; }
    }
}
