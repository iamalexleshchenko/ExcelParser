using ExcelParser.DTOs;
using OfficeOpenXml;

namespace ExcelParser.Services;

public class ExcelParserService
{
    public List<Person> Parse(Stream stream)
    {
        using var package = new ExcelPackage(stream);

        var sheet = package.Workbook.Worksheets[0];
        int rowCount = sheet.Dimension.Rows + 2;

        var resultList = new List<Person>();

        for (var row = 4; row <= rowCount; row++)
        {
            var person = new Person();
            person.FirstName = sheet.Cells[row, 2].Text;
            person.LastName = sheet.Cells[row, 3].Text;
            person.MiddleName = sheet.Cells[row, 4].Text;

            resultList.Add(person);
        }
        return resultList;
    }
}