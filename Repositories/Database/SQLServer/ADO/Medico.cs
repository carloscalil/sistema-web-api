﻿using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;



namespace Repositories.Database.SQLServer.ADO
{
    public class Medico: IRepository<Models.Medico>
    {
        private readonly SqlConnection conn;
        private readonly SqlCommand cmd;

        public Medico(string connectionString)
        {
            conn = new SqlConnection(connectionString);
            cmd = new SqlCommand();
            
        }
        public List<Models.Medico> get()
        {
            List<Models.Medico> medicos = new List<Models.Medico>();

            using (conn)
            {
                conn.Open();

                using (cmd)
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
        public Models.Medico getById(int id)
        {
            Models.Medico medico = new Models.Medico();

            using (conn)
            {
                conn.Open();

                using (cmd)
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select codigo,nome,datanascimento,crm from medico where codigo=@codigo;";
                    cmd.Parameters.Add(new SqlParameter("@codigo", System.Data.SqlDbType.Int)).Value = id;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            medico.Codigo = (int)dr["codigo"];
                            medico.Nome = dr["nome"].ToString();
                            if (dr["datanascimento"] != DBNull.Value)
                            {
                                medico.DataNascimento = (DateTime)dr["datanascimento"];
                            }
                            else
                            {
                                medico.DataNascimento = null;
                            }
                            medico.CRM = dr["crm"].ToString();
                        }
                    }

                }
            }
            return medico;

        }
        public void add(Models.Medico medico) 
        {
            using (conn)
            {
                conn.Open();

                using (cmd)
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "insert into medico (nome,datanascimento,crm) values (@nome,@datanascimento,@crm); select convert(int,@@identity) as codigo;";
                    cmd.Parameters.Add(new SqlParameter("@nome", System.Data.SqlDbType.VarChar)).Value = medico.Nome;
                    if (medico.DataNascimento != null)
                    {
                        cmd.Parameters.Add(new SqlParameter("@datanascimento", System.Data.SqlDbType.Date)).Value = medico.DataNascimento;
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("datanascimento", System.Data.SqlDbType.Date)).Value = DBNull.Value;

                    }
                    cmd.Parameters.Add(new SqlParameter("crm", System.Data.SqlDbType.Char)).Value = medico.CRM;
                    medico.Codigo = (int)cmd.ExecuteScalar();
                }
            }
        }
        public int update(int id, Models.Medico medico)
        {
            int linhasAfetadas = 0;
            using (conn)
            {
                conn.Open();

                using (cmd)
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "update medico set nome=@nome,datanascimento=@datanascimento,crm=@crm where codigo=@codigo;";
                    cmd.Parameters.Add(new SqlParameter("@nome", System.Data.SqlDbType.VarChar)).Value = medico.Nome;
                    if (medico.DataNascimento != null)
                    {
                        cmd.Parameters.Add(new SqlParameter("@datanascimento", System.Data.SqlDbType.Date)).Value = medico.DataNascimento;
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@datanascimento", System.Data.SqlDbType.Date)).Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(new SqlParameter("@crm", System.Data.SqlDbType.Char)).Value = medico.CRM;
                    cmd.Parameters.Add(new SqlParameter("@codigo", System.Data.SqlDbType.Int)).Value = id;

                    linhasAfetadas = cmd.ExecuteNonQuery();
                }
            }
            return linhasAfetadas;
        }
        public int delete(int id)
        {
            int linhasAfetadas = 0;

            using (conn)
            {
                conn.Open();

                using (cmd)
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "delete from medico where codigo=@codigo;";
                    cmd.Parameters.Add(new SqlParameter("@codigo", System.Data.SqlDbType.Int)).Value = id;

                    linhasAfetadas = cmd.ExecuteNonQuery();
                }
            }
           return linhasAfetadas;
        }
        
    }
}
