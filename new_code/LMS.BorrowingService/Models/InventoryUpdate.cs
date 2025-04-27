using System.Runtime.Serialization;

namespace LMS.BorrowingService.Models
{
    /// <summary>
    /// Model for book inventory updates
    /// Used to increment or decrement available book copies
    /// </summary>
    [DataContract]
    public class InventoryUpdate
    {
        [DataMember]
        public string BookId { get; set; }
        
        [DataMember]
        public int QuantityChange { get; set; }
    }
} 