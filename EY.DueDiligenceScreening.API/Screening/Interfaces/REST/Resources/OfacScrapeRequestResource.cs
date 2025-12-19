using System.ComponentModel.DataAnnotations;

namespace EY.DueDiligenceScreening.API.Screening.Interfaces.REST.Resources;

public record OfacScrapeRequestResource(
    [Required(ErrorMessage = "Company name is required")]
    [MinLength(2, ErrorMessage = "Company name must be at least 2 characters")]
    string CompanyName
);

