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
        
    public PeopleController(DatabaseContext database)
    {
        _databaseContext = database;
    }

    [HttpGet("/getPersonList")]
    public async Task<IActionResult> GetPersonList()
    {
        try
        {
            var personList = await _databaseContext.People
                .ToListAsync();
            return Ok(personList);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpGet("/getPersonListWithPagination")]
    public async Task<IActionResult> GetPersonListWithPagination(int? limit, int? offset)
    {
        try
        {
            var listPersonEntity = await _databaseContext.People
                .ToListAsync(limit, offset);
            
            return Ok(listPersonEntity);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}