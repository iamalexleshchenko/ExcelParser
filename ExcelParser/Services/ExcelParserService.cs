using ExcelParser.DTOs;
using OfficeOpenXml;

namespace ExcelParser.Services;

public class ExcelParserService
{
    private readonly ILogger<ExcelParserService> _logger;
    public ExcelParserService(ILogger<ExcelParserService> logger)
    {
        _logger = logger;
    }

    public List<Person> Parse(Stream stream)
    {
        _logger.LogInformation("Начало парсинга Excel-файла.");
        using var package = new ExcelPackage(stream);

        var sheet = package.Workbook.Worksheets[0];
        int rowCount = sheet.Dimension.Rows + 2;
        
        _logger.LogInformation($"Файл содержит {rowCount} строк");
        
        var resultList = new List<Person>();

        for (var row = 4; row <= rowCount; row++)
        {
            var person = new Person();
            person.FirstName = sheet.Cells[row, 2].Text;
            person.LastName = sheet.Cells[row, 3].Text;
            person.MiddleName = sheet.Cells[row, 4].Text;

            resultList.Add(person);
            _logger.LogInformation($"Добавлен объект {person.FirstName} {person.LastName}");
        }
        
        _logger.LogInformation($"Парсинг завершен. Обнаружено {resultList.Count} объектов.");
        
        return resultList;
    }
}
