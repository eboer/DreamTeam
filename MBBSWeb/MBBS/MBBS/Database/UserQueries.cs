﻿//===========================================================================================
//Project: MBBS
//Description:
//  Queries for user related calls. Function names are self explanatory.
//
//Date: 10-6-2016
//Author: Janine Lanting
//===========================================================================================
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

        public void AddDocentData(int docentID)
        {
            HttpContext context = HttpContext.Current;
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO DocentData(DocentID, DocentCode, Room) " +
               "VALUES (@docentID, @docentCode, @room)", con);
            cmd.Parameters.AddWithValue("@docentID", docentID);
            cmd.Parameters.AddWithValue("@docentCode", context.Request["docentCode"].Trim());
            cmd.Parameters.AddWithValue("@room", context.Request["roomNumber"].Trim());

            SqlDataReader reader = cmd.ExecuteReader();
            con.Close();
        }

        public int CreateUser()
        {
            HttpContext context = HttpContext.Current;
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Users(FirstName, LastName, Email, UserTypeID) " +
                 "OUTPUT inserted.UserID " +
                 "VALUES (@firstName, @lastName, @email, @userType)", con);

            cmd.Parameters.AddWithValue("@firstName", context.Request["firstName"].Trim());
            cmd.Parameters.AddWithValue("@lastName", context.Request["lastName"].Trim());
            cmd.Parameters.AddWithValue("@email", context.Request["email"].Trim());
            cmd.Parameters.AddWithValue("@userType", context.Request["userType"]);
 

            SqlDataReader reader = cmd.ExecuteReader();
            int userID = 0;
            while (reader.Read())
            {
                userID = reader.GetInt32(0);
            }
            con.Close();
            return userID;
        }

        //old function, still used in older version of registration
        //caused issues, hence the cheat used in query creation
        public int CreateUser(string firstName, string lastName, string email, int userType)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Users(FirstName, LastName, Email, UserTypeID) " +
                 "OUTPUT inserted.UserID " + 
                 "VALUES (@firstName, @lastName, @email, " + userType + ")", con);
            //Must declare the scalar variable 
            cmd.Parameters.AddWithValue("@firstName", firstName);
            cmd.Parameters.AddWithValue("@lastName", lastName);
            cmd.Parameters.AddWithValue("@email", email);
            //cmd.Parameters.AddWithValue("@userTypeID", userType);

            SqlDataReader reader = cmd.ExecuteReader();
            int userID = 0;
            while (reader.Read())
            {
                userID = reader.GetInt32(0);
            }
            con.Close();
            return userID;
        }

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

        public string UpdatePassword(int userID)
        {
            HttpContext context = HttpContext.Current;
            con.Open();
            SqlCommand cmd = new SqlCommand("Update LoginCode " +
                "SET Password = @newPassword " +
                "WHERE UserID = @userID " + 
                "AND Password = @oldPassword", con);
            cmd.Parameters.AddWithValue("@newPassword", context.Request["newPassword"].Trim());
            cmd.Parameters.AddWithValue("@oldPassword", context.Request["oldPassword"].Trim());
            cmd.Parameters.AddWithValue("@userID", userID);
            int result = cmd.ExecuteNonQuery();
            con.Close();
            if (result.Equals(0))
            {
                return "Old password was not correct";
            }
            if (result.Equals(1))
            {
                return "success";
            }
            return "Something has gone terribly wrong.";
        }
    }
}
