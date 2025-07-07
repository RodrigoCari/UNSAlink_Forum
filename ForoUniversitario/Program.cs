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
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowViteDev", policy =>
    {
        policy
         .WithOrigins("http://localhost:5173")  // la URL de tu Vue dev
         .AllowAnyHeader()
         .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

//builder.Services.AddDbContext<ForumDbContext>(options =>
//    options.UseInMemoryDatabase("ForumDb"));

builder.Services.AddDbContext<ForumDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IPostService, PostService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IGroupService, GroupService>();

builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();

builder.Services.AddScoped<IGroupFactory, GroupFactory>();
builder.Services.AddScoped<IGroupDomainService, GroupDomainService>();

builder.Services.AddScoped<IPostFactory, PostFactory>();
builder.Services.AddScoped<IPostDomainService, PostDomainService>();

var app = builder.Build();

app.UseCors("AllowViteDev");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI();
    app.UseSwagger();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
