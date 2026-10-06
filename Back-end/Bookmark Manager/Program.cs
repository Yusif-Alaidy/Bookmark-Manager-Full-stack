
using Bookmark_Manager.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookmark_Manager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            #region DB Connection
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
               sqlOptions => sqlOptions.EnableRetryOnFailure(
                                    maxRetryCount: 5,                  // Maximum number of retry attempts
                                    maxRetryDelay: TimeSpan.FromSeconds(10), // Maximum delay between retries
                                    errorNumbersToAdd: null            // Additional SQL error numbers to treat as transient
)
                ));
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
