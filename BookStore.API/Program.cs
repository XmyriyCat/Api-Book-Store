using ApiBookStore.Extensions;
using DLL.Models;
using Microsoft.AspNetCore.Identity;

namespace ApiBookStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddRepositoryWrapper();
            builder.Services.ConfigureMsSqlServerContext(builder.Configuration);
            builder.Services.ConfigureAutoMapper();
            builder.Services.ConfigureFluentValidation();
            builder.Services.ConfigureDtoServices();
            builder.Services.ConfigureNewtonJson();
            builder.Services.ConfigureJwtTokenService();
            builder.Services.ConfigureJwtAuthentication(builder.Configuration);
            builder.Services.ConfigureOAuth2Authentication(builder.Configuration);
            //builder.Services.ConfigureSwaggerJwtAuthentication(); // For Jwt working in Swagger
            builder.Services.ConfigureSwaggerOAuth2Authentication(); // For OAuth2 working in Swagger
            
            var app = builder.Build();

            app.AppendGlobalErrorHandler();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.OAuthClientId(builder.Configuration["Authentication:Google:ClientId"]);
                    c.OAuthClientSecret(builder.Configuration["Authentication:Google:ClientSecret"]);
                    c.OAuth2RedirectUrl("https://localhost:8000/signin-google");
                    c.OAuthScopes("email");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}