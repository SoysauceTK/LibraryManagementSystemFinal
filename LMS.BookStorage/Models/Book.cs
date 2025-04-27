using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace LMS.BookStorage.Models
{
    [DataContract]
    [XmlRoot("Book")]
    public class Book
    {
        [DataMember]
        [XmlElement("Id")]
        public string Id { get; set; }

        [DataMember]
        [XmlElement("Title")]
        public string Title { get; set; }

        [DataMember]
        [XmlElement("Author")]
        public string Author { get; set; }

        [DataMember]
        [XmlElement("ISBN")]
        public string ISBN { get; set; }

        [DataMember]
        [XmlElement("Category")]
        public string Category { get; set; }

        [DataMember]
        [XmlElement("PublicationYear")]
        public int PublicationYear { get; set; }

        [DataMember]
        [XmlElement("Publisher")]
        public string Publisher { get; set; }

        [DataMember]
        [XmlElement("CopiesAvailable")]
        public int CopiesAvailable { get; set; }

        [DataMember]
        [XmlElement("Description")]
        public string Description { get; set; }

        [DataMember]
        [XmlElement("CoverImageUrl")]
        public string CoverImageUrl { get; set; }
    }

    [DataContract]
    [XmlRoot("InventoryUpdate")]
    public class InventoryUpdate
    {
        [DataMember]
        [XmlElement("BookId")]
        public string BookId { get; set; }

        [DataMember]
        [XmlElement("QuantityChange")]
        public int QuantityChange { get; set; }
    }
}