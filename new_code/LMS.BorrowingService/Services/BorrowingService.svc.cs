using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.IO;
using LMS.BorrowingService.Models;
using LMS.BorrowingService.Utils;
using LMS.BorrowingService.Services;

namespace LMS.BorrowingService
{
    [ServiceContract]
    public interface IBorrowingService
    {
        [OperationContract]
        BorrowRecord CheckoutBook(string bookId, string userId, string userName);

        [OperationContract]
        BorrowRecord ReturnBook(string borrowId);

        [OperationContract]
        List<BorrowRecord> GetCurrentBorrowsByUser(string userId);

        [OperationContract]
        List<BorrowRecord> GetBorrowHistory(string userId);

        [OperationContract]
        List<BorrowLog> GetAllLogs();

        [OperationContract]
        Member GetMemberById(string userId);

        [OperationContract]
        Member CreateMember(Member member);
    }

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class BorrowingService : IBorrowingService
    {
        // Constants for paths
        private const string LOGS_FILE_NAME = "logs.xml";
        private const string CURRENT_BORROWS_FILE_NAME = "current_borrows.xml";
        private const string USER_RECORDS_DIR = "user_records";
        private static readonly string LOCAL_DATA_PATH;
        private static readonly string USER_RECORDS_PATH;

        // Remote service references
        private readonly string _bookServiceEndpoint = "http://localhost:YOUR_PORT/LMS.BookStorage/Services/BookService.svc";
        
        // Data access objects
        private XmlDataAccess<BorrowLog> _logsData;
        private XmlDataAccess<BorrowRecord> _currentBorrowsData;
        private XmlDataAccess<Member> _membersData;

        // Static constructor to initialize paths
        static BorrowingService()
        {
            // Set up a local directory for this service's data
            LOCAL_DATA_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");
            USER_RECORDS_PATH = Path.Combine(LOCAL_DATA_PATH, USER_RECORDS_DIR);
            
            // Ensure directories exist
            if (!Directory.Exists(LOCAL_DATA_PATH))
            {
                Directory.CreateDirectory(LOCAL_DATA_PATH);
            }
            
            if (!Directory.Exists(USER_RECORDS_PATH))
            {
                Directory.CreateDirectory(USER_RECORDS_PATH);
            }
        }

        public BorrowingService()
        {
            // Initialize data access objects
            _logsData = new XmlDataAccess<BorrowLog>(Path.Combine(LOCAL_DATA_PATH, LOGS_FILE_NAME));
            _currentBorrowsData = new XmlDataAccess<BorrowRecord>(Path.Combine(LOCAL_DATA_PATH, CURRENT_BORROWS_FILE_NAME));
            _membersData = new XmlDataAccess<Member>(Path.Combine(LOCAL_DATA_PATH, "members.xml"));
        }

        public BorrowRecord CheckoutBook(string bookId, string userId, string userName)
        {
            try
            {
                // Get book data from BookService using the new client
                var bookServiceClient = new BookServiceClient(_bookServiceEndpoint);
                var book = bookServiceClient.GetBookById(bookId);
                
                if (book == null)
                {
                    throw new InvalidOperationException("Book not found.");
                }
                
                if (book.CopiesAvailable <= 0)
                {
                    throw new InvalidOperationException("No copies of this book are available.");
                }
                
                // Create member record if it doesn't exist
                var member = GetMemberById(userId);
                if (member == null)
                {
                    member = new Member
                    {
                        Id = userId,
                        Name = userName,
                        MembershipDate = DateTime.Now
                    };
                    CreateMember(member);
                }
                
                // Create borrow record
                var borrowRecord = new BorrowRecord
                {
                    Id = Guid.NewGuid().ToString(),
                    BookId = bookId,
                    BookTitle = book.Title,
                    UserId = userId,
                    BorrowDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(14), // 2 weeks checkout period
                    IsReturned = false
                };
                
                // Update book inventory (decrement available copies)
                bookServiceClient.UpdateInventory(new InventoryUpdate { BookId = bookId, QuantityChange = -1 });
                
                // Add to current borrows
                _currentBorrowsData.Insert(borrowRecord);
                
                // Add to user's borrow history
                var userRecordPath = Path.Combine(USER_RECORDS_PATH, $"{userId}.xml");
                var userBorrowsData = new XmlDataAccess<BorrowRecord>(userRecordPath);
                userBorrowsData.Insert(borrowRecord);
                
                // Log the checkout action
                var log = new BorrowLog
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    UserName = userName,
                    BookId = bookId,
                    BookTitle = book.Title,
                    ActionType = "Checkout",
                    ActionDate = DateTime.Now
                };
                _logsData.Insert(log);
                
                return borrowRecord;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error during checkout: {ex.Message}");
                throw;
            }
        }

