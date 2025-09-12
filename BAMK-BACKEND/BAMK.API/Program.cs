using BAMK.API;
using BAMK.API.Configuration;
using BAMK.API.Middleware;
using BAMK.API.Services;
using BAMK.Application.Services;
using BAMK.Application.Validation;
using BAMK.Infrastructure.Data;
using BAMK.Infrastructure.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// ✅ Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateTShirtDtoValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add JWT Configuration
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Add Infrastructure Services
builder.Services.AddInfrastructureServices(builder.Configuration);

// Add AutoMapper
builder.Services.AddAutoMapper(
    typeof(BAMK.Application.Mappings.TShirtMappingProfile),
    typeof(BAMK.Application.Mappings.ProductDetailMappingProfile),
    typeof(BAMK.Application.Mappings.OrderMappingProfile),
    typeof(BAMK.Application.Mappings.QuestionMappingProfile),
    typeof(BAMK.Application.Mappings.CartMappingProfile),
    typeof(BAMK.Application.Mappings.UserMappingProfile) // Added for User
);

// Add Application Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITShirtService, TShirtService>();
builder.Services.AddScoped<IProductDetailService, ProductDetailService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IValidationService, ValidationService>();
builder.Services.AddScoped<BAMK.Application.Services.ICartService, BAMK.Application.Services.CartService>();

// Add JWT Services
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Add API Services
builder.Services.AddScoped<IProductMappingService, ProductMappingService>();
builder.Services.AddScoped<ICartService, CartService>();

// ✅ Add Caching
builder.Services.AddMemoryCache();
builder.Services.AddScoped<BAMK.Core.Interfaces.ICacheService, BAMK.Core.Services.CacheService>();
builder.Services.AddScoped<BAMK.Core.Interfaces.ILoggingService, BAMK.Core.Services.LoggingService>();

// Add JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
if (jwtSettings?.SecretKey == null)
{
    throw new InvalidOperationException("JWT settings not configured properly");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SecretKey)),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// ✅ CORS: Sadece frontend'e izin ver
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://127.0.0.1:3000", "http://localhost:3002")
               .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

// ✅ Veritabanı migration ve test data
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<BAMKDbContext>();
        context.Database.Migrate();
        Console.WriteLine("✅ Veritabanı başarıyla güncellendi (Auto Migration)");

        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        var tShirtService = scope.ServiceProvider.GetRequiredService<ITShirtService>();
        var productDetailService = scope.ServiceProvider.GetRequiredService<IProductDetailService>();
        var questionService = scope.ServiceProvider.GetRequiredService<IQuestionService>();
        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

        var cartService = scope.ServiceProvider.GetRequiredService<BAMK.Application.Services.ICartService>();
        var seeder = new TestDataSeeder(userService, tShirtService, productDetailService, questionService, orderService, cartService, context);
        
        // Environment variable ile database reset kontrolü
        var resetDatabase = Environment.GetEnvironmentVariable("RESET_DATABASE");
        if (resetDatabase?.ToLower() == "true")
        {
            Console.WriteLine("🗑️ Database sıfırlanıyor...");
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            Console.WriteLine("✅ Database sıfırlandı");
        }
        
        await seeder.SeedAllTestDataAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Veritabanı güncelleme hatası: {ex.Message}");
    }
}

// ✅ Middleware sırası önemli
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BAMK API v1");
        c.RoutePrefix = "swagger";
    });
}

app.MapGet("/", () => Results.Redirect("/swagger/index.html"));

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");           // ❗ CORS middleware burada
app.UseMiddleware<ValidationMiddleware>(); // ✅ Validation middleware
app.UseMiddleware<GlobalExceptionMiddleware>(); // Global exception handling
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
