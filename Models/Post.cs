using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Web;
using System.Configuration;

namespace Discoursify.Models
{
    public class Post
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public string Owner { get; set; }
        public string UniqKey { get; set; } 
        public int Like { get; set; }    

        public int DisLike { get; set; }
        public int CommentCount { get; set; }   
        public string PostDate { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }

        public List<PostComment> PostComments { get; set; }

        private static string CS = ConfigurationManager.ConnectionStrings["Discoursify"].ConnectionString;

        public string EditPost(Post model)
        {
            try
            {

                if (model == null)
                {
                    return "Failed to create post, invalid content.";
                }

                if (model.ImageFile != null && model.ImageFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(model.ImageFile.FileName);

                    Random random = new Random();
                    string randomNumberString = random.Next().ToString();
                    fileName = randomNumberString + "_" + fileName;

                    var path = Path.Combine("D:\\0-ITE-Year 4\\PP\\Project\\Discoursify\\Image", fileName);
                    model.ImageFile.SaveAs(path);

                    // Save the filename to the model property
                    model.ImageUrl = fileName;
                }
                else
                {
                    model.ImageUrl = null;
                }

                using (SqlConnection con = new SqlConnection(CS))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("admPost", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CMD", "Edit");
                        cmd.Parameters.AddWithValue("@Owner", model.Owner);
                        cmd.Parameters.AddWithValue("@PostKey", model.UniqKey);
                        cmd.Parameters.AddWithValue("@Title", model.Title);
                        cmd.Parameters.AddWithValue("@Content", model.Content);
                        cmd.Parameters.AddWithValue("@ImgName", model.ImageUrl);
                        cmd.ExecuteNonQuery();
                    }
                }
                return "success";

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CreatePost(Post model)
        {
            try
            {

                if (model == null)
                {
                    return "Failed to create post, invalid content.";
                }

                if (model.ImageFile != null && model.ImageFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(model.ImageFile.FileName);

                    Random random = new Random();
                    string randomNumberString = random.Next().ToString();
                    fileName = randomNumberString + "_" + fileName;

                    var path = Path.Combine("D:\\0-ITE-Year 4\\PP\\Project\\Discoursify\\Image", fileName);
                    model.ImageFile.SaveAs(path);

                    // Save the filename to the model property
                    model.ImageUrl = fileName;
                }
                else
                {
                    model.ImageUrl = null;
                }

                using (SqlConnection con = new SqlConnection(CS))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("admPost", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CMD", "Create");
                        cmd.Parameters.AddWithValue("@Owner", model.Owner);
                        cmd.Parameters.AddWithValue("@Title", model.Title);
                        cmd.Parameters.AddWithValue("@Content", model.Content);
                        cmd.Parameters.AddWithValue("@ImgName", model.ImageUrl);
                        cmd.ExecuteNonQuery();
                    }
                }
                return "success";

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }


        public bool Vote(string method, string owner, string postKey)
        {
            try
            {
                if (owner == null)
                {
                    return false;
                }

                if(method == "DOWN")
                {
                    method = "DownVote";
                }
                else if(method == "UP")
                {
                    method = "UpVote";
                }
                else if(method == "Downcmt")
                {
                    method = "DownCmt";
                }
                else if(method == "UPcmt")
                {
                    method = "UpCmt";
                }

                using (SqlConnection con = new SqlConnection(CS))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("admPost", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CMD", method);
                        cmd.Parameters.AddWithValue("@OWNER", owner);
                        cmd.Parameters.AddWithValue("@PostKey", postKey);
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public Post PostDetail(string postKey)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {

                try
                {
                    SqlCommand cmd = new SqlCommand("admPost", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CMD", "PostDetail");
                    cmd.Parameters.AddWithValue("@PostKey", postKey);

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Post result = new Post()
                            { 

                                Title = reader["Title"].ToString(),
                                Content = reader["Content"].ToString(), 
                                ImageUrl = reader["ImageUrl"].ToString(),
                                Owner = reader["Owner"].ToString(),
                                PostDate = reader["Post_Date"].ToString(),
                                UniqKey = (string)reader["Uniquekey"],
                                Like = (int)reader["Up_Vote"],
                                DisLike = (int)reader["Down_Vote"],
                                CommentCount = (int)reader["CmtCount"],

                            };
                            return result;
                        }
                    }
                    con.Close();
                    return null;
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