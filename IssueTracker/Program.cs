using IssueTracker.Exceptions;
using IssueTracker.Services.IssueService;
using IssueTracker.Services.ProjectService;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace IssueTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create a WebApplication builder
            var builder = WebApplication.CreateBuilder(args);

            var connectionString =
                builder.Configuration.GetConnectionString("DefaultConnection");

            // SERIALIZATION: Configure JSON serialization options to handle enum values as strings
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter()
                );
            });

            // Register OpenAPI services
            builder.Services.AddOpenApi();

            // Register AddExceptionHandler
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            //standardized error response format.
            builder.Services.AddProblemDetails();

            // Add database context with SQL Server connection
            builder.Services.AddDbContext<Data.IssueTrackerDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Register services for dependency injection
            builder.Services.AddScoped<IIssueService, IssueService>();
            builder.Services.AddScoped<IProjectService, ProjectService>();

            // Build the application
            var app = builder.Build();


            // Enable OpenAPI/Swagger for API documentation
            app.MapOpenApi();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(
                        
                        "/openapi/v1.json",
                        "Issue Tracker API V1"
                        );
                });
            }

            // use ExceptionHanlder
            app.UseExceptionHandler();

            // Configure the HTTP request pipeline
            app.UseHttpsRedirection();

            // Enable routing and map controller endpoints
            app.MapControllers();

            // Run the application
            app.Run();
        }
    }
}