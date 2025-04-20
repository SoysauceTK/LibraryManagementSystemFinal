using System;

public class User
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public UserRole Role { get; set; }
    public DateTime RegistrationDate { get; set; }
}

// Enum for user roles
public enum UserRole
{
    Member,
    Staff,
    Admin
}