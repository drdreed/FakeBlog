using FakeBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeBlog.DAL
{
    interface IRepository
    {
        // List of methods to help deliver features
        // Create
        void AddPost(string title, string body, ApplicationUser owner);

        // Read
        Post GetPost(int postId);
        List<Post> GetPostsFromUser(string userId);

        // Update
        bool EditPost(int postId, string title, string body); // true: successful, false: not successful
        bool PublishDraft(int postId, bool status);

        // Delete
        bool RemovePost(int postId);
    }
}
