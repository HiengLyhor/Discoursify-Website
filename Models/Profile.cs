﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Discoursify.Models
{
    public class Profile
    {
        public string Username { set; get; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public List<ProfileAction> ProfileActions { get; set; }

        private static string CS = ConfigurationManager.ConnectionStrings["Discoursify"].ConnectionString;

        public Profile profileData(string username)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {

                try
                {
                    SqlCommand cmd = new SqlCommand("admProfile", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CMD", "getProfile");
                    cmd.Parameters.AddWithValue("@Username", username);

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Profile result = new Profile()
                            {

                                Username = reader["Username"].ToString(),
                                Email = reader["Email"].ToString(),
                                Bio = reader["Bio"].ToString(),

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

        public List<ProfileAction> getDataByAction(string action, string username) 
        {
            List<ProfileAction> result = new List<ProfileAction>();
            using (SqlConnection con = new SqlConnection(CS))   
            {

                try
                {
                    SqlCommand cmd = new SqlCommand("admProfile", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CMD", action);
                    cmd.Parameters.AddWithValue("@Username", username);

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProfileAction data = new ProfileAction()
                            {
                                UserAction = action,
                                Title = reader["Title"].ToString(),
                                Content = reader["Content"].ToString(),
                                PostKey = reader["PostKey"].ToString(),
                                ActionDate = reader["ActionDate"].ToString(),
                                Username = reader["Username"].ToString(),

                            };
                            result.Add(data);
                        }
                        
                    }
                    con.Close(); 

                    return result;
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