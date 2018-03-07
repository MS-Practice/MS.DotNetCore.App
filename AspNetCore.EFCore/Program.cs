using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCore.EFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            #region CreateData
            //using (var context = new MyContext())
            //{
            //    if (!context.Tags.Any())
            //    {
            //        context.Database.EnsureDeleted();
            //        context.Database.EnsureCreated();
            //        context.Tags.AddRange(new List<Tag>()
            //        {
            //            new Tag
            //            {
            //                TagId="1",
            //                TagName = "Tag1"
            //            },
            //            new Tag
            //            {
            //                TagId="2",
            //                TagName = "Tag2"
            //            },
            //            new Tag
            //            {
            //                TagId="3",
            //                TagName = "Tag3"
            //            }
            //        });
            //            context.SaveChanges();
            //    }


            //    context.Posts.Add(new Post
            //    {
            //        Content = "内容1",
            //        Title = "标题1",
            //        PostTags = new List<PostTag>()
            //        {
            //            new PostTag
            //            {
            //                TagId = "1"
            //            }
            //        }
            //    });
            //    context.SaveChanges();
            //} 
            #endregion

            using (var context = new MyContext())
            {
                var postTag = context.Posts.Include(p=>p.PostTags).First(p => p.PostId == 1);
                //context.Remove(postTag.PostTags.First());
                postTag.PostTags.RemoveRange(0, 3);
                postTag.PostTags.AddRange(new List<PostTag>()
                {
                    new PostTag()
                    {
                        TagId = "2"
                    },
                    new PostTag()
                    {
                        TagId = "3"
                    }
                });
                context.SaveChanges();
            }
        }
    }
}
