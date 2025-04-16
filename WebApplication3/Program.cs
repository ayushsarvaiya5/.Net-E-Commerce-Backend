
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApplication3.Data;
using WebApplication3.Mappings;
using WebApplication3.Repositories;
using WebApplication3.Services;
using WebApplication3.Utils;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using System.Globalization;
using WebApplication3.Middleware;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using WebApplication3.Model;
using WebApplication3.Interfaces;
using Serilog;
using WebApplication3.Middlewares;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.ResponseCompression;

namespace WebApplication3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            // Configure Serilog with structured JSON logging
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("Logs/Combine/log-.json", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information, formatProvider: null,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} {Properties:j}{NewLine}")
                .WriteTo.File("Logs/Error/log-.json", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error, formatProvider: null,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} {Properties:j}{NewLine}")
                .CreateLogger();

            // Register DbContext and configure PostgreSQL
            builder.Services.AddDbContext<MobileDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

            // adding controller routing & for ModelState error handeling
            builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return new BadRequestObjectResult(new ApiError(
                        (int)HttpStatusCode.BadRequest,
                        "Validation Failed",
                        errors
                    ));
                };
            });

            // AutoMapper -> for autmatting converting into DTOs
            builder.Services.AddAutoMapper(typeof(UserMapping));
            builder.Services.AddAutoMapper(typeof(MobileProfile));

            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    policy => policy.WithOrigins("http://127.0.0.1:5500", "http://127.0.0.1:5500/CORS_Testing.html")
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });


            // JWT Authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:SecretKey"]))
                };
        
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        // Read token from cookies if Authorization header is missing
                        var token = context.Request.Cookies["AccessToken"];
                        if (!string.IsNullOrEmpty(token))
                        {
                            context.Token = token;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            // RBAC
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("CustomerOnly", policy => policy.RequireRole("Customer"));
                options.AddPolicy("EmployeeOnly", policy => policy.RequireRole("Employee"));
            });


            builder.Services.AddMemoryCache();

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration["RedisConnectionStrings:Redis"];
            });

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your valid token."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            // Api Versioning
            builder.Services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0); // v1 is default
                options.ReportApiVersions = true;
                options.ApiVersionReader = new Microsoft.AspNetCore.Mvc.Versioning.UrlSegmentApiVersionReader();
            });

            builder.Services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV"; // Format: v1, v2
                options.SubstituteApiVersionInUrl = true;
            });

            builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

            // Rate Limiting => Window
            builder.Services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.OnRejected = async (context, cancellationToken) =>
                {
                    var response = new ApiResponse<string>(
                        429,
                        "Too many requests. Try again later."
                    );

                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    await context.HttpContext.Response.WriteAsJsonAsync(response, cancellationToken);
                };

                options.AddPolicy<string>("RateLimiter", requestContext =>
                {
                    return RateLimitPartition.GetSlidingWindowLimiter(
                        requestContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        ip => new SlidingWindowRateLimiterOptions
                        {
                            PermitLimit = requestContext.Request.Path.ToString() switch
                            {
                                "/api/User/Get-CurrentUser" => 2,
                                _ => 15
                            },
                            Window = TimeSpan.FromMinutes(1),
                            SegmentsPerWindow = 6
                        });
                });
            });

            // OData Model Builder
            var modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<MobileModel>("Mobiles"); // Define MobileModel as an OData entity

            // Add OData services
            builder.Services.AddControllers()
                .AddOData(options =>
                    options.Select()  // Enable $select
                           .Filter()  // Enable $filter
                           .Expand()  // Enable $expand
                           .OrderBy() // Enable $orderby
                           .SetMaxTop(100)  // Limit max results
                           .Count() // Enable $count
                           .AddRouteComponents("odata", modelBuilder.GetEdmModel()));


            // Response Compression 
            builder.Services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                {
                    "application/json", // compress JSON responses
                    "text/plain",
                    "text/css",
                    "application/javascript",
                    "text/html",
                    "image/svg+xml"
                });
            });

            builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = System.IO.Compression.CompressionLevel.Optimal; // 3 Options : Fastest, Optimal, NoCompression
            });

            builder.Services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = System.IO.Compression.CompressionLevel.Optimal;
            });


            // DI - Dependency Injection

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            
            builder.Services.AddScoped<IFeaturesRepository, FeaturesRepository>();
            builder.Services.AddScoped<IFeaturesService, FeaturesService>();

            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<ICartService, CartService>();

            builder.Services.AddScoped<IMobileRepository, MobileRepository>();
            builder.Services.AddScoped<IMobileService, MobileService>();

            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderService, OrderService>();


            builder.Services.AddSignalR();

            // Cloudinary Service
            builder.Services.AddSingleton<CloudinaryService>();

            // Cashing Service
            builder.Services.AddScoped<ICacheService, CacheService>();

            // For Optimally Removing Cashe Key from Redis
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
                ConnectionMultiplexer.Connect(builder.Configuration.GetSection("RedisConnectionStrings:Redis").Value));

            var app = builder.Build();


            // Swagger
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                                                description.GroupName.ToUpperInvariant());
                    }
                });
            }

            // Middlewares

            app.UseMiddleware<RequestLoggingMiddleware>();  // Logging middleware

            app.UseMiddleware<ExceptionMiddleware>();       // Global Exception Handling Middleware

            app.UseRateLimiter();                           // Ratelimiting Middleware

            app.UseCors("CorsPolicy");                      // Cors Middleware

            app.UseResponseCompression();                   // Response Compression

            app.UseAuthentication();                        // Athentication Middleware

            app.UseMiddleware<UserMiddleware>();            // Custom Middleware

            app.UseAuthorization();                         // Authorization Middleware

            app.MapControllers();

            app.Run();
        }
    }
}



