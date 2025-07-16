using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Zora.Core;
using Zora.Core.Config;
using Zora.Core.Features.Auth;

var builder = WebApplication.CreateBuilder(args);

// ------------------ JWT Settings ------------------
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var jwtKey = Encoding.UTF8.GetBytes(jwtSettings.Key);

// ------------------ Authentication ------------------
builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(jwtKey),
        };
    });

builder.Services.AddAuthorization();

// ------------------ Your App Services ------------------
builder.Services.AddDatabase();
builder.Services.AddRequiredServices();
builder.Services.AddScoped<PasswordHasher<Zora.Core.Database.Models.UserModel>>();

// ------------------ Controllers ------------------
builder
    .Services.AddControllers(options =>
    {
        options.SuppressAsyncSuffixInActionNames = false;
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();
app.Use(
    async (context, next) =>
    {
        try
        {
            await next();
        }
        catch (Exception ex)
        {
            Console.WriteLine("=== UNHANDLED EXCEPTION ===");
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            throw;
        }
    }
);

// ------------------ Middleware ------------------
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Zora API V1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication(); // MUST be before UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();
