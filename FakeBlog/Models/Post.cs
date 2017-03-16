using System;
using System.ComponentModel.DataAnnotations;

namespace FakeBlog.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        [Required]
        [MinLength(3)]
        public string Title { get; set; }

        public string Body { get; set; }
        public bool IsDraft { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime PublishedAt { get; set; }
        public string URL { get; set; }

        public ApplicationUser Owner { get; set; }

    }
}