using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SistemaWebAPI.Configurations
{
    public class Log
    {
        public static string getFullPath()
        {
            string fileName = $"{DateTime.Now.ToString("yyyy-MM-dd")}.txt";
            string path = System.Configuration.ConfigurationManager.AppSettings["consultorio-caminho-arquivo-log"];
            string fullPath=System.IO.Path.Combine(path, fileName);
            return fullPath;
        }
    }
}