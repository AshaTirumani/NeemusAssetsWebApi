using Microsoft.EntityFrameworkCore;
using NeemusAssetWebAPI;
using NeemusAssetWebAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

builder.Services.AddOpenApi();

// ================= DATABASE CONTEXTS =================

// Asset AMS Database
builder.Services.AddDbContext<PostgreDBContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("AssetAMSConnection")));

// Asset Common Database
builder.Services.AddDbContext<AssetCommonDBContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("AssetCommonConnection")));

// Asset SAP Database
builder.Services.AddDbContext<AssetSAPDBContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("AssetSAPCommonConnection")));

// ERP Database
builder.Services.AddDbContext<ERPDBContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("AssetERPCommonConnection")));

// ================= CORS =================

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

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();