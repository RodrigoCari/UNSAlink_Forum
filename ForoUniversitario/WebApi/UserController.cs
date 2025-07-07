using ForoUniversitario.ApplicationLayer.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForoUniversitario.WebApi;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var id = await _userService.RegisterAsync(command);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProfile(Guid id, [FromBody] UpdateUserProfileCommand command)
    {
        await _userService.UpdateProfileAsync(id, command);
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [Authorize]
    [HttpGet("{id}/works")]
    public async Task<IActionResult> GetWorks(Guid id)
    {
        var works = await _userService.GetWorksAsync(id);
        return Ok(works);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        try
        {
            var token = await _userService.LoginAsync(command);
            return Ok(new { Token = token });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Invalid credentials");
        }
    }
}
