using System.ComponentModel.DataAnnotations;

namespace EY.DueDiligenceScreening.API.IAM.Interfaces.REST.Resources;

public record SignInResource(
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    string Email,

    [Required(ErrorMessage = "Password is required")]
    string Password
);

