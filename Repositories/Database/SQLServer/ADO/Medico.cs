using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;



namespace Repositories.Database.SQLServer.ADO
{
    public class Medico
    {
        public static List<Models.Medico> get(string connectionString)
        {
            List<Models.Medico> medicos = new List<Models.Medico>();

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = connectionString;
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select codigo,nome,datanascimento,crm from medico;";

                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        Models.Medico medico = new Models.Medico();
                        medico.Codigo = (int)dr["codigo"];
                        medico.Nome = dr["nome"].ToString();
                        medico.DataNascimento = dr["datanascimento"] == DBNull.Value ? null : (DateTime?)dr["datanascimento"];
                        medico.CRM = dr["crm"].ToString();
                        /*if (dr["datanascimento"] !=DBNull.Value)
                        {
                            medico.DataNascimento = (DateTime)dr["datanascimento"];    
                        }
                        else 
                        {
                            medico.DataNascimento=null;
                        }*/



                        medicos.Add(medico);
                    }
                }
            }
            return medicos;
        }
    }
}
