using ApiBookStore.Extensions;

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
            builder.Services.ConfigureSwaggerJwtAuthentication(); // For Jwt working in Swagger

            var app = builder.Build();

            app.AppendGlobalErrorHandler();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}