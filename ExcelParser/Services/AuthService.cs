using Microsoft.EntityFrameworkCore;

using ExcelParser.Database;
using ExcelParser.DTOs;
using ExcelParser.Entities;
using ExcelParser.Exceptions;

namespace ExcelParser.Services;

public class AuthService
{
    private readonly DatabaseContext _database;
    private readonly ILogger<AuthService> _logger;

    public AuthService(DatabaseContext database, ILogger<AuthService> logger)
    {
        _database = database;
        _logger = logger;
    }

    public async Task<UserEntity> Register(User user)
    {
        var isUserExists = await _database.Users.AnyAsync(q => q.Login == user.Login || q.Email == user.Email);

        if (isUserExists)
        {
            throw new UserAlreadyExistsException("Пользователь уже существует");
        }
        
        var createdUser = new UserEntity 
        { 
            Login = user.Login,
            Email = user.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password)
        };
                
        _database.Users.Add(createdUser);
        await _database.SaveChangesAsync();
            
        return createdUser;
    }
}