using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Discoursify.Enum;
using System.Security.Cryptography;
using System.Text;
using System.Web.Helpers;

namespace Discoursify.Models
{
    public class Login
    {
        public string Username { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string UserRole { get; set; }
        public string ConfirmPassword { get; set; }

        private static string CS = ConfigurationManager.ConnectionStrings["Discoursify"].ConnectionString;

        public string loginAction(string email, string password)
        {
            try
            {
                bool status = AuthenticateUser(email, password);

                if (!status)
                {
                    return "Failed";
                }

                return "Success";

            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Failed during the processing.";
            }
        }

        public string Register(Login signUp)
        {
            if(signUp == null)
            {
                return "Failed";
            }
            RegisterUser(signUp.Gender, signUp.Username, signUp.DateOfBirth, signUp.Email, signUp.Password);

            return "Success";
        }

        public static void RegisterUser(string gender, string username, DateTime dob, string email, string password)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("admUser_Login", con))
                    {
                        byte[] hashedPassword = HashPassword(password);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CMD", "Register");
                        cmd.Parameters.AddWithValue("@pUsrEmail", email);
                        cmd.Parameters.AddWithValue("@pUsrPass", hashedPassword);
                        cmd.Parameters.AddWithValue("@pUsrRole", "User");
                        cmd.Parameters.AddWithValue("@pUsrBio", "N/A");
                        //cmd.Parameters.AddWithValue("@pDob", dob);
                        cmd.Parameters.AddWithValue("@pUsrName", username);
                        cmd.Parameters.AddWithValue("@pUsrGen", gender);
                        cmd.ExecuteNonQuery();
                    }
                }
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
        }
        public string CheckUsername(string username)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT [Username] FROM User_Data WHERE Username = @Username", con))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return "Existed";
                            }
                            else
                            {
                                Console.WriteLine("User not found");
                                return "Not Found";
                            }
                        }
                    }
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Existed";
            }
        }
        private static bool AuthenticateUser(string email, string password)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT [Password] FROM User_Data WHERE Username = (SELECT Username FROM User_Detail WHERE Email = @Email)", con))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                byte[] storedPassword = (byte[])reader["Password"];
                                byte[] inputPassword = HashPassword(password);

                                return CompareByteArrays(storedPassword, inputPassword);
                            }
                            else
                            {
                                Console.WriteLine("User not found");
                                return false;
                            }
                        }
                    }
                }
            } catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private static bool CompareByteArrays(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
                return false;

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                    return false;
            }

            return true;
        }

        public static byte[] HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                return sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        public Login getUserLogin(string email)
        {
            Login userInfo = new Login();
           
            using(SqlConnection con = new SqlConnection(CS))
            {

                try
                {
                    SqlCommand cmd = new SqlCommand("admUser_Login", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CMD", "UsrInfo");
                    cmd.Parameters.AddWithValue("@pUsrEmail", email);

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userInfo = new Login()
                            {
                                Username = reader["User Name"].ToString(),
                                Email = reader["User Email"].ToString(),
                                UserRole = reader["User Role"].ToString()
                            };
                        }
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }

            return userInfo;
        }

    }
}