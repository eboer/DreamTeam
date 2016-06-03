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

        public List<Object> GetAllModules()
        {
            con.Open();
            List<Object> modules = new List<Object>();
            SqlCommand cmd = new SqlCommand("SELECT ModuleID, ModuleName " +
                "FROM Module", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Module module = new Module();
                module.module_id = reader.GetString(0);
                module.module_name = reader.GetString(1);
                modules.Add(module);
            }
            con.Close();
            return modules;
        }

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
                Module docentModule = new Module();
                docentModule.module_id = reader.GetString(0);
                docentModule.module_name = reader.GetString(1);
                modules.Add(docentModule);
            }
            con.Close();
            return modules;
        }

        public ModuleContent GetModuleData(string moduleID, int subsectionID, string languageID)
        {
            ModuleContent moduleContent = new ModuleContent();
            double version = GetSubsectionVersion(moduleID, subsectionID, languageID.ToUpper());

            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT AuthorID, Content " +
               "FROM ModuleContent " +
               "WHERE ModuleID = @moduleID AND SubsectionID = @subsectionID " +
               "AND LanguageID = @languageID AND VersionNumber = @versionNumber", con);
            cmd.Parameters.AddWithValue("@moduleID", moduleID.Trim());
            cmd.Parameters.AddWithValue("@subsectionID", subsectionID);
            cmd.Parameters.AddWithValue("@languageID", languageID.Trim().ToUpper());
            cmd.Parameters.AddWithValue("@versionNumber", version);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                moduleContent.AuthorID = reader.GetInt32(0);
                moduleContent.Content = reader.GetString(1);
                moduleContent.VersionNumber = version;
            }
            con.Close();
            return moduleContent;
        }

        public List<Subsection> GetSubsectionNames(string languageID)
        {
            List<Subsection> names = new List<Subsection>();

            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT SubsectionID, SubsectionCode, SectionID, SubsectionName, RequiredBool " + 
                "FROM Subsection", con);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Subsection subsection = new Subsection();
                 subsection.SubsectionID = reader.GetInt32(0);
                string subsectionCode = reader.GetDouble(1).ToString();
                string sectionID = reader.GetInt32(2).ToString();
                subsection.SubsectionCode = sectionID + "." +subsectionCode;
                subsection.SubsectionName = reader.GetString(3);
                Boolean requiredBool = true;
                if(reader.GetBoolean(4) != true)
                {
                    requiredBool = false;
                }
                subsection.RequiredBool = requiredBool;
                names.Add(subsection);
            }
            con.Close();
                return names;
        }

        public double GetSubsectionVersion(string moduleID, int subsectionID, string languageID)
        {
            SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MS_TableConnectionString"].ConnectionString);

            connection.Open();
            SqlCommand cmd = new SqlCommand("SELECT ModuleContent.VersionNumber FROM ModuleContent WHERE ModuleID = @moduleID AND SubsectionID = @subsectionID AND LanguageID = @languageID ORDER BY VersionNumber DESC", connection);
            cmd.Parameters.AddWithValue("@moduleID", moduleID.Trim());
            cmd.Parameters.AddWithValue("@subsectionID", subsectionID);
            cmd.Parameters.AddWithValue("@languageID", languageID.Trim().ToUpper());
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                double version = reader.GetDouble(0);
                connection.Close();
                return version;
            }
            connection.Close();
            return 0;
        }

        public void PostModuleData(int userID)
        {
            HttpContext context = HttpContext.Current;
            double version = GetSubsectionVersion(System.Web.HttpContext.Current.Request["moduleID"], Int32.Parse(System.Web.HttpContext.Current.Request["subsectionID"]), context.Request["languageID"]);
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO ModuleContent(ModuleID, SubsectionID, LanguageID, AuthorID, Content, VersionNumber) " +
                "VALUES (@moduleID, @subsectionID, @languageID, @userID, @content, @version)", con);
            cmd.Parameters.AddWithValue("@moduleID", context.Request["moduleID"].Trim());
            cmd.Parameters.AddWithValue("@subsectionID", context.Request["subsectionID"]);
            cmd.Parameters.AddWithValue("@languageID", context.Request["languageID"].Trim().ToUpper());
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@content", context.Request["content"]);
            cmd.Parameters.AddWithValue("@version", version++);
            SqlDataReader reader = cmd.ExecuteReader();
            con.Close();
        }
    }
}