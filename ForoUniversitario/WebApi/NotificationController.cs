using ForoUniversitario.ApplicationLayer.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace ForoUniversitario.WebApi;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetForUser(Guid userId)
    {
        var notifications = await _notificationService.GetForUserAsync(userId);
        return Ok(notifications);
    }

    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] SendNotificationCommand command)
    {
        await _notificationService.SendAsync(command.ReceiverId, command.Message, command.NotificationType);
        return Ok();
    }

    [HttpPost("{id}/markAsRead")]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        await _notificationService.MarkAsReadAsync(id);
        return NoContent();
    }
}
