using System.ComponentModel.DataAnnotations;
using EY.DueDiligenceScreening.API.Screening.Domain.Model.ValueObjects;

namespace EY.DueDiligenceScreening.API.Providers.Interfaces.REST.Resources;

public record ExecuteScreeningRequest(
    [Required][MinLength(1)] List<ScreeningSource> Sources);

