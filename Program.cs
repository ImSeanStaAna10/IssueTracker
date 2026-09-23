
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

            // SERIALIZATION: Configure JSON serialization options to handle enum values as strings
            builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(
                new JsonStringEnumConverter()
            );
        });


            // Register AddExceptionHandler
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            //standardized error response format.
            builder.Services.AddProblemDetails();

            // Add database context with SQL Server connection
            builder.Services.AddDbContext<Data.IssueTrackerDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register services for dependency injection
            builder.Services.AddScoped<IIssueService, IssueService>();
            builder.Services.AddScoped<IProjectService, ProjectService>();

            // Build the application
            var app = builder.Build();


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
