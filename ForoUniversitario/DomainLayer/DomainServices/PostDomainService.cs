using ForoUniversitario.DomainLayer.Posts;

namespace ForoUniversitario.DomainLayer.DomainServices
{
    public class PostDomainService : IPostDomainService
    {
        public Task<bool> CanSharePostToGroupAsync(Post post, Guid targetGroupId)
        {
            // Ejemplo simple: no permitir compartir en mismo grupo
            bool canShare = post.GroupId != targetGroupId;
            return Task.FromResult(canShare);
        }
    }
}
