using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FakeBlog.Models;
using FakeBlog.Controllers.Contracts;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace FakeBlog.DAL
{
    public class PostRepository : IPostRepository
    {
        IDbConnection _blogConnection;

        public PostRepository(IDbConnection blogConnection)
        {
            _blogConnection = blogConnection;
        }

        public void AddPost(string title, string body, ApplicationUser owner)
        {
            _blogConnection.Open();

            try
            {
                var addPostCommand = _blogConnection.CreateCommand();
                addPostCommand.CommandText = "Insert into Posts(Title,Body,Owner_Id) values(@title,@body,@ownerId)";
                var titleParameter = new SqlParameter("title", SqlDbType.VarChar);
                titleParameter.Value = title;
                addPostCommand.Parameters.Add(titleParameter);
                var bodyParameter = new SqlParameter("body", SqlDbType.VarChar);
                bodyParameter.Value = body;
                addPostCommand.Parameters.Add(bodyParameter);
                var ownerParameter = new SqlParameter("ownerId", SqlDbType.Int);
                ownerParameter.Value = owner.Id;
                addPostCommand.Parameters.Add(ownerParameter);

                addPostCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            finally
            {
                _blogConnection.Close();
            }
        }

        public void EditPostTitle(int postId, string newtitle)
        {
            _blogConnection.Open();

            try
            {
                var updatePostCommand = _blogConnection.CreateCommand();
                updatePostCommand.CommandText = @"
                    Update Posts 
                    Set Title = @NewTitle
                    Where postid = @postId";
                var nameParameter = new SqlParameter("NewTitle", SqlDbType.VarChar);
                nameParameter.Value = newtitle;
                updatePostCommand.Parameters.Add(nameParameter);
                var postIdParameter = new SqlParameter("postId", SqlDbType.Int);
                postIdParameter.Value = postId;
                updatePostCommand.Parameters.Add(postIdParameter);

                updatePostCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            finally
            {
                _blogConnection.Close();
            }
        }

        public void EditPostBody(int postId, string newbody)
        {
            _blogConnection.Open();

            try
            {
                var updatePostCommand = _blogConnection.CreateCommand();
                updatePostCommand.CommandText = @"
                    Update Posts 
                    Set Title = @NewBody
                    Where postid = @postId";
                var nameParameter = new SqlParameter("NewBody", SqlDbType.VarChar);
                nameParameter.Value = newbody;
                updatePostCommand.Parameters.Add(nameParameter);
                var postIdParameter = new SqlParameter("postId", SqlDbType.Int);
                postIdParameter.Value = postId;
                updatePostCommand.Parameters.Add(postIdParameter);

                updatePostCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            finally
            {
                _blogConnection.Close();
            }
        }

        public Post GetPost(int postId)
        {
            _blogConnection.Open();

            try
            {
                var getPostCommand = _blogConnection.CreateCommand();
                getPostCommand.CommandText = @"
                    SELECT postId, Name, Url, Owner_Id 
                    FROM Posts 
                    WHERE PostId = @postId";
                var postIdParam = new SqlParameter("postId", SqlDbType.Int);
                postIdParam.Value = postId;
                getPostCommand.Parameters.Add(postIdParam);

                var reader = getPostCommand.ExecuteReader();

                if (reader.Read())
                {
                    var post = new Post
                    {
                        PostId = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Body = reader.GetString(2),
                        Owner = new ApplicationUser { Id = reader.GetString(3) }
                    };
                    return post;
                }
            }
            catch (Exception ex) { }
            finally
            {
                _blogConnection.Close();
            }

            return null;
        }

        public List<Post> GetPostsFromUser(string userId)
        {
            _blogConnection.Open();

            try
            {
                var getPostCommand = _blogConnection.CreateCommand();
                getPostCommand.CommandText = @"
                    SELECT postId, Name, Url, Owner_Id 
                    FROM Posts 
                    WHERE Owner_Id = @userId";
                var postIdParam = new SqlParameter("userId", SqlDbType.VarChar);
                postIdParam.Value = userId;
                getPostCommand.Parameters.Add(postIdParam);

                var reader = getPostCommand.ExecuteReader();

                var posts = new List<Post>();
                while (reader.Read())
                {
                    var post = new Post
                    {
                        PostId = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Body = reader.GetString(2),
                        Owner = new ApplicationUser { Id = reader.GetString(3) }
                    };

                    posts.Add(post);
                }

                return posts;
            }
            catch (Exception ex) { }
            finally
            {
                _blogConnection.Close();
            }

            return new List<Post>();
        }

        public void PublishDraft(int postId)
        {
            _blogConnection.Open();

            try
            {
                var updatePostCommand = _blogConnection.CreateCommand();
                updatePostCommand.CommandText = @"
                    Update Posts 
                    Set IsDraft = @status
                    Where postid = @postId";
                var nameParameter = new SqlParameter("status", SqlDbType.Bit);
                nameParameter.Value = true;
                updatePostCommand.Parameters.Add(nameParameter);
                var postIdParameter = new SqlParameter("postId", SqlDbType.Int);
                postIdParameter.Value = postId;
                updatePostCommand.Parameters.Add(postIdParameter);

                updatePostCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            finally
            {
                _blogConnection.Close();
            }
        }

        public bool RemovePost(int postId)
        {
            _blogConnection.Open();

            try
            {
                var removePostCommand = _blogConnection.CreateCommand();
                removePostCommand.CommandText = @"
                    Delete 
                    From Posts
                    Where PostId = @postId";

                var postIdParameter = new SqlParameter("postId", SqlDbType.Int);
                postIdParameter.Value = postId;
                removePostCommand.Parameters.Add(postIdParameter);

                removePostCommand.ExecuteNonQuery();

                return true;
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            finally
            {
                _blogConnection.Close();
            }

            return false;

        }
    }
}