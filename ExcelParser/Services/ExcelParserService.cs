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
        try
        {
            _logger.LogInformation("Начало парсинга Excel-файла.");
            using var package = new ExcelPackage(stream);

            var sheet = package.Workbook.Worksheets[0];
            int rowCount = sheet.Dimension.Rows + 2;

            _logger.LogInformation("Файл содержит {RowCount} строк", rowCount);

            var resultList = new List<Person>();

            for (var row = 4; row <= rowCount; row++)
            {
                var person = new Person
                {
                    FirstName = sheet.Cells[row, 2].Text,
                    LastName = sheet.Cells[row, 3].Text,
                    MiddleName = sheet.Cells[row, 4].Text
                };

                resultList.Add(person);
                _logger.LogInformation("В результирующий список добавлен объект: {FirstName} {LastName}", person.FirstName, person.LastName);
            }

            _logger.LogInformation("Парсинг завершен. Обнаружено {Count} объектов.", resultList.Count);

            return resultList;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка парсинга Excel-файла.");
            throw;
        }
    }
}