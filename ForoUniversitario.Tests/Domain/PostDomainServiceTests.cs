using Xunit;
using ForoUniversitario.DomainLayer.Posts;
using System;

namespace ForoUniversitario.Tests.Domain
{
    public class PostTests
    {
        [Fact]
        public void CreatePost_ShouldInitializeCorrectly()
        {
            // === Inicializar ===
            var authorId = Guid.NewGuid();
            var groupId = Guid.NewGuid();

            // === Ejecutar ===
            var post = new Post(Guid.NewGuid(), "Titulo", new PostContent("Contenido"), authorId, groupId, TypePost.Discussion);

            // === Verificar ===
            Assert.Equal("Titulo", post.Title);
            Assert.Equal("Contenido", post.Content.Text);
            Assert.Equal(TypePost.Discussion, post.Type);
            Assert.Equal(authorId, post.AuthorId);
            Assert.Equal(groupId, post.GroupId);
        }
    }
}

