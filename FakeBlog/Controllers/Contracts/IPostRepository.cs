using FakeBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeBlog.Controllers.Contracts
{
    interface IPostRepository
    {
        // List of methods to help deliver features
        // Create
        void AddPost(string title, string body, ApplicationUser owner);

        // Read
        Post GetPost(int postId);
        List<Post> GetPostsFromUser(string userId);

        // Update
        void EditPostTitle(int postId, string title); 
        void EditPostBody(int postId, string body);
        void PublishDraft(int postId);

        // Delete
        bool RemovePost(int postId);
    }
}
