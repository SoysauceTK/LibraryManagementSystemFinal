using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace LMS.BorrowingService.Models
{
    [DataContract]
    [XmlRoot("BorrowLog")]
    public class BorrowLog
    {
        [DataMember]
        [XmlElement("Id")]
        public string Id { get; set; }

        [DataMember]
        [XmlElement("UserId")]
        public string UserId { get; set; }

        [DataMember]
        [XmlElement("UserName")]
        public string UserName { get; set; }

        [DataMember]
        [XmlElement("BookId")]
        public string BookId { get; set; }

        [DataMember]
        [XmlElement("BookTitle")]
        public string BookTitle { get; set; }

        [DataMember]
        [XmlElement("ActionType")]
        public string ActionType { get; set; }  // "Checkout" or "Return"

        [DataMember]
        [XmlElement("ActionDate")]
        public DateTime ActionDate { get; set; }
    }
} 