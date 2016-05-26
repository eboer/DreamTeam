using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using MBBS.Models;

namespace MBBS.Database
{
    public class ModuleQueries
    {
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MS_TableConnectionString"].ConnectionString);
        public List<Object> GetDocentModules(int userID)
        {
            con.Open();
            List<Object> modules = new List<Object>();
            SqlCommand cmd = new SqlCommand("SELECT DocentModule.ModuleID, Module.ModuleName "+
                "FROM DocentModule " +
                "INNER JOIN Module " +
                "ON DocentModule.ModuleID = Module.ModuleID " +
                "WHERE DocentModule.DocentID = @userID", con);
            cmd.Parameters.AddWithValue("@userID", userID);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                DocentModule docentModule = new DocentModule();
                docentModule.module_id = reader.GetString(0);
                docentModule.module_name = reader.GetString(1);
                modules.Add(docentModule);
            }
            con.Close();
            return modules;
        }
    }
}