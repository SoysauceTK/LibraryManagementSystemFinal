using System;
using System.Runtime.Serialization;

namespace LMS.BorrowingService.Models
{
    /// <summary>
    /// Represents a book in the library system
    /// This model is used locally in the BorrowingService
    /// </summary>
    [DataContract]
    public class Book
    {
        [DataMember]
        public string Id { get; set; }
        
        [DataMember]
        public string Title { get; set; }
        
        [DataMember]
        public string Author { get; set; }
        
        [DataMember]
        public string ISBN { get; set; }
        
        [DataMember]
        public string Category { get; set; }
        
        [DataMember]
        public int PublicationYear { get; set; }
        
        [DataMember]
        public string Publisher { get; set; }
        
        [DataMember]
        public int CopiesAvailable { get; set; }
        
        [DataMember]
        public string Description { get; set; }
        
        [DataMember]
        public string CoverImageUrl { get; set; }
    }
} 