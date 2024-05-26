using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Discoursify.Enum;
using System.Security.Cryptography;
using System.Text;
using System.Web.Helpers;
using System.Net;

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
        public string TelegramId { get; set; }  
        public string OtpCode { get; set; }

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
            RegisterUser(signUp.Gender, signUp.Username, signUp.DateOfBirth, signUp.Email, signUp.Password, signUp.TelegramId);

            return "Success";
        }

        public static void RegisterUser(string gender, string username, DateTime dob, string email, string password, string teleId)
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
                        cmd.Parameters.AddWithValue("@pUsrName", username);
                        cmd.Parameters.AddWithValue("@pUsrGen", gender);
                        cmd.Parameters.AddWithValue("@pTeleId", teleId);
                        cmd.ExecuteNonQuery();
                    }
                }
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
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

        public string getOTPUser(string username)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT DBO.DecryptData(Otp_Code) AS 'OTP', B.TelegramId AS 'TeleId', B.Username AS 'Name' FROM User_Data A INNER JOIN User_Detail B ON A.Username = B.Username WHERE B.Email = @Username", con))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader["OTP"].ToString() + "," + reader["TeleId"].ToString() + "," + reader["Name"].ToString();
                            }
                            else
                            {
                                Console.WriteLine("User not found");
                                return "Not Found";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
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

        public string sendOtp(string otp)
        {
            string result = "Fail";
            string[] part = otp.Split(',');
            string otpCode = part[0];
            string sendTo = part[1];
            string user = part[2];

            if(sendTo != null && sendTo != "")
            {
                string message = "Dear " + user + ",\n\nWe received a request to reset your password for your Discoursify account. " +
                            "To proceed with resetting your password, please use the following One-Time Password (OTP):" +
                            "\n\n<b>Your OTP: " + " <tg-spoiler>" + otpCode + "</tg-spoiler></b>\n\nThis OTP " +
                            "is valid for 10 minutes. Please enter this code on the Discoursify password reset page to " +
                            "continue with the process.\n\nIf you did not request a password reset, please ignore this message " +
                            "or contact our support team for assistance.\n\nThank you for using Discoursify!\n\n" +
                            "Best regards,\nThe Discoursify Team";

                sendMessgae(message, sendTo);

                result = "Success";

            }
            return result;

        }

        public void sendMessgae(string message, string user_id)
        {
            string botToken = "7053350901:AAHNiUUc4R8dUZvdpxbiufe1A7MBsNkmjGk";
            string url = "https://api.telegram.org/bot" + botToken + "/sendMessage";

            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                string parameters = "chat_id=" + user_id + "&text=" + message + "&parse_mode=HTML";
                //  +"&parse_mode=MarkdownV2";
                byte[] responseBytes = client.UploadData(url, "POST", Encoding.UTF8.GetBytes(parameters));

            }
        }

        public string compareOtp(string otp, string email)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT DBO.DecryptData(OTP_CODE) AS RESULT FROM User_Data A INNER JOIN User_Detail B ON A.Username = B.Username WHERE B.Email = @Email", con))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if(reader["RESULT"].ToString() == otp)
                                {
                                    return reader["RESULT"].ToString();
                                }
                                return reader["RESULT"].ToString();
                            }
                            else
                            {
                                Console.WriteLine("User not found");
                                return "Fail";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Fail";
            }
        }

        public string changePassword(string password, string email) 
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
                        cmd.Parameters.AddWithValue("@CMD", "ChangePw");
                        cmd.Parameters.AddWithValue("@pUsrEmail", email);
                        cmd.Parameters.AddWithValue("@pUsrPass", hashedPassword);
                        cmd.ExecuteNonQuery();
                    }
                }
                return "Success";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Fail";
            }

        }

    }
}