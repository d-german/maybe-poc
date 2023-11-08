using BlazorAppMaybePoc.Server.Data;
using BlazorAppMaybePoc.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppMaybePoc.Server;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        context.Database.Migrate(); // Ensure database is created and all migrations are applied

        // Look for any data, if data found, DB has been seeded
        if ((context.ToDoItems ?? throw new InvalidOperationException()).Any() && (context.Users ?? throw new InvalidOperationException()).Any())
        {
            return; // DB has already been seeded
        }

        // Seed initial user data
        var users = new[]
        {
            new User
            {
                UserName = "User 1",
                Email = "user1@example.com"
            },
            new User
            {
                UserName = "User 2",
                Email = "user2@example.com"
            },
            // ... other users ...
        };

        foreach (var user in users)
        {
            context.Users?.Add(user);
        }

        context.SaveChanges();

        // Seed initial task data with additional tasks for User 1 and User 2
        var random = new Random();
        var tasks = Enumerable.Range(1, 3).SelectMany(i => new[]
        {
            new ToDoItem
            {
                Description = $"Task for User 1 - {i}",
                DueDate = DateTime.Now.AddDays(random.Next(1, 30)),
                UserId = users[0].UserId,
                Priority = (Priority)random.Next(0, 3),
                Status = (Status)random.Next(0, 3)
            },
            new ToDoItem
            {
                Description = $"Task for User 2 - {i}",
                DueDate = DateTime.Now.AddDays(random.Next(1, 30)),
                UserId = users[1].UserId,
                Priority = (Priority)random.Next(0, 3),
                Status = (Status)random.Next(0, 3)
            }
        }).ToArray();

        foreach (var task in tasks)
        {
            context.ToDoItems.Add(task);
        }

        context.SaveChanges();
    }
}