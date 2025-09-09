using BAMK.API;
using BAMK.API.Configuration;
using BAMK.API.Services;
using BAMK.Application.Services;
using BAMK.Infrastructure.Data;
using BAMK.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add JWT Configuration
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Add Infrastructure Services
builder.Services.AddInfrastructureServices(builder.Configuration);

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(BAMK.Application.Mappings.TShirtMappingProfile), typeof(BAMK.Application.Mappings.OrderMappingProfile), typeof(BAMK.Application.Mappings.QuestionMappingProfile));

// Add Application Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITShirtService, TShirtService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();

// Add JWT Services
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();

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

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Auto Migration - Veritabanını otomatik güncelle
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<BAMKDbContext>();
        context.Database.Migrate();
        Console.WriteLine("✅ Veritabanı başarıyla güncellendi (Auto Migration)");

        // Test verilerini oluştur
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        var tShirtService = scope.ServiceProvider.GetRequiredService<ITShirtService>();
        var questionService = scope.ServiceProvider.GetRequiredService<IQuestionService>();
        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
        
        var seeder = new TestDataSeeder(userService, tShirtService, questionService, orderService);
        await seeder.SeedAllTestDataAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Veritabanı güncelleme hatası: {ex.Message}");
        // Uygulama çalışmaya devam etsin, sadece log yazdır
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BAMK API v1");
        c.RoutePrefix = "swagger"; // Swagger UI'ı /swagger adresinde aç
    });
}

// Ana sayfa yönlendirmesi - Swagger'a yönlendir
app.MapGet("/", () => Results.Redirect("/swagger/index.html"));

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
