using System.ComponentModel.DataAnnotations;

namespace EY.DueDiligenceScreening.API.IAM.Interfaces.REST.Resources;

public record RegisterUserResource(
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    string Email,

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    string Password,

    [Required(ErrorMessage = "Full name is required")]
    [MinLength(2, ErrorMessage = "Full name must be at least 2 characters")]
    string FullName
);

