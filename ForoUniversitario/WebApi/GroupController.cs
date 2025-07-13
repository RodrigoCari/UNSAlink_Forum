using ForoUniversitario.ApplicationLayer.Groups;
using Microsoft.AspNetCore.Mvc;

namespace ForoUniversitario.WebApi;

[ApiController]
[Route("api/[controller]")]
public class GroupController : ControllerBase
{
    private readonly IGroupService _groupService;

    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    [HttpPost]
    public async Task<ActionResult<GroupDto>> CreateAsync([FromBody] CreateGroupCommand command)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var id = await _groupService.CreateAsync(command);
        var dto = await _groupService.GetByIdAsync(id);
        return CreatedAtAction(nameof(GetByIdAsync), new { id = dto.Id }, dto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GroupDto>> GetByIdAsync(Guid id)
    {
        var group = await _groupService.GetByIdAsync(id);
        if (group == null)
            return NotFound();
        return Ok(group);
    }

    [HttpPost("{groupId}/join")]
    public async Task<IActionResult> JoinAsync(Guid groupId, [FromQuery] Guid userId)
    {
        await _groupService.JoinAsync(groupId, userId);
        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<GroupDto>>> SearchAsync(
        [FromQuery] string name,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var results = await _groupService.SearchAsync(name, page, pageSize);
        return Ok(results);
    }
}
