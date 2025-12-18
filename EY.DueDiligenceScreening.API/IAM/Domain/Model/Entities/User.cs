namespace EY.DueDiligenceScreening.API.IAM.Domain.Model.Entities;

public class User
{
    public long UserID { get; set; }
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public User() { }

    public User(string email, string passwordHash, string fullName)
    {
        Email = email;
        PasswordHash = passwordHash;
        FullName = fullName;
        CreatedAt = DateTime.UtcNow;
    }
}

