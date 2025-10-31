using System.Reflection;
using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using ForoUniversitario.ApplicationLayer.Posts;
using ForoUniversitario.DomainLayer.Posts;
using ForoUniversitario.DomainLayer.Users;
using ForoUniversitario.DomainLayer.Groups;
using ForoUniversitario.DomainLayer.Factories;
using ForoUniversitario.DomainLayer.DomainServices;
using System.Collections.Generic;

namespace ForoUniversitario.Tests.Application
{
    public class PostServiceTests
    {
        private readonly Mock<IPostRepository> _mockPostRepository = new();
        private readonly Mock<IUserRepository> _mockUserRepository = new();
        private readonly Mock<IGroupRepository> _mockGroupRepository = new();
        private readonly Mock<IPostFactory> _mockPostFactory = new();
        private readonly Mock<IPostDomainService> _mockPostDomainService = new();

        [Fact]
        public async Task CreateAsync_ShouldReturnGuid_WhenPostIsCreated()
        {
            var command = new CreatePostCommand
            {
                Title = "Post de prueba",
                Content = "Contenido de prueba",
                AuthorId = Guid.NewGuid(),
                GroupId = Guid.NewGuid(),
                Type = TypePost.Discussion
            };

            var post = new Post(Guid.NewGuid(), command.Title, new PostContent(command.Content),
                command.AuthorId, command.GroupId, command.Type);

            _mockPostFactory.Setup(f => f.CreatePost(
                command.Title, command.Content, command.AuthorId, command.GroupId, command.Type))
                .Returns(post);

            _mockPostRepository.Setup(r => r.AddAsync(It.IsAny<Post>())).Returns(Task.CompletedTask);
            _mockPostRepository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var service = new PostService(
                _mockPostRepository.Object,
                _mockUserRepository.Object,
                _mockGroupRepository.Object,
                _mockPostFactory.Object,
                _mockPostDomainService.Object);

            var result = await service.CreateAsync(command);

            Assert.NotEqual(Guid.Empty, result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenPostDoesNotExist()
        {
            _mockPostRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Post?)null);

            var service = new PostService(
                _mockPostRepository.Object,
                _mockUserRepository.Object,
                _mockGroupRepository.Object,
                _mockPostFactory.Object,
                _mockPostDomainService.Object);

            var result = await service.GetByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnPostDto_WhenPostExists()
        {
            var authorId = Guid.NewGuid();
            var groupId = Guid.NewGuid();

            var post = new Post(Guid.NewGuid(), "Titulo", new PostContent("Contenido"), authorId, groupId, TypePost.Discussion);

            _mockPostRepository.Setup(r => r.GetByIdAsync(post.Id)).ReturnsAsync(post);

            var user = new User(
                Guid.NewGuid(),
                "Test User",
                "test@example.com",
                Role.Student,
                "hashedPassword123"
            );

            var adminUser = new User(Guid.NewGuid(), "Admin", "admin@mail.com", Role.Student, "hash");

            var group = (Group)Activator.CreateInstance(
                typeof(Group),
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                new object[] { Guid.NewGuid(), "Grupo", "Grupo de prueba", user },
                null
            )!;

            _mockUserRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new User(Guid.NewGuid(), "Autor", "autor@mail.com", Role.Student, "hash"));

            _mockGroupRepository
                .Setup(r => r.FindAsync(It.IsAny<Guid>()))
                .ReturnsAsync(group);

            var service = new PostService(
                _mockPostRepository.Object,
                _mockUserRepository.Object,
                _mockGroupRepository.Object,
                _mockPostFactory.Object,
                _mockPostDomainService.Object);

            var result = await service.GetByIdAsync(post.Id);

            Assert.NotNull(result);
            Assert.Equal("Titulo", result!.Title);
            Assert.Equal("Autor", result.AuthorName);
            Assert.Equal("Grupo", result.GroupName);
        }

        [Fact]
        public async Task GetPostsByUserAsync_ShouldReturnPostsOfUser()
        {
            var userId = Guid.NewGuid();
            var postList = new List<Post>
            {
                new Post(Guid.NewGuid(), "Post 1", new PostContent("Contenido 1"), userId, Guid.NewGuid(), TypePost.Discussion),
                new Post(Guid.NewGuid(), "Post 2", new PostContent("Contenido 2"), userId, Guid.NewGuid(), TypePost.News)
            };

            _mockPostRepository.Setup(r => r.GetPostsByUserAsync(userId)).ReturnsAsync(postList);

            var service = new PostService(
                _mockPostRepository.Object,
                _mockUserRepository.Object,
                _mockGroupRepository.Object,
                _mockPostFactory.Object,
                _mockPostDomainService.Object);

            var result = await service.GetPostsByUserAsync(userId);

            Assert.Equal(2, result.Count());
        }
    }
}

