using ExcelParser.Services;
using ExcelParser.Database;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

ExcelPackage.License.SetNonCommercialPersonal("Alexander Leshchenko");

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<ExcelParserService>();
builder.Services.AddDbContext<DatabaseContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));;

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
