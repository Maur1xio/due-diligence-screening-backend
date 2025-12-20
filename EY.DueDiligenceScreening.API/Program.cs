using EY.DueDiligenceScreening.API.IAM.Application.CommandServices;
using EY.DueDiligenceScreening.API.IAM.Application.Internal;
using EY.DueDiligenceScreening.API.IAM.Application.QueryServices;
using EY.DueDiligenceScreening.API.IAM.Domain.Repositories;
using EY.DueDiligenceScreening.API.IAM.Domain.Services;
using EY.DueDiligenceScreening.API.IAM.Infrastructure.Repositories;
using EY.DueDiligenceScreening.API.Providers.Application.Services;
using EY.DueDiligenceScreening.API.Providers.Domain.Repositories;
using EY.DueDiligenceScreening.API.Providers.Domain.Services;
using EY.DueDiligenceScreening.API.Providers.Infrastructure.Persistence;
using EY.DueDiligenceScreening.API.Screening.Application.Services;
using EY.DueDiligenceScreening.API.Screening.Domain.Services;
using EY.DueDiligenceScreening.API.Screening.Infrastructure.Scrapers;
using EY.DueDiligenceScreening.API.Shared.Infrastructure.Extensions;
using EY.DueDiligenceScreening.API.Shared.Infrastructure.Persistence.EFC;
using EY.DueDiligenceScreening.API.Shared.Infrastructure.Settings;
using EY.DueDiligenceScreening.API.Shared.Infrastructure.Swagger;
using EY.DueDiligenceScreening.API.Shared.Interfaces.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Threading.RateLimiting;


var builder = WebApplication.CreateBuilder(args);

// 🔥 Configuración para producción: Priorizar variables de entorno sobre appsettings.json
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 20,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0  
            }));

    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.ContentType = "application/json";
        
        var response = new
        {
            success = false,
            statusCode = 429,
            message = "Too many requests. Please try again later.",
            errors = new
            {
                code = "RATE_LIMIT_EXCEEDED",
                details = "You have exceeded the maximum number of 20 requests per minute.",
                retryAfter = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter)
                    ? retryAfter.TotalSeconds
                    : 60
            },
            timestamp = DateTime.UtcNow
        };

        await context.HttpContext.Response.WriteAsJsonAsync(response, cancellationToken);
    };
});

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings!.Secret)),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
    
    options.ConfigureCustomEvents();
});

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        // 🔥 CORS dinámico: Lee de configuración o usa valores por defecto
        var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() 
            ?? new[] { "http://localhost:3000", "http://localhost:5173" };
        
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiResponseFilter>();
    options.Filters.Add<GlobalExceptionFilter>();
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
})
.ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value?.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
            );

        var errorResponse = new EY.DueDiligenceScreening.API.Shared.Interfaces.Response.ApiErrorResponse(
            400,
            "Validation failed",
            errors
        );

        return new BadRequestObjectResult(errorResponse);
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EY Screening API",
        Version = "v1",
        Description = "API for high-risk entity screening and supplier due diligence",
        Contact = new OpenApiContact
        {
            Name = "EY Forensics & Integrity Services",
            Email = "forensics@ey.com"
        }
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
    
    c.UseInlineDefinitionsForEnums();
    
    c.SchemaFilter<ExampleSchemaFilter>();
});

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IAuthCommandService, AuthCommandService>();
builder.Services.AddScoped<IAuthQueryService, AuthQueryService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IOfacScraperService, OfacPlaywrightScraperService>();
builder.Services.AddScoped<IOffshoreLeaksScraperService, OffshoreLeaksPlaywrightScraperService>();
builder.Services.AddScoped<IWorldBankScraperService, WorldBankPlaywrightScraperService>();

builder.Services.AddScoped<IMultiSourceScreeningService, MultiSourceScreeningService>();

builder.Services.AddScoped<IProviderRepository, ProviderRepository>();
builder.Services.AddScoped<IScreeningHistoryRepository, ScreeningHistoryRepository>();
builder.Services.AddScoped<IProviderCommandService, ProviderCommandService>();
builder.Services.AddScoped<IProviderQueryService, ProviderQueryService>();
builder.Services.AddScoped<IScreeningHistoryService, ScreeningHistoryService>();

var app = builder.Build();

//acolocaar

app.UseRateLimiter();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "EY Screening API v1");
    c.RoutePrefix = string.Empty; 
});

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();
