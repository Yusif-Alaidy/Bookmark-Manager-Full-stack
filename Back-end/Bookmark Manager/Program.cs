
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

            #region CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular", policy =>
                    policy.WithOrigins("http://localhost:4200")   // add your real frontend URL later
                          .AllowAnyHeader()
                          .AllowAnyMethod());
            });

            #endregion

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

            builder.Services.AddEndpointsApiExplorer(); // Required for Minimal APIs / routing mapping
            builder.Services.AddSwaggerGen();


            var app = builder.Build();

            // CORS ---------------------------------------
            app.UseCors("AllowAngular");
            // --------------------------------------------

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                // Enables the middleware to serve the generated JSON document
                app.UseSwagger();

                // Enables the interactive Swagger UI web page
                app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
