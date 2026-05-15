using Microsoft.EntityFrameworkCore;
using NeemusAssetWebAPI;
using NeemusAssetWebAPI.Data;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// ✅ REGISTER DB CONTEXT (THIS FIXES YOUR ERROR)
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<PostgreDBContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("AssetAMSConnection")));

builder.Services.AddDbContext<AssetCommonDBContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("AssetCommonConnection")));
// ADD CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
