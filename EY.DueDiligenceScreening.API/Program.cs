using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using EY.DueDiligenceScreening.API.IAM.Application.CommandServices;
using EY.DueDiligenceScreening.API.IAM.Application.Internal;
using EY.DueDiligenceScreening.API.IAM.Application.QueryServices;
using EY.DueDiligenceScreening.API.IAM.Domain.Repositories;
using EY.DueDiligenceScreening.API.IAM.Domain.Services;
using EY.DueDiligenceScreening.API.IAM.Infrastructure.Repositories;
using EY.DueDiligenceScreening.API.Screening.Domain.Services;
using EY.DueDiligenceScreening.API.Screening.Infrastructure.Scrapers;
using EY.DueDiligenceScreening.API.Shared.Infrastructure.Persistence.EFC;
using EY.DueDiligenceScreening.API.Shared.Infrastructure.Settings;


var builder = WebApplication.CreateBuilder(args);

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
});

builder.Services.AddAuthorization();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});


builder.Services.AddControllers(options =>
{
    options.Filters.Add<EY.DueDiligenceScreening.API.Shared.Interfaces.Filters.ApiResponseFilter>();
    options.Filters.Add<EY.DueDiligenceScreening.API.Shared.Interfaces.Filters.GlobalExceptionFilter>();
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
});


builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IAuthCommandService, AuthCommandService>();
builder.Services.AddScoped<IAuthQueryService, AuthQueryService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IOfacScraperService, OfacPlaywrightScraperService>();


var app = builder.Build();

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
    try
    {
        dbContext.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while creating the database.");
    }
}

app.Run();
