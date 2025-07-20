using ForoUniversitario.ApplicationLayer.Posts;
using Microsoft.AspNetCore.Mvc;

namespace ForoUniversitario.WebApi;

[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly ICommentService _commentService;

    public PostController(IPostService postService, ICommentService commentService)
    {
        _postService = postService;
        _commentService = commentService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreatePostCommand comand)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var id = await _postService.CreateAsync(comand);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }
        catch (System.Exception ex)
        {
            return Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PostDto>> GetById(Guid id)
    {
        var post = await _postService.GetByIdAsync(id);
        return post is null
            ? NotFound()
            : Ok(post);
    }

    [HttpPost("{postId}/share/{groupId}")]
    public async Task<IActionResult> ShareToGroup(Guid postId, Guid groupId)
    {
        await _postService.ShareToGroupAsync(postId, groupId);
        return NoContent();
    }

    [HttpPost("{postId}/comment")]
    public async Task<IActionResult> Comment(Guid postId, [FromBody] AddCommentCommand command)
    {
        command.PostId = postId;
        await _commentService.AddCommentAsync(command);
        return Ok();
    }

    [HttpGet("type/{type}")]
    public async Task<IActionResult> GetByType(int type)
    {
        var list = await _postService.GetByTypeAsync(type);
        return Ok(list);
    }

    [HttpPost("{postId}/request-ideas")]
    public async Task<IActionResult> RequestIdeas(Guid postId)
    {
        await _postService.RequestIdeasAsync(postId);
        return NoContent();
    }
}
