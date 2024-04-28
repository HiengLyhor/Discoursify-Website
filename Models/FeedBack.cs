using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Discoursify.Models
{
    public class FeedBack
    {
        public string Owner { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public DateTime FeedbackDate { get; set; }
        public int Rate { get; set; }
        public List<FeedBack> feedBacks { get; set; }

        private static string CS = ConfigurationManager.ConnectionStrings["Discoursify"].ConnectionString;

        public List<FeedBack> GetFeedback()
        {
            List<FeedBack> feedbacks = new List<FeedBack>();

            using (SqlConnection con = new SqlConnection(CS))
            {

                try
                {
                    SqlCommand cmd = new SqlCommand("admFeedback", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CMD", "Get");

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FeedBack result = new FeedBack()
                            {

                                // Apply logic get comment for each post of Unique Key
                                Owner = reader["Owner"].ToString(),
                                Content = reader["Content"].ToString(),
                                Title = reader["Title"].ToString(),
                                FeedbackDate = (DateTime)reader["Date"],
                                Rate = (int)reader["Star"],

                            };
                            feedbacks.Add(result);
                        }
                    }
                    con.Close();

                    return feedbacks;
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

        public bool CreateFeedBack(string title, string content, string owner, int rate)
        {
            try
            {
                if (owner == null)
                {
                    return false;
                }

                using (SqlConnection con = new SqlConnection(CS))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("admFeedback", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CMD", "Create");
                        cmd.Parameters.AddWithValue("@OWNER", owner);
                        cmd.Parameters.AddWithValue("@CONTENT", content);
                        cmd.Parameters.AddWithValue("@TITLE", title);
                        cmd.Parameters.AddWithValue("@RATE", rate);
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
    }

}