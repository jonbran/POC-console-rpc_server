using Microsoft.EntityFrameworkCore;

namespace console_service.Models
{
    public class TodoContext : DbContext
    {
        /* public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        } */

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Server=JONS-DESKTOP;Database=TodoItems;Trusted_Connection=True;");

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}