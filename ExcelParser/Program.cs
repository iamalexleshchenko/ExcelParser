using ExcelParser.Services;
using ExcelParser.Database;
using ExcelParser.MappingProfiles;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

ExcelPackage.License.SetNonCommercialPersonal("Alexander Leshchenko");

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<ExcelParserService>();
builder.Services.AddDbContext<DatabaseContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));
builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
