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
        if (context.ToDoItems.Any())
        {
            return;
        }

        // Seed initial data
        var tasks = new ToDoItem[]
        {
            new ToDoItem
            {
                Description = "Task 1",
                DueDate = DateTime.Now.AddDays(1)
            },
            new ToDoItem
            {
                Description = "Task 2",
                DueDate = DateTime.Now.AddDays(2)
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