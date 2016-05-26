using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace MBBS.Database
{
    public class UserQueries
    {
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MS_TableConnectionString"].ConnectionString);

        public int GetUserId(string email)
        {
            int userID = 0;
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT UserID FROM Users WHERE email = @email", con);
            cmd.Parameters.AddWithValue("@email", email);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                userID = reader.GetInt32(0);
            }
            con.Close();
            return userID;
        }

        public void CreateUser(string firstName, string lastName, string email, int userType)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Users(FirstName, LastName, Email, UserTypeID) VALUES (@firstName, @lastName, @email, " + userType + ")", con);
            //Must declare the scalar variable 
            cmd.Parameters.AddWithValue("@firstName", firstName);
            cmd.Parameters.AddWithValue("@lastName", lastName);
            cmd.Parameters.AddWithValue("@email", email);
            //cmd.Parameters.AddWithValue("@userTypeID", userType);

            SqlDataReader reader = cmd.ExecuteReader();
            con.Close();
        }

        public string GetPassword(int userID)
        {
            string retrievedPassword = null;
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT Password FROM LoginCode WHERE userID = @userID", con);
            cmd.Parameters.AddWithValue("@userID", userID);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                retrievedPassword = reader.GetString(0);
            }
            con.Close();
            return retrievedPassword;
        }

        public void SetPassword(int userID, string password)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO LoginCode(UserID, Password) VALUES(@userID, @password)", con);
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@password", password);
            SqlDataReader reader = cmd.ExecuteReader();
        }

        


    }
}
