using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Discoursify.Models
{
    public class PostComment
    {

        public string PostKey { get; set; }
        public string UniqueKey { get; set; }
        public string Description { get; set; }
        public string DownVote { get; set; }
        public string UpVote { get; set; }
        public string CommentDate { get; set; }
        public string CmtOwner { get; set; }

        private static string CS = ConfigurationManager.ConnectionStrings["Discoursify"].ConnectionString;

        public List<PostComment> GetPostComments(string postKey)
        {
            List<PostComment> postComment = new List<PostComment>();

            using (SqlConnection con = new SqlConnection(CS))
            {

                try
                {
                    SqlCommand cmd = new SqlCommand("admComment", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CMD", "GetCmt");
                    cmd.Parameters.AddWithValue("@pPOSTKEY", postKey);

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PostComment result = new PostComment()
                            {

                                // Apply logic get comment for each post of Unique Key
                                CmtOwner = reader["Owner"].ToString(),
                                Description = reader["Description"].ToString(),
                                CommentDate = reader["CreateDate"].ToString(),
                                UniqueKey = reader["CmtKey"].ToString(),
                                DownVote = reader["DownVote"].ToString(),
                                UpVote = reader["UpVote"].ToString(),

                            };
                            postComment.Add(result);
                        }
                    }
                    con.Close();
                    return postComment;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
                finally
                {
                    con.Close();
                }
            }
        }
    }
}