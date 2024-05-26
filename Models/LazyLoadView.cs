using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Discoursify.Models
{
    public class LazyLoadView
    {
        public IEnumerable<Post> Posts { get; set; }

        private static string CS = ConfigurationManager.ConnectionStrings["Discoursify"].ConnectionString;

        public IEnumerable<Post> GetPostsFromDatabase(string filter)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("admPost", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CMD", "GetPost");
                    cmd.Parameters.AddWithValue("@Content",filter);
                    cmd.Parameters.AddWithValue("@Title", filter);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Post> posts = new List<Post>();

                        while (reader.Read())
                        {
                            Post post = new Post
                            {
                                Title = (string)reader["Title"],
                                ImageUrl = (string)reader["Img_Name"],
                                Owner = (string)reader["Owner"],
                                UniqKey = (string)reader["Uniquekey"],
                                Like = (int)reader["Up_Vote"],
                                DisLike = (int)reader["Down_Vote"],
                                Content = (string)reader["Content"],
                                CommentCount = (int)reader["CMTCOUNT"]
                            };

                            posts.Add(post);
                        }

                        return posts;
                    }
                }
            }
        }

    }
}