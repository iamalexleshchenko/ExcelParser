using AutoMapper;
using ExcelParser.Database;
using ExcelParser.Entities;
using ExcelParser.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace ExcelParser.Controllers;

[ApiController]
[Route("[controller]")]
public class ExcelController : ControllerBase
{
    private readonly ExcelParserService _excelParserService;
    private readonly DatabaseContext _databaseContext;
    private readonly IMapper _mapper;
    private readonly ILogger<ExcelController> _logger;

    public ExcelController(DatabaseContext databaseContext, ExcelParserService excelParserService, IMapper mapper, ILogger<ExcelController> logger)
    {
        _excelParserService = excelParserService;
        _databaseContext = databaseContext;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpPost("createPersonList")]
    public IActionResult ListPerson(IFormFile file)
    {
        using var stream = file.OpenReadStream();
        var list = _excelParserService.Parse(stream);
        return Ok(list);
    }
    
    [HttpPost("uploadPersonList")]
    public async Task<IActionResult> UploadList(IFormFile file)
    {
        try
        {
            _logger.LogInformation($"Загрузил файл {file.FileName}");
            await using var stream = file.OpenReadStream();
            var list = _excelParserService.Parse(stream);
           
            var entities = _mapper.Map<List<PersonEntity>>(list);
            
            await _databaseContext.People.AddRangeAsync(entities); 
            await _databaseContext.SaveChangesAsync();
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Ошибка при загрузке файла {file.FileName}");
            return BadRequest();
        }
    }
    
    [HttpGet("/getPersonExcel")]
    public async Task<FileResult> GetPersonExcel()
    {
        try
        {
            var personList = await _databaseContext.People
                .ToListAsync();
        
            var stream = new MemoryStream();
            using var package = new ExcelPackage(stream);
        
            var workSheet = package.Workbook.Worksheets.Add("Sheet1");  
            workSheet.Cells.LoadFromCollection(personList, true);
        
            await package.SaveAsync();
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "person.xlsx");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}