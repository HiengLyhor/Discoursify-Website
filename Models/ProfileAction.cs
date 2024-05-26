using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Discoursify.Models
{
    public class ProfileAction
    {
        public string UserAction { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ActionDate { get; set; }  
        public string PostKey { get; set; }
        public string Username { get; set; }
        public string ImgUrl { get; set; }
        public string CmtKey { get; set; }
        private static string CS = ConfigurationManager.ConnectionStrings["Discoursify"].ConnectionString;

        public void deletePost(string postKey, string owner)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("admPost", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CMD", "DelPost");
                        cmd.Parameters.AddWithValue("@Owner", owner);
                        cmd.Parameters.AddWithValue("@PostKey", postKey);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }

        }

        public void deleteComment(string comment, string owner) 
        {

            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("admPost", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CMD", "DelCmt");
                        cmd.Parameters.AddWithValue("@Owner", owner);
                        cmd.Parameters.AddWithValue("@PostKey", comment);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        public void unlike(string postkey, string owner)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("admPost", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CMD", "UnLike");
                        cmd.Parameters.AddWithValue("@Owner", owner);
                        cmd.Parameters.AddWithValue("@PostKey", postkey);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }


        }
    }

}