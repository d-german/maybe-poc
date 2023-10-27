using BlazorAppMaybePoc.Server.Data;
using BlazorAppMaybePoc.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppMaybePoc.Server;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        context.Database.Migrate(); // Ensure database is created and all migrations are applied

        // Look for any data, if data found, DB has been seeded
        if (context.ToDoItems.Any() && context.Users.Any())
        {
            return;
        }

        // Seed initial user data
        var users = new User[]
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

        foreach (User user in users)
        {
            context.Users.Add(user);
        }

        context.SaveChanges();

        // Seed initial task data
        var tasks = new ToDoItem[]
        {
            new ToDoItem
            {
                Description = "Task 1",
                DueDate = DateTime.Now.AddDays(1),
                UserId = users[0].UserId // Assign user to task
            },
            new ToDoItem
            {
                Description = "Task 2",
                DueDate = DateTime.Now.AddDays(2),
                UserId = users[1].UserId // Assign user to task
            },
            // ... other tasks ...
        };

        foreach (ToDoItem task in tasks)
        {
            context.ToDoItems.Add(task);
        }

        context.SaveChanges();
    }
}