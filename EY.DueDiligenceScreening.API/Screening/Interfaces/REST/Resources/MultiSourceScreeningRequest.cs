using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using EY.DueDiligenceScreening.API.Screening.Domain.Model.ValueObjects;

namespace EY.DueDiligenceScreening.API.Screening.Interfaces.REST.Resources;

public record MultiSourceScreeningRequest(
    [Required(ErrorMessage = "Company name is required")]
    [MinLength(2, ErrorMessage = "Company name must be at least 2 characters")]
    [DefaultValue("Peru")]
    string CompanyName,
    
    [Required(ErrorMessage = "At least one source must be specified")]
    [MinLength(1, ErrorMessage = "At least one source must be specified")]
    List<ScreeningSource> Sources
)
{
    public static MultiSourceScreeningRequest Example => new(
        CompanyName: "Peru",
        Sources: new List<ScreeningSource> 
        { 
            ScreeningSource.OFAC, 
            ScreeningSource.OffshoreLeaks, 
            ScreeningSource.WorldBank 
        }
    );
};

