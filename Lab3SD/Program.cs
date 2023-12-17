using Lab3SD.Context;
using Lab3SD.Mapping;
using Lab3SD.Models;
using Lab3SD.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetSection("ConnectionStrings")["Sql_DB"];

// Add services to the container.

builder.Services.AddScoped<IRepository<Customer>, Repository<Customer>>();
builder.Services.AddScoped<IRepository<Order>, Repository<Order>>();
builder.Services.AddScoped<IRepository<OrderItem>, Repository<OrderItem>>();
builder.Services.AddScoped<IRepository<UserAccount>, Repository<UserAccount>>();
builder.Services.AddScoped<IRepository<Product>, Repository<Product>>();
builder.Services.AddScoped<IRepository<UserRole>, Repository<UserRole>>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddDbContext<TablewareStoreDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo {
        Title = "WEB API",
        Version = "v1"
    });
});

builder.Services.AddCors(c => {
    c.AddPolicy("AllowOrigin", builder =>
    {
        builder
            .WithOrigins("*") // or specify your allowed origins
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("Content-Disposition")
            .SetIsOriginAllowed(origin => true); // SetIsOriginAllowed can be customized based on your needs
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowOrigin"); 

app.UseAuthorization();

app.MapControllers();

app.Run();