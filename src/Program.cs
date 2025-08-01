using SensitiveWordsAPI.Extensions;
using System.Data;
using Microsoft.Data.SqlClient;
using SensitiveWordsAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add configuration from appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Add services to the container.
builder.Services.AddControllers();

// Register repositories and services
builder.Services.RegisterServices(builder.Configuration);

builder.Services.Configure<MemoryCacheConfiguration>(
    builder.Configuration.GetSection("MemoryCacheSettings"));

// Add Swagger for API docs (optional)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
