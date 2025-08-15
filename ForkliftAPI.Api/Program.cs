using ForkliftAPI.Application.Interfaces;
using ForkliftAPI.Application.MappingProfiles;
using ForkliftAPI.Application.Services;
using ForkliftAPI.Infrastructure.Data;
using ForkliftAPI.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add DbContext from Infrastructure
builder.Services.AddDbContext<ForkliftContext>(options =>
    options.UseSqlite(connectionString));

// Register repository and service (DI)
builder.Services.AddScoped<IForkliftRepository, ForkliftRepository>();
builder.Services.AddScoped<IForkliftService, ForkliftService>();

builder.Services.AddAutoMapper(typeof(ForkliftProfile).Assembly);

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")  // React dev server URL
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowFrontendApp");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();