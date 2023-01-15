using System.Text;
using ApiBookStore.MiddlewareHandlers;
using BLL.Errors;
using BLL.Infrastructure.Mapper;
using BLL.Infrastructure.Validators.Author;
using BLL.Services.Contract;
using BLL.Services.Implementation;
using DLL.Data;
using DLL.Repository.UnitOfWork;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace ApiBookStore.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureMsSqlServerContext(this IServiceCollection services, IConfiguration config)
        {
            string connectionString = config.GetConnectionString("ConnectionStringBookDbSql");
            services.AddDbContext<ShopDbContext>(options => options.UseSqlServer(connectionString));
        }

        public static void AddRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }

        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
        }

        public static void ConfigureFluentValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<CreateAuthorDtoValidator>();
        }

        public static void ConfigureNewtonJson(this IServiceCollection services) // to ignore recursion in json objects
        {
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
        }

        public static void ConfigureDtoServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthorCatalogService, AuthorCatalogService>();
            services.AddScoped<IBookCatalogService, BookCatalogService>();
            services.AddScoped<IGenreCatalogService, GenreCatalogService>();
            services.AddScoped<IPublisherCatalogService, PublisherCatalogService>();
            services.AddScoped<IUserCatalogService, UserCatalogService>();
            services.AddScoped<IDeliveryCatalogService, DeliveryCatalogService>();
            // TODO: Add other services later
        }

        public static void AppendGlobalErrorHandler(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<GlobalErrorHandler>();
        }

        public static void ConfigureJwtTokenService(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
        }

        public static void ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtToken:Key"] ?? throw new JwtKeyIsNotFound("JWT key is null!"))),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }

        public static void ConfigureSwaggerJwtAuthentication(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "My API",
                    Version = "v1"
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
        }
    }
}
