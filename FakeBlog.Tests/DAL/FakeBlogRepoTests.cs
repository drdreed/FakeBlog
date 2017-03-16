using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FakeBlog.DAL;
using FakeBlog.Models;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;

namespace FakeBlog.Tests.DAL
{
    [TestClass]
    public class FakeBlogRepoTests
    {
        public Mock<FakeBlogContext> fakeContext { get; set; }
        public FakeBlogRepository repo { get; set; }
        public Mock<DbSet<Post>> mockPostsSet { get; set; }
        public IQueryable<Post> queryPosts { get; set; }
        public List<Post> fakePostTable { get; set; }
        public ApplicationUser dustin { get; set; }
        public ApplicationUser kelly { get; set; }

        [TestInitialize]
        public void Setup()
        {
            fakePostTable = new List<Post>();
            fakeContext = new Mock<FakeBlogContext>();
            mockPostsSet = new Mock<DbSet<Post>>();
            repo = new FakeBlogRepository(fakeContext.Object);

            kelly = new ApplicationUser { Id = "kelly-id-1" };
            dustin = new ApplicationUser { Id = "dustin-id-1" };
        }

        public void CreateFakeDatabase()
        {

            var queryPosts = fakePostTable.AsQueryable();

            mockPostsSet.As<IQueryable<Post>>().Setup(p => p.Provider).Returns(queryPosts.Provider);
            mockPostsSet.As<IQueryable<Post>>().Setup(p => p.Expression).Returns(queryPosts.Expression);
            mockPostsSet.As<IQueryable<Post>>().Setup(p => p.ElementType).Returns(queryPosts.ElementType);
            mockPostsSet.As<IQueryable<Post>>().Setup(p => p.GetEnumerator()).Returns(queryPosts.GetEnumerator());
        
            mockPostsSet.Setup(b => b.Add(It.IsAny<Post>())).Callback((Post post) => fakePostTable.Add(post));
            mockPostsSet.Setup(b => b.Remove(It.IsAny<Post>())).Callback((Post post) => fakePostTable.Remove(post));
            fakeContext.Setup(c => c.Posts).Returns(mockPostsSet.Object);
        }

        [TestMethod]
        public void EnsureICanCreateInstanceofRepo()
        {
            FakeBlogRepository repo = new FakeBlogRepository();

            Assert.IsNotNull(repo);
        }


        [TestMethod]
        public void EnsureICanInjectContestInstance()
        {
            FakeBlogContext context = new FakeBlogContext();

            FakeBlogRepository repo = new FakeBlogRepository(context);

            Assert.IsNotNull(repo.Context);
        }

        [TestMethod]
        public void EnsureICanHaveNotNullContext()
        {
            FakeBlogRepository repo = new FakeBlogRepository();

            Assert.IsNotNull(repo.Context);
        }

        [TestMethod]
        public void EnsureICanAddPost()
        {
            // Arrange
            CreateFakeDatabase();

            ApplicationUser aUser = new ApplicationUser
            {
                Id = "my-user-id",
                UserName = "Sammy",
                Email = "sammy@gmail.com"
            };

            // Act
            repo.AddPost("My Post", "This is my body", aUser);

            // Assert
            Assert.AreEqual(repo.Context.Posts.Count(), 1);
        }

        [TestMethod]
        public void EnsureICanReturnPosts()
        {
            // Arrange
            fakePostTable.Add(new Post { Title = "My Post" });
            CreateFakeDatabase();

            // Act

            int expectedPostCount = 1;
            int actualPostCount = repo.Context.Posts.Count();

            // Assert
            Assert.AreEqual(expectedPostCount, actualPostCount);
        }

        [TestMethod]
        public void EnsureICanFindAPost()
        {
            fakePostTable.Add(new Post { PostId = 1, Title = "My Post" });
            CreateFakeDatabase();

            // Act

            string expectedPostTitle = "My Post";
            Post actualPost = repo.GetPost(1);
            string actualPostTitle = actualPost.Title;

            // Assert
            Assert.AreEqual(expectedPostTitle, actualPostTitle);

        }

        [TestMethod]
        public void EnsureICanFindAndEditAPost()
        {
            fakePostTable.Add(new Post { PostId = 1, Title = "My Post", Body = "asdf" });
            CreateFakeDatabase();

            // Act
            bool expectResult = true;
            bool actualResult = repo.EditPost(1, "My New Title", "Some New Body");

            Assert.AreEqual(expectResult, actualResult);

            string expectedPostTitle = "My New Title";
            Post actualPost = repo.GetPost(1);
            string actualPostTitle = actualPost.Title;

            // Assert
            Assert.AreEqual(expectedPostTitle, actualPostTitle);

        }

        [TestMethod]
        public void EnsureICanRemoveAPost()
        {
            // Arrange
            fakePostTable.Add(new Post { PostId = 1, Title = "My Post", Owner = kelly });
            fakePostTable.Add(new Post { PostId = 2, Title = "My Post", Owner = kelly });
            fakePostTable.Add(new Post { PostId = 3, Title = "My Post", Owner = dustin });
            CreateFakeDatabase();
            
                        // Act
            int expected_post_count = 2;
            repo.RemovePost(3);
            int actual_post_count = repo.Context.Posts.Count();
            
                        // Assert
            Assert.AreEqual(expected_post_count, actual_post_count);

        }

        [TestMethod]
        public void EnsureICanGetPostsByUser()
        {
            // Arrange
            fakePostTable.Add(new Post { PostId = 1, Title = "My Post", Owner = kelly });
            fakePostTable.Add(new Post { PostId = 2, Title = "My Post", Owner = kelly });
            fakePostTable.Add(new Post { PostId = 3, Title = "My Post", Owner = dustin });
            CreateFakeDatabase();

            // Act

            int expectedPostCount = 2;
            int actualPostCount = repo.GetPostsFromUser(kelly.Id).Count;

            Assert.AreEqual(expectedPostCount, actualPostCount);
        }
    }
}
