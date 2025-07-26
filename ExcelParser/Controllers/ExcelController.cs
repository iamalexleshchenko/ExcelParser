using ExcelParser.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExcelParser.Controllers;

[ApiController]
[Route("[controller]")]

public class ExcelController : ControllerBase
{
    private readonly ExcelParserService _excelParserService;

    public ExcelController (ExcelParserService excelParserService)
    {
        _excelParserService = excelParserService;
    }

    [HttpPost("createPersonList")]
    public IActionResult ListPerson(IFormFile file)
    {
        using var stream = file.OpenReadStream();
        var list = _excelParserService.Parse(stream);
        return Ok(list);
    }
}