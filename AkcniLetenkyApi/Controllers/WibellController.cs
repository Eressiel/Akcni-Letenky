using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace AkcniLetenkyApi.Controllers
{
    public class WibellController : ApiController
    {
        private SqlConnection dbcon = new SqlConnection(
            WebConfigurationManager.ConnectionStrings["WibellConnectionString"].ConnectionString);
        private bool connected = false;
        public WibellController()
        {
            try
            {
                dbcon.Open();
            }
            catch(Exception e)
            {
                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "/App_Data/Wibell_Errors.txt", e.Message + Environment.NewLine);
            }
            
        }

        ~WibellController()
        {
            try
            {
                dbcon.Close();
            }
            catch (Exception e)
            {
                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "/App_Data/Wibell_Errors.txt", e.Message + Environment.NewLine);
            }
        }

        [Route("api/wibell/setdatabase")]
        public void SetDatabase()
        {
            File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "/App_Data/DB_Log.txt", "In Setdatabase method..." + Environment.NewLine);
        }

    }
}
