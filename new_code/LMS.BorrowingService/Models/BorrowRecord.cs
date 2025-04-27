using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace LMS.BorrowingService.Models
{
    [DataContract]
    [XmlRoot("BorrowRecord")]
    public class BorrowRecord
    {
        [DataMember]
        [XmlElement("Id")]
        public string Id { get; set; }

        [DataMember]
        [XmlElement("BookId")]
        public string BookId { get; set; }
        
        [DataMember]
        [XmlElement("BookTitle")]
        public string BookTitle { get; set; }

        [DataMember]
        [XmlElement("UserId")]
        public string UserId { get; set; }

        [DataMember]
        [XmlElement("BorrowDate")]
        public DateTime BorrowDate { get; set; }

        [DataMember]
        [XmlElement("DueDate")]
        public DateTime DueDate { get; set; }

        [DataMember]
        [XmlElement("ReturnDate")]
        public DateTime? ReturnDate { get; set; }

        [DataMember]
        [XmlElement("IsReturned")]
        public bool IsReturned { get; set; }
    }
} 