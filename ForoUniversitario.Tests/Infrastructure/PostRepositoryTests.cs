using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using ForoUniversitario.DomainLayer.Posts;
using ForoUniversitario.DomainLayer.Users;
using ForoUniversitario.DomainLayer.Groups;
using ForoUniversitario.InfrastructureLayer.Persistence;

namespace ForoUniversitario.Tests.Infrastructure
{
    public class PostRepositoryTests
    {
        private DbContextOptions<ForumDbContext> CreateNewContextOptions()
        {
            // Crear una nueva base de datos en memoria para cada test
            return new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task AddAsync_ShouldAddPostToDatabase()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new ForumDbContext(options);
            var repository = new PostRepository(context);

            var author = new User(Guid.NewGuid(), "Autor", "autor@mail.com", Role.Student, "hash");
            var group = (Group)Activator.CreateInstance(
                typeof(Group),
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null,
                new object[] { Guid.NewGuid(), "Grupo de prueba", "Descripción del grupo", author },
                null
            )!;

            var post = new Post(
                Guid.NewGuid(),
                "Título del Post",
                new PostContent("Contenido de prueba"),
                author.Id,
                group.Id,
                TypePost.Discussion
            );

            // Act
            await repository.AddAsync(post);
            await context.SaveChangesAsync();

            // Assert
            var savedPost = await context.Posts.FirstOrDefaultAsync(p => p.Id == post.Id);
            Assert.NotNull(savedPost);
            Assert.Equal("Título del Post", savedPost!.Title);
            Assert.Equal(author.Id, savedPost.AuthorId);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnPost_WhenExists()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new ForumDbContext(options);
            var repository = new PostRepository(context);

            var author = new User(Guid.NewGuid(), "Autor", "autor@mail.com", Role.Student, "hash");
            var group = (Group)Activator.CreateInstance(
                typeof(Group),
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null,
                new object[] { Guid.NewGuid(), "Grupo Test", "Descripción", author },
                null
            )!;

            var post = new Post(Guid.NewGuid(), "Post Existente", new PostContent("Contenido"), author.Id, group.Id, TypePost.News);
            await context.Posts.AddAsync(post);
            await context.SaveChangesAsync();

            // Act
            var foundPost = await repository.GetByIdAsync(post.Id);

            // Assert
            Assert.NotNull(foundPost);
            Assert.Equal(post.Title, foundPost!.Title);
            Assert.Equal(post.Id, foundPost.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenPostDoesNotExist()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new ForumDbContext(options);
            var repository = new PostRepository(context);

            // Act
            var result = await repository.GetByIdAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }
    }
}

