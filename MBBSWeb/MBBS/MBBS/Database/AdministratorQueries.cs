using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MBBS.Database
{
    public class AdministratorQueries
    {
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MS_TableConnectionString"].ConnectionString);

        public void PostDocentModule()
        {
            HttpContext context = HttpContext.Current;
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Module(ModuleID, ModuleName, CompetencyMatrixID, AdministratorID, Location) " +
                "VALUES(@moduleID, @moduleName, @competencyMatrixID, @administratorID, @location)", con);
            cmd.Parameters.AddWithValue("@moduleID", context.Request["moduleID"].Trim());
            cmd.Parameters.AddWithValue("@moduleName", context.Request["moduleName"].Trim());
            cmd.Parameters.AddWithValue("@competencyMatrixID", context.Request["matrixID"]);
            cmd.Parameters.AddWithValue("@administratorID", context.Request["administratorID"]);
            cmd.Parameters.AddWithValue("@location", context.Request["location"].Trim());
            SqlDataReader reader = cmd.ExecuteReader();
            con.Close();
        }
    }
}