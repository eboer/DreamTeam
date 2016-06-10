﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using MBBS.Models;

namespace MBBS.Database
{
    public class ModuleQueries
    {
        TranslationQueries translationQuery = new TranslationQueries();
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

        public double GetCompetencyVersion(string moduleID, string coordinate, string languageID)
        {
            SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MS_TableConnectionString"].ConnectionString);

            connection.Open();
            SqlCommand cmd = new SqlCommand("SELECT Competency.VersionNumber FROM ModuleContent WHERE ModuleID = @moduleID AND Coordinate = @coordinate AND LanguageID = @languageID ORDER BY VersionNumber DESC", connection);
            cmd.Parameters.AddWithValue("@moduleID", moduleID.Trim());
            cmd.Parameters.AddWithValue("@coordinate", coordinate.Trim());
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

        public MatrixData GetMatrixData(string moduleID, string languageID)
        {
            languageID = languageID.ToUpper().Trim();
            MatrixData matrix = GetMatrixHeaders(moduleID, languageID);
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT Coordinate, CompetencyDescription, CompetencyLevel, AuthorID " +
                                "FROM Competency " +
                                "WHERE ModuleID = @moduleID " +
                                "AND LanguageID = @languageID " +
                                "ORDER BY VersionNumber DESC", con);
            cmd.Parameters.AddWithValue("@moduleID", moduleID.Trim());
            cmd.Parameters.AddWithValue("languageID", languageID);
            SqlDataReader reader = cmd.ExecuteReader();
            Dictionary<string, Competency> dictCompetencies = new Dictionary<string, Competency>();
            while (reader.Read())
            {
                string coordinate = reader.GetString(0);
               if(!dictCompetencies.ContainsKey(coordinate))
                {
                    Competency competency = new Competency();
                    competency.Coordinate = coordinate;
                    competency.CompetencyDescription = reader.GetString(1);
                    competency.CompetencyLevel = reader.GetInt32(2);
                    dictCompetencies.Add(coordinate, competency);
                }
            }
            List<Competency> competencies = new List<Competency>();
            competencies.AddRange(dictCompetencies.Values);
            matrix.competencies = competencies;
            con.Close();
            return matrix;
        }

        public MatrixData GetMatrixHeaders(string moduleID, string languageID)
        {
            MatrixData matrix = new MatrixData();
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT CompetencyMatrix.Name, XAxis, YAxis " + 
                "FROM CompetencyMatrix " +
                "INNER JOIN Module " +
                "ON CompetencyMatrix.CompetencyMatrixID = Module.CompetencyMatrixID " +
                "WHERE ModuleID = @moduleID", con);
            cmd.Parameters.AddWithValue("@moduleID", moduleID.Trim());
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                matrix.Name = reader.GetString(0);
                matrix.XAxis = translationQuery.TranslateArray(reader.GetString(1).Split(','), languageID);
                matrix.YAxis = translationQuery.TranslateArray(reader.GetString(2).Split(','), languageID);                
            }
            con.Close();

            return matrix;
        }

        public List<Section> GetSectionNames(string languageID)
        {
            List<Subsection> subsections = new List<Subsection>();
            List<Section> sectionNames = new List<Section>();
            languageID = languageID.Trim().ToUpper();

            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT Section.SectionID, Section.SectionName, SubsectionID, SubsectionCode, SubsectionName, RequiredBool " +
                                            "FROM Subsection " +
                                            "INNER JOIN Section " +
                                            "ON Section.SectionID = Subsection.SectionID " +
                                            "ORDER BY Section.SectionID", con);
            SqlDataReader reader = cmd.ExecuteReader();
            int sectionID = 100;
            Section section = new Section();
            while (reader.Read())
            {
                Subsection subsection = new Subsection();
                int currentSectionID = reader.GetInt32(0);
                if(!sectionID.Equals(currentSectionID))
                {
                    if(!sectionID.Equals(100))
                    {
                        section.Subsections = subsections;
                        sectionNames.Add(section);
                        section = new Section();
                        subsections = new List<Subsection>();
                    }
                    sectionID = currentSectionID;
                    section.SectionID = sectionID;
                    section.SectionName = translationQuery.GetTranslation(reader.GetString(1), languageID);  
                }
                subsection.SubsectionID = reader.GetInt32(2);
                subsection.SectionID = sectionID;
                string subsectionCode = reader.GetDouble(3).ToString();
                subsection.SubsectionCode = sectionID + "." + subsectionCode;
                subsection.RequiredBool = reader.GetBoolean(5);   
                subsection.SubsectionName = translationQuery.GetTranslation(reader.GetString(4), languageID);
                subsections.Add(subsection);
            }
            if(!sectionID.Equals(100))
            {
                section.Subsections = subsections;
                sectionNames.Add(section);
            }
            con.Close();
            return sectionNames;
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
                subsection.SectionID = reader.GetInt32(2);
                subsection.RequiredBool = reader.GetBoolean(4);
                string englishWord = reader.GetString(3);
                languageID = languageID.Trim().ToUpper();
                subsection.SubsectionName = translationQuery.GetTranslation(englishWord, languageID);
               
                names.Add(subsection);
            }
            con.Close();
                return names;
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

        public void PostCompetency(int userID)
        {
            HttpContext context = HttpContext.Current;
            double version = GetCompetencyVersion(System.Web.HttpContext.Current.Request["moduleID"], System.Web.HttpContext.Current.Request["coordinate"], context.Request["languageID"].Trim().ToUpper());
            version++;
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Competency(ModuleID, Coordinate, CompetencyDescription, CompetencyLevel, VersionNumber, AuthorID)" +
                "VALUES (@moduleID, @coordinate, @description, @level, @version, @authorID)", con);
            cmd.Parameters.AddWithValue("@moduleID", context.Request["moduleID"].Trim());
            cmd.Parameters.AddWithValue("@coordinate", context.Request["coordinate"]);
            cmd.Parameters.AddWithValue("@description", context.Request["description"].Trim());
            cmd.Parameters.AddWithValue("@level", context.Request["level"]);
            cmd.Parameters.AddWithValue("@languageID", context.Request["languageID"].Trim().ToUpper());
            cmd.Parameters.AddWithValue("@version", version);
            cmd.Parameters.AddWithValue("@authorID", userID);
            SqlDataReader reader = cmd.ExecuteReader();
            con.Close();
        }

        public void PostModuleData(int userID)
        {
            HttpContext context = HttpContext.Current;
            double version = GetSubsectionVersion(System.Web.HttpContext.Current.Request["moduleID"], Int32.Parse(System.Web.HttpContext.Current.Request["subsectionID"]), context.Request["languageID"]);
            version++;
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO ModuleContent(ModuleID, SubsectionID, LanguageID, AuthorID, Content, VersionNumber) " +
                "VALUES (@moduleID, @subsectionID, @languageID, @userID, @content, @version)", con);
            cmd.Parameters.AddWithValue("@moduleID", context.Request["moduleID"].Trim());
            cmd.Parameters.AddWithValue("@subsectionID", context.Request["subsectionID"]);
            cmd.Parameters.AddWithValue("@languageID", context.Request["languageID"].Trim().ToUpper());
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@content", context.Request["content"]);
            cmd.Parameters.AddWithValue("@version", version);
            SqlDataReader reader = cmd.ExecuteReader();
            con.Close();
        }
    }
}