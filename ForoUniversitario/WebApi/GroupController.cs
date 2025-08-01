﻿using ForoUniversitario.ApplicationLayer.Groups;
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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateGroupCommand command)
    {
        var id = await _groupService.CreateAsync(command);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var group = await _groupService.GetByIdAsync(id);
        if (group == null) return NotFound();
        return Ok(group);
    }

    [HttpPost("{groupId}/join")]
    public async Task<IActionResult> Join(Guid groupId, [FromQuery] Guid userId)
    {
        await _groupService.JoinAsync(groupId, userId);
        return Ok();
    }

    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] string name)
    {
        var groups = await _groupService.SearchAsync(name);
        var dtos = groups.Select(g => new GroupDto 
        { 
            Id = g.Id, 
            Name = g.Name,
            Description = g.Description,
            AdminId = g.AdminId
        });
        return Ok(dtos);
    }

    [HttpGet("with-latest-posts")]
    public async Task<IActionResult> GetAllWithLatestPosts()
    {
        var groups = await _groupService.GetAllWithLatestPostAsync();
        return Ok(groups);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetGroupsByUser(Guid userId)
    {
        var groups = await _groupService.GetGroupsByUserAsync(userId);
        return Ok(groups);
    }
}
