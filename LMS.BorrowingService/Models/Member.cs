using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace LMS.BorrowingService.Models
{
    [DataContract]
    [XmlRoot("Member")]
    public class Member
    {
        [DataMember]
        [XmlElement("Id")]
        public string Id { get; set; }

        [DataMember]
        [XmlElement("Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlElement("Email")]
        public string Email { get; set; }

        [DataMember]
        [XmlElement("MembershipDate")]
        public DateTime MembershipDate { get; set; }
    }
} 