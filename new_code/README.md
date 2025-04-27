# Library Management System - Borrowing Service

This is a WCF service that handles book borrowing operations for the Library Management System.

## Features

- Book checkout and return operations
- User borrow history tracking
- Log of all borrowing and return actions
- Integration with Book Storage service to manage inventory
- Member management

## Data Storage

The service maintains three types of XML data files:

1. `App_Data/logs.xml` - Logs of all borrowing and return actions
2. `App_Data/current_borrows.xml` - Currently borrowed books
3. `App_Data/user_records/[userId].xml` - Individual user borrowing history

## Service Endpoints

The service provides the following endpoints:

- `CheckoutBook` - Allows a user to borrow a book
- `ReturnBook` - Allows a user to return a borrowed book
- `GetCurrentBorrowsByUser` - Gets all books currently borrowed by a user
- `GetBorrowHistory` - Gets the entire borrowing history for a user
- `GetAllLogs` - Gets all borrowing and return logs
- `GetMemberById` - Gets member information
- `CreateMember` - Creates a new member

## Integration

This service communicates with the Book Storage service to manage book inventory. When a book is borrowed, the available copies count is decremented. When a book is returned, the count is incremented back.

## Setup Instructions

1. Update the `_bookServiceEndpoint` in `BorrowingService.svc.cs` to point to your BookService.
2. Ensure the App_Data directory has write permissions.
3. Add a reference to this service in your main application.

## Implementation Notes

- Book quantity is set to 10 for all books by default.
- Checkout period is 14 days by default.
- User records are created automatically on first borrow. 