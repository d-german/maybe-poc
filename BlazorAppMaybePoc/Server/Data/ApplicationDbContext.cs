using BlazorAppMaybePoc.Shared;
using FileContextCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppMaybePoc.Server.Data;

public class ApplicationDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseFileContextDatabase("json");
        base.OnConfiguring(optionsBuilder);
    }

    public DbSet<User>? Users { get; set; }
    public DbSet<ToDoItem>? ToDoItems { get; set; }
}