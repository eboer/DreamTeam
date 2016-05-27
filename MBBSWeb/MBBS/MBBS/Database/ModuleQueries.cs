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

        public double GetSubsectionVersion()
        {
            string moduleID = System.Web.HttpContext.Current.Request["moduleID"];
            int subsectionID = Int32.Parse(System.Web.HttpContext.Current.Request["subsectionID"]);
            double currentVersion = 0;
            SqlConnection conVersion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MS_TableConnectionString"].ConnectionString);
            conVersion.Open();
            SqlCommand cmd = new SqlCommand("SELECT ModuleContent.VersionNumber " +
                "FROM ModuleContent " +
                "WHERE ModuleID = @moduleID AND SubsectionID = @subsectionID", conVersion);
            cmd.Parameters.AddWithValue("@moduleID", moduleID);
            cmd.Parameters.AddWithValue("@subsectionID", subsectionID);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                double version = reader.GetDouble(0);
                if(version > currentVersion)
                {
                    currentVersion = version;
                }
            }
            conVersion.Close();
            
            return currentVersion++;
        }

        public void PostModuleData(int userID)
        {
            HttpContext context = HttpContext.Current;

            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO ModuleContent(ModuleID, SubsectionID, LanguageID, AuthorID, Content, VersionNumber) " +
                "VALUES (@moduleID, @subsectionID, @languageID, @userID, @content, @version)", con);
            cmd.Parameters.AddWithValue("@moduleID", context.Request["moduleID"]);
            cmd.Parameters.AddWithValue("@subsectionID", context.Request["subsectionID"]);
            cmd.Parameters.AddWithValue("@languageID", context.Request["languageID"]);
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@content", context.Request["content"]);
            cmd.Parameters.AddWithValue("@version", GetSubsectionVersion());
            SqlDataReader reader = cmd.ExecuteReader();



        }
    }
}