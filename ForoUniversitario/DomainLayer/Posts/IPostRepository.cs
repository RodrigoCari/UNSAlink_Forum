﻿namespace ForoUniversitario.DomainLayer.Posts;

public interface IPostRepository
{
    Task<Post?> GetByIdAsync(Guid id);
    Task AddAsync(Post post);
    Task SaveChangesAsync();
    Task<IEnumerable<Post>> GetByTypeAsync(TypePost type);
    Task<IEnumerable<Post>> GetPostsByUserAsync(Guid userId);
    Task<IEnumerable<Post>> GetByGroupAsync(Guid groupId);
}
