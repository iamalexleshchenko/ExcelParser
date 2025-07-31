using Microsoft.EntityFrameworkCore;
using ExcelParser.Entities;

namespace ExcelParser.Database;

public class DatabaseContext : DbContext
{
    public DbSet<PersonEntity> People { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        
    }
}