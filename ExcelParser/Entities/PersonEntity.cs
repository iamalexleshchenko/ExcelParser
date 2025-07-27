namespace ExcelParser.Entities;

public class PersonEntity : WithId
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
}