using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.EFCore
{
    class MyContext : DbContext
    {
        private static readonly ILoggerFactory EfCoreSqlLoggerFactory = LoggerFactory.Create(
            loggerBuilder => loggerBuilder.AddFilter((s, l) => l == LogLevel.Information)
            );
        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(EfCoreSqlLoggerFactory)
                .UseSqlServer(@"Data Source=84E2;Database=EfCore.ExampleDb;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration
            modelBuilder.Entity<PostTag>()
                .HasKey(t => new { t.PostId, t.TagId });

            modelBuilder.Entity<PostTag>()
                .HasOne(pt => pt.Post)
                .WithMany(p => p.PostTags)
                .HasForeignKey(pt => pt.PostId);

            modelBuilder.Entity<PostTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.PostTags)
                .HasForeignKey(pt => pt.TagId);
        }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public List<PostTag> PostTags { get; set; }
    }

    public class Tag
    {
        public string TagId { get; set; }
        public string TagName { get; set; }
        public List<PostTag> PostTags { get; set; }
    }

    public class PostTag
    {
        public int PostId { get; set; }
        public Post Post { get; set; }

        public string TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
