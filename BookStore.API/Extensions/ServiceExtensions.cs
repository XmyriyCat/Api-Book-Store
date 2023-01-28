using System.Text;
using ApiBookStore.MiddlewareHandlers;
using BLL.Errors;
using BLL.Infrastructure.Mapper;
using BLL.Infrastructure.Validators.Author;
using BLL.Services.Contract;
using BLL.Services.Implementation;
using DLL.Data;
using DLL.Models;
using DLL.Repository.UnitOfWork;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
// ReSharper disable StringLiteralTypo

namespace ApiBookStore.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureMsSqlServerContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("ConnectionStringBookDbSql");
            services.AddDbContext<ShopDbContext>(options => options.UseSqlServer(connectionString));
        }

        public static void ConfigureIdentityCore(this IServiceCollection services)
        {
            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ "; // added space
            })
            .AddEntityFrameworkStores<ShopDbContext>()
            .AddDefaultTokenProviders();
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
            services.AddScoped<IAdminCatalogService, AdminCatalogService>();
            services.AddScoped<IAuthorCatalogService, AuthorCatalogService>(); 
            services.AddScoped<IBookCatalogService, BookCatalogService>(); 
            services.AddScoped<IGenreCatalogService, GenreCatalogService>(); 
            services.AddScoped<IPublisherCatalogService, PublisherCatalogService>();
            services.AddScoped<IUserCatalogService, UserCatalogService>();
            services.AddScoped<IDeliveryCatalogService, DeliveryCatalogService>();
            services.AddScoped<IPaymentWayCatalogService, PaymentWayCatalogService>();
            services.AddScoped<IShipmentCatalogService, ShipmentCatalogService>();
            services.AddScoped<IRoleCatalogService, RoleCatalogService>();
            services.AddScoped<IWarehouseCatalogService, WarehouseCatalogService>();
            services.AddScoped<IWarehouseBookCatalogService, WarehouseBookCatalogService>();
            services.AddScoped<IOrderCatalogService, OrderCatalogService>();
            services.AddScoped<IOrderLineCatalogService, OrderLineCatalogService>();

            // TODO: Add other services later
        }

        public static void AppendGlobalErrorHandler(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<GlobalErrorHandler>();
        }

        public static void ConfigureJwtTokenService(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenJwtService>();
        }

        public static void ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; 
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
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
        
        public static void ConfigureGoogleTokenService(this IServiceCollection services)
        {
            services.AddScoped<IGoogleTokenService, GoogleTokenService>();
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
