using Talabat.API.Errors;
using Talabat.API.Extensions;
using Talabat.API.Middlewares;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories;
using Talabat.DataAccess.Data;
using Talabat.DataAccess.Identity;
using Talabat.DataAccess.Reposioties;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Talabat.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // Configure Services fot app
            builder.Services.AddAplicationServices(builder.Configuration);

            builder.Services.AddSwaggerServices();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", options =>
                {
                    //options.AllowAnyHeader().AllowAnyMethod();
                    //options.AllowAnyHeader().WithMethods("GET", "PUT");
                    //options.AllowAnyHeader().WithOrigins(builder.Configuration["FrontBaseUrl"]);
                    options.AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowAnyOrigin();
                });
            });

            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                var dbContext = services.GetRequiredService<AppDbContext>();
                await dbContext.Database.MigrateAsync();

                var identityDbContext = services.GetRequiredService<AppIdentityDbContext>();
                await identityDbContext.Database.MigrateAsync();

                await SeedAppDbContext.SeedAsync(dbContext);

                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                await SeedAppIdentityDbContext.SeedUserAsyc(userManager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex.Message);
            }

            // Server Error
            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddlewares();
            }

            // Not Found Endpoint
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseStaticFiles();


            // to allow cnnect with another project (Angular)
            app.UseCors("MyPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
