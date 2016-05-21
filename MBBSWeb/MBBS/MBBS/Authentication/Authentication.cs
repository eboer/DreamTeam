using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MBBS.Authentication
{
    public class Authenticate
    {
        public string generateToken()
        {
            Guid g = Guid.NewGuid();
            string token = Convert.ToBase64String(g.ToByteArray());
            token = token.Replace("=", "");
            token = token.Replace("+", "");
            return token;
        }

        public string setToken(int userID)
        {
            string token = generateToken();
            DateTime time = DateTime.Now.AddMinutes(30);
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MS_TableConnectionString"].ConnectionString);
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(
                    "IF EXISTS (SELECT NULL FROM Token WHERE UserID = @userID) " +
                    "UPDATE Token SET Token = @token, Expires = @expires " + 
                    "ELSE INSERT INTO Token(UserID, Token, Expires) VALUES (@userID, @token, @expires)", con);
                cmd.Parameters.AddWithValue("@userID", userID);
                cmd.Parameters.AddWithValue("@token", token);
                cmd.Parameters.AddWithValue("@expires", time);

                SqlDataReader reader = cmd.ExecuteReader();
                con.Close();
            }
            catch (Exception e)
            {
                con.Close();
                return null;
            }
            return token;
        }

        public string getToken(int userID)
        {
            string token;
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MS_TableConnectionString"].ConnectionString);
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT token, expires FROM Token WHERE UserID = @userID", con);
                cmd.Parameters.AddWithValue("@userID", userID);

                SqlDataReader reader = cmd.ExecuteReader();
                if(reader.HasRows)
                {
                    while (reader.Read())
                    {
                        token = reader.GetString(0);
                        DateTime time = reader.GetDateTime(1);
                        if (time < DateTime.Now)
                        {
                            deleteToken(userID);
                            return null;
                        }
                        con.Close();
                        return token;
                    }
                    return null;
                }
                else
                {
                    return null;
                }
                
                
            }
            catch (Exception e)
            {
                con.Close();
                return null;
            }
           
        }

        public void deleteToken(int userID)
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MS_TableConnectionString"].ConnectionString);
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Token WHERE UserID = @userID", con);
                cmd.Parameters.AddWithValue("@userID", userID);

                SqlDataReader reader = cmd.ExecuteReader();
                con.Close();
            }
            catch (Exception e)
            {
                con.Close();
            }
        }

        public Boolean confirmToken(int userID, string token)
        {
            //System.Web.HttpContext.Current.Request
            string userToken = getToken(userID);
            if(userToken == null)
            {
                return false;
            }
            if(userToken.Equals(token))
            {
                return true;
            }
            return false;
        }

    }
}