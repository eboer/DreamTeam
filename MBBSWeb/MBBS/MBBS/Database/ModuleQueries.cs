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

        public ModuleContentModel GetModuleData(string moduleID, int subsectionID, string languageID)
        {
            ModuleContentModel moduleContent = new ModuleContentModel();
            double version = GetSubsectionVersion(moduleID, subsectionID, languageID);

            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT AuthorID, Content " +
               "FROM ModuleContent " +
               "WHERE ModuleID = @moduleID AND SubsectionID = @subsectionID " +
               "AND LanguageID = @languageID AND VersionNumber = @versionNumber", con);
            cmd.Parameters.AddWithValue("@moduleID", moduleID);
            cmd.Parameters.AddWithValue("@subsectionID", subsectionID);
            cmd.Parameters.AddWithValue("@languageID", languageID);
            cmd.Parameters.AddWithValue("@versionNumber", version);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                moduleContent.AuthorID = reader.GetInt32(0);
                moduleContent.Content = reader.GetString(1);
                moduleContent.VersionNumber = version;
            }
            return moduleContent;
        }

        public List<SubsectionModel> GetSubsectionNames(string languageID)
        {
            List<SubsectionModel> names = new List<SubsectionModel>();

            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT SubsectionID, SubsectionCode, SectionID, SubsectionName, RequiredBool " + 
                "FROM Subsection", con);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                SubsectionModel subsection = new SubsectionModel();
                 subsection.SubsectionID = reader.GetInt32(0);
                string subsectionCode = reader.GetDouble(1).ToString();
                string sectionID = reader.GetInt32(2).ToString();
                subsection.SubsectionCode = sectionID + "." +subsectionCode;
                subsection.SubsectionName = reader.GetString(3);
                Boolean requiredBool = true;
                if(reader.GetByte(4) != 1)
                {
                    requiredBool = false;
                }
                subsection.RequiredBool = requiredBool;
                names.Add(subsection);
            }
                return names;
        }

        public double GetSubsectionVersion(string moduleID, int subsectionID, string languageID)
        {
            
            double currentVersion = 0;
            SqlConnection conVersion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MS_TableConnectionString"].ConnectionString);
            conVersion.Open();
            SqlCommand cmd = new SqlCommand("SELECT ModuleContent.VersionNumber " +
                "FROM ModuleContent " +
                "WHERE ModuleID = @moduleID AND SubsectionID = @subsectionID " +
                "AND LanguageID = @languageID", conVersion);
            cmd.Parameters.AddWithValue("@moduleID", moduleID);
            cmd.Parameters.AddWithValue("@subsectionID", subsectionID);
            cmd.Parameters.AddWithValue("@languageID", languageID);
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
            
            return currentVersion;
        }

        public void PostModuleData(int userID)
        {
            HttpContext context = HttpContext.Current;
            double version = GetSubsectionVersion(System.Web.HttpContext.Current.Request["moduleID"], Int32.Parse(System.Web.HttpContext.Current.Request["subsectionID"]), context.Request["languageID"]);
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO ModuleContent(ModuleID, SubsectionID, LanguageID, AuthorID, Content, VersionNumber) " +
                "VALUES (@moduleID, @subsectionID, @languageID, @userID, @content, @version)", con);
            cmd.Parameters.AddWithValue("@moduleID", context.Request["moduleID"]);
            cmd.Parameters.AddWithValue("@subsectionID", context.Request["subsectionID"]);
            cmd.Parameters.AddWithValue("@languageID", context.Request["languageID"]);
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@content", context.Request["content"]);
            cmd.Parameters.AddWithValue("@version", version++);
            SqlDataReader reader = cmd.ExecuteReader();



        }
    }
}