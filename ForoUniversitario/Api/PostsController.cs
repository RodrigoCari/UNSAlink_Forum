using ForoUniversitario.Application.Posts;
using Microsoft.AspNetCore.Mvc;

namespace ForoUniversitario.Api;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;

    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpPost]
    public async Task<IActionResult> CrearPost([FromBody] CreatePostCommand command)
    {
        var id = await _postService.CrearPostAsync(command);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var post = await _postService.ObtenerPostPorIdAsync(id);

        if (post == null)
            return NotFound();

        return Ok(post);
    }
}
