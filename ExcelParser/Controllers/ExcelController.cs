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
        try
        {
            _logger.LogInformation("Передан {FileName} файл.", file.FileName);
            
            using var stream = file.OpenReadStream();
            var list = _excelParserService.Parse(stream);
            
            _logger.LogInformation("Сформирован json-файл, содержащий в себе {Count} записей.", list.Count);
            
            return Ok(list);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка загрузки файла {FileName}.", file.FileName);
            throw;
        }
    }
    
    [HttpPost("uploadPersonList")]
    public async Task<IActionResult> UploadList(IFormFile file)
    {
        try
        {
            _logger.LogInformation("Загрузил файл {FileName}",  file.FileName);
            
            await using var stream = file.OpenReadStream();
            var list = _excelParserService.Parse(stream);
           
            var entities = _mapper.Map<List<PersonEntity>>(list);
            
            await _databaseContext.People.AddRangeAsync(entities); 
            await _databaseContext.SaveChangesAsync();
            
            _logger.LogInformation("{Count} объектов сохранены в базу данных", entities.Count);
            
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка загрузки файла {FileName}.", file.FileName);
            return BadRequest();
        }
    }
    
    [HttpGet("/getPersonExcel")]
    public async Task<FileResult> GetPersonExcel()
    {
        try
        {
            _logger.LogInformation("Начало выполнения GET-запроса /getPersonExcel.");
            
            var personList = await _databaseContext.People
                .ToListAsync();
        
            _logger.LogInformation("Получен List<PersonEntity>, содержащий {Count} объектов.", personList.Count);
            
            var stream = new MemoryStream();
            using var package = new ExcelPackage(stream);
        
            var workSheet = package.Workbook.Worksheets.Add("Sheet1");  
            workSheet.Cells.LoadFromCollection(personList, true);
        
            await package.SaveAsync();
            stream.Position = 0;
            
            _logger.LogInformation("Сформирован поток Excel-файла. Размер потока: {Length} байт.", stream.Length);
            
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "person.xlsx");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка создания Excel-файла.");
            throw;
        }
    }
}