using Microsoft.EntityFrameworkCore;
using ProductCrudApiV2.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services
 builder.Services.AddControllers();
 builder.Services.AddEndpointsApiExplorer();
 builder.Services.AddSwaggerGen();

 // PostgreSQL connection
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();