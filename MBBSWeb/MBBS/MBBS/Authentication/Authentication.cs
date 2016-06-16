//===========================================================================================
//Project: MBBS
//Description:
//   
//
//Date: 10-6-2016
//Author: Janine Lanting
//===========================================================================================

using MBBS.Models;
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
            //DateTime time = DateTime.Now.AddSeconds(30);
            DateTime time = DateTime.Now.AddMinutes(30);
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MS_TableConnectionString"].ConnectionString);
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(
                    "IF EXISTS (SELECT NULL FROM Token WHERE UserID = @userID) " +
                    "UPDATE Token SET Token = @token, Expires = @expires WHERE UserID = @userID " + 
                    "ELSE INSERT INTO Token(UserID, Token, Expires) VALUES (@userID, @token, @expires)", con);
                cmd.Parameters.AddWithValue("@userID", userID);
                cmd.Parameters.AddWithValue("@token", token);
                cmd.Parameters.AddWithValue("@expires", time);

                SqlDataReader reader = cmd.ExecuteReader();
                con.Close();
            }
            catch (Exception)
            {
                con.Close();
                return null;
            }
            return token;
        }

        private AuthenticatedUser getUserID(string token)
        {
            AuthenticatedUser user = new AuthenticatedUser();
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MS_TableConnectionString"].ConnectionString);
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT Token.UserID, Expires, UserTypeID FROM Token " +
                    "INNER JOIN Users " +
                    "On Token.UserID = Users.UserID " +
                    "WHERE Token = @token", con);
                cmd.Parameters.AddWithValue("@token", token);

                SqlDataReader reader = cmd.ExecuteReader();
                if(reader.HasRows)
                {
                    while (reader.Read())
                    {
                        user.UserID = reader.GetInt32(0);
                        DateTime time = reader.GetDateTime(1);
                        if (time < DateTime.Now)
                        {
                            deleteToken(user.UserID);
                            user.UserID = 0;
                            return user;
                        }
                        user.UserTypeID = reader.GetInt32(2);
                        con.Close();
                        return user;
                    }
                    return user;
                }
                else
                {
                    return user;
                }
                
                
            }
            catch (Exception)
            {
                con.Close();
                return user;
            }
           
        }

        private void deleteToken(int userID)
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
            catch (Exception)
            {
                con.Close();
            }
        }

        public AuthenticatedUser confirmToken()
        {
            string token = System.Web.HttpContext.Current.Request.Headers["Authorization"];
            return getUserID(token);
        }

    }
}