﻿using System;

public class BorrowRecord
{
    public string Id { get; set; }
    public string BookId { get; set; }
    public string UserId { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public bool IsReturned { get; set; }
}