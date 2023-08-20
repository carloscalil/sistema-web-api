using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Logger
{
    public  class Log
    {
       public static void write(Exception ex,string fullPath)
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fullPath, true))
            {
                sw.WriteLine($"\n------\nData:{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")} \n Mensagem:{ex.Message} \n StackTrace:{ex.StackTrace} \n InnerException:{ex.InnerException} \n Tipo do erro: {ex.GetType()} \n Source: {ex.Source} \n TargetSite: {ex.TargetSite}");
            }
        }
    }
}
