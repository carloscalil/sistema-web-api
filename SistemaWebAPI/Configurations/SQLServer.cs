using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaWebAPI.Configurations
{
    public class SQLServer
    {
        public static string getConnectionString() 
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["consultorio"].ConnectionString;
        }
    }
}