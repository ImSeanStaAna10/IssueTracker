
using IssueTracker.Exceptions;
using IssueTracker.Services.IssueService;
using IssueTracker.Services.ProjectService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

namespace IssueTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString =
                builder.Configuration.GetConnectionString("DefaultConnection");

            // JSON serialization: enums are returned as strings
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter());
            });

            // JWT authentication
            var jwtKey = builder.Configuration["Jwt:Key"]
                ?? throw new InvalidOperationException(
                    "JWT key is missing.");

            var jwtIssuer = builder.Configuration["Jwt:Issuer"];
            var jwtAudience = builder.Configuration["Jwt:Audience"];

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = jwtIssuer,

                            ValidateAudience = true,
                            ValidAudience = jwtAudience,

                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(jwtKey)),

                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.FromSeconds(30)
                        };
                });

            builder.Services.AddAuthorization();

            // OpenAPI + JWT Bearer security scheme
            builder.Services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer<
                    BearerSecuritySchemeTransformer>();
            });

            // Exception handling
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            // Database
            builder.Services.AddDbContext<Data.IssueTrackerDbContext>(
                options => options.UseSqlServer(connectionString));

            // Dependency injection
            builder.Services.AddScoped<IIssueService, IssueService>();
            builder.Services.AddScoped<IProjectService, ProjectService>();

            builder.Services.AddScoped<
                Services.AuthenticationService.IAuthService,
                Services.AuthenticationService.AuthService>();

            // Build the application AFTER all service registrations
            var app = builder.Build();

            // OpenAPI document
            app.MapOpenApi();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(
                        "/openapi/v1.json",
                        "Issue Tracker API V1");
                });
            }

            app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }))
   .AllowAnonymous();

            app.UseExceptionHandler();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
