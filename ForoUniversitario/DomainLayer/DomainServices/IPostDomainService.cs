using ForoUniversitario.DomainLayer.Posts;

namespace ForoUniversitario.DomainLayer.DomainServices
{
    public interface IPostDomainService
    {
        Task<bool> CanSharePostToGroupAsync(Post post, Guid targetGroupId);
    }
}
