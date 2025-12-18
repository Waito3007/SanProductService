using BackgroundLogService;
using Microsoft.EntityFrameworkCore;
using SANProductService.Product.Application;
using SANProductService.Product.Infrastructure;
using SANProductService.Product.API.Middleware;
using SANProductService.Product.Infrastructure.Data;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Product Service API", Version = "v1" });
});

// DI
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure(builder.Configuration);

//Log Service
builder.Services.AddBackgroundLogService(
    builder.Configuration, 
    "SANProductService"
);

// CORS Configuration
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Đăng ký Global Exception Middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();