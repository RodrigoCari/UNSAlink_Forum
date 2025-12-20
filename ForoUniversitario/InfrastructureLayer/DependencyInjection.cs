using ForoUniversitario.ApplicationLayer.Groups;
using ForoUniversitario.ApplicationLayer.Notifications;
using ForoUniversitario.ApplicationLayer.Posts;
using ForoUniversitario.ApplicationLayer.Users;
using ForoUniversitario.DomainLayer.DomainServices;
using ForoUniversitario.DomainLayer.Factories;
using ForoUniversitario.DomainLayer.Groups;
using ForoUniversitario.DomainLayer.Notifications;
using ForoUniversitario.DomainLayer.Posts;
using ForoUniversitario.DomainLayer.Users;
using ForoUniversitario.InfrastructureLayer.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace ForoUniversitario.InfrastructureLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityModule(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }

    public static IServiceCollection AddCommunityModule(this IServiceCollection services)
    {
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<IGroupService, GroupService>();
        services.AddScoped<IGroupFactory, GroupFactory>();
        services.AddScoped<IGroupDomainService, GroupDomainService>();
        return services;
    }

    public static IServiceCollection AddContentModule(this IServiceCollection services)
    {
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<IPostFactory, PostFactory>();
        services.AddScoped<IPostDomainService, PostDomainService>();
        
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<ICommentService, CommentService>();
        return services;
    }

    public static IServiceCollection AddNotificationModule(this IServiceCollection services)
    {
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<INotificationService, NotificationService>();
        return services;
    }
}
