using ExcelParser.Database;
using ExcelParser.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExcelParser.Controllers;

[ApiController]
[Route("[controller]")]
public class PeopleController : ControllerBase
{
    private readonly DatabaseContext _databaseContext;
    private readonly ILogger<PeopleController> _logger;
        
    public PeopleController(DatabaseContext database, ILogger<PeopleController> logger)
    {
        _databaseContext = database;
        _logger = logger;
    }

    [HttpGet("/getPersonList")]
    public async Task<IActionResult> GetPersonList()
    {
        try
        {
            _logger.LogInformation("Начало выполнения GET-запроса /getPersonList.");
            
            var personList = await _databaseContext.People
                .ToListAsync();
            
            _logger.LogInformation("Получен List<PersonEntity>, содержащий {Count} записей.", personList.Count);
            
            return Ok(personList);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка получения List<PersonEntity> из базы данных.");
            throw;
        }
    }

    [HttpGet("/getPersonListWithPagination")]
    public async Task<IActionResult> GetPersonListWithPagination(int? limit, int? offset)
    {
        try
        {
            _logger.LogInformation("Начало выполнения GET-запроса /getPersonListWithPagination.");
            
            var listPersonEntity = await _databaseContext.People
                .ToListAsync(limit, offset);
            
            _logger.LogInformation("Получен List<PersonEntity> ({Count} записей). Параметры пагинации: Limit = {Limit}, Offset = {Offset}.", listPersonEntity.Count, limit, offset);
            
            return Ok(listPersonEntity);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка получения List<PersonEntity> из базы данных.");
            throw;
        }
    }
}