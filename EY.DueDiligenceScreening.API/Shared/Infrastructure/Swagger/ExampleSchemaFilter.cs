using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using EY.DueDiligenceScreening.API.Screening.Interfaces.REST.Resources;
using EY.DueDiligenceScreening.API.Screening.Domain.Model.ValueObjects;

namespace EY.DueDiligenceScreening.API.Shared.Infrastructure.Swagger;

public class ExampleSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(MultiSourceScreeningRequest))
        {
            schema.Example = new OpenApiObject
            {
                ["companyName"] = new OpenApiString("Lima"),
                ["sources"] = new OpenApiArray
                {
                    new OpenApiString("OFAC"),
                    new OpenApiString("OffshoreLeaks"),
                    new OpenApiString("WorldBank")
                }
            };
        }
    }
}

