using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FakeBlog.Models;

namespace FakeBlog.DAL
{
    public class FakeBlogRepository : IRepository
    {
        public FakeBlogContext Context { get; set; }

        public FakeBlogRepository()
        {
            Context = new FakeBlogContext();
        }

        public FakeBlogRepository(FakeBlogContext context)
        {
            Context = context;
        }

        public void AddPost(string title, string body, ApplicationUser owner)
        {
            Post post = new Post
            {
                Title = title,
                Owner = owner,
                Body = body,
                IsDraft = true,
            };


            Context.Posts.Add(post);
            Context.SaveChanges();
        }

        public bool EditPost(int postId, string title, string body)
        {
            Post foundPost = GetPost(postId);
            if (foundPost != null)
            {
                foundPost.Title = title;
                foundPost.Body = body;
                Context.SaveChanges();
                return true;
            }
            return false;
        }

        public Post GetPost(int postId)
        {
            return Context.Posts.FirstOrDefault(p => p.PostId == postId);
        }

        public List<Post> GetPostsFromUser(string userId)
        {
            return Context.Posts.Where(p => p.Owner.Id == userId).ToList();
        }

        public bool PublishDraft(int postId, bool status)
        {
            Post foundPost = GetPost(postId);
            if (foundPost != null)
            {
                foundPost.IsDraft = status;
                Context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool RemovePost(int postId)
        {
            Post found_post = GetPost(postId);
            if (found_post != null)
            {
                Context.Posts.Remove(found_post);
                Context.SaveChanges();
                return true;
            }
            return false;

        }
    }
}