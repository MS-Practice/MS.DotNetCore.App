﻿using Microsoft.EntityFrameworkCore;

namespace CustomModelBindingSample.Data
{
    public class AppDbContext:DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
