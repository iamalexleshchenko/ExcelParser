using Microsoft.AspNetCore.Mvc;
using ExcelParser.DTOs;
using ExcelParser.Exceptions;
using ExcelParser.Services;

namespace ExcelParser.Controllers;

[ApiController]
[Controller]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly AuthService _authService;
    
    public AuthController(ILogger<AuthController> logger, AuthService authService)
    {
        _logger = logger;
        _authService = authService;
    }

    [HttpPost("/register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        try
        {
            _logger.LogInformation("Регистрация пользователя.");

            var createdUser = await _authService.Register(user);

            _logger.LogInformation("Зарегистрирован пользователь с ID: {Id}", createdUser.Id);

            return CreatedAtAction(nameof(Register), new { id = createdUser.Id }, createdUser);
        }
        catch (UserAlreadyExistsException e)
        {
            return Conflict(e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Неизвестная ошибка на сервере.");
            return StatusCode(500);
        }
    }
}