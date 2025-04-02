using Admin.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Ajout du service FastApiService
builder.Services.AddHttpClient<FastApiService>(client =>
{
    client.Timeout = TimeSpan.FromMinutes(5); // Désactiver le timeout pour tester
});
builder.Services.AddScoped<FastApiService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<UserContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DevConnection")));

builder.Services.AddAuthentication(x =>
{

    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuperSecretKeyForJWTGeneration2025.....")),
        ValidateAudience = false,
        ValidateIssuer=false,
        ClockSkew=TimeSpan.Zero
    };
});
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(options =>
            options.WithOrigins("http://localhost:4200/")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowAnyOrigin()
            );
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