        public BorrowRecord ReturnBook(string borrowId)
        {
            try
            {
                // Find the borrow record
                var borrowRecord = _currentBorrowsData.GetById(b => b.Id == borrowId);
                if (borrowRecord == null)
                {
                    throw new InvalidOperationException("Borrow record not found.");
                }
                
                if (borrowRecord.IsReturned)
                {
                    throw new InvalidOperationException("Book has already been returned.");
                }
                
                // Update return information
                borrowRecord.ReturnDate = DateTime.Now;
                borrowRecord.IsReturned = true;
                
                // Update the record in current borrows
                _currentBorrowsData.Update(borrowRecord, b => b.Id == borrowId);
                
                // Update in user's borrow history
                var userRecordPath = Path.Combine(USER_RECORDS_PATH, $"{borrowRecord.UserId}.xml");
                var userBorrowsData = new XmlDataAccess<BorrowRecord>(userRecordPath);
                userBorrowsData.Update(borrowRecord, b => b.Id == borrowId);
                
                // Update book inventory (increment available copies) using the new client
                var bookServiceClient = new BookServiceClient(_bookServiceEndpoint);
                bookServiceClient.UpdateInventory(new InventoryUpdate { BookId = borrowRecord.BookId, QuantityChange = 1 });
                
                // Log the return action
                var member = GetMemberById(borrowRecord.UserId);
                var log = new BorrowLog
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = borrowRecord.UserId,
                    UserName = member?.Name ?? borrowRecord.UserId,
                    BookId = borrowRecord.BookId,
                    BookTitle = borrowRecord.BookTitle,
                    ActionType = "Return",
                    ActionDate = DateTime.Now
                };
                _logsData.Insert(log);
                
                return borrowRecord;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error during return: {ex.Message}");
                throw;
            }
        }

        public List<BorrowRecord> GetCurrentBorrowsByUser(string userId)
        {
            var allBorrows = _currentBorrowsData.GetAll();
            return allBorrows
                .Where(b => b.UserId == userId && !b.IsReturned)
                .ToList();
        }

        public List<BorrowRecord> GetBorrowHistory(string userId)
        {
            var userRecordPath = Path.Combine(USER_RECORDS_PATH, $"{userId}.xml");
            if (!File.Exists(userRecordPath))
            {
                return new List<BorrowRecord>();
            }
            
            var userBorrowsData = new XmlDataAccess<BorrowRecord>(userRecordPath);
            return userBorrowsData.GetAll();
        }

        public List<BorrowLog> GetAllLogs()
        {
            return _logsData.GetAll();
        }

        public Member GetMemberById(string userId)
        {
            return _membersData.GetById(m => m.Id == userId);
        }

        public Member CreateMember(Member member)
        {
            if (string.IsNullOrEmpty(member.Id))
            {
                member.Id = Guid.NewGuid().ToString();
            }
            
            if (member.MembershipDate == DateTime.MinValue)
            {
                member.MembershipDate = DateTime.Now;
            }
            
            _membersData.Insert(member);
            return member;
        }
    }
} 