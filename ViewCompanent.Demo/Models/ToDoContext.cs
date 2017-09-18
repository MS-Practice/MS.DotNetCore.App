using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewCompanent.Demo.Models
{
    public class ToDoContext:DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options)
                : base(options)
        {
        }
        public DbSet<TodoItem> ToDo { get; set; }
    }
}
