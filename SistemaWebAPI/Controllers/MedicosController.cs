using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SistemaWebAPI.Controllers
{
    public class MedicosController : ApiController
    {
        // GET: api/Medicos
        public IHttpActionResult Get()
        {
            try 
            {
                return Ok(Repositories.Database.SQLServer.ADO.Medico.get(Configurations.SQLServer.getConnectionString()));
            }
            catch (Exception ex)
            {
                Logger.Log.write(ex, Configurations.Log.getFullPath());
                return InternalServerError();
            }
        }

        // GET: api/Medicos/5
        public IHttpActionResult Get(int id)
        {
            try
            {
                Models.Medico medico = new Models.Medico();

                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = Configurations.SQLServer.getConnectionString();
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand())
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
                if (medico.Codigo == 0)
                    return NotFound();

                return Ok(medico);
            }
            catch (Exception ex)
            {

                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Configurations.Log.getFullPath(), true))
                {
                    sw.WriteLine($"\n------\nData:{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")} \n Mensagem: {ex.Message}");
                }
                return InternalServerError();
            }
        }

        // POST: api/Medicos
        public IHttpActionResult Post(Models.Medico medico)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = Configurations.SQLServer.getConnectionString();
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand())
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
                return Ok(medico);
            }
            catch (Exception ex)
            {

                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Configurations.Log.getFullPath(), true))
                {
                    sw.WriteLine($"\n------\nData:{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")} \n Mensagem: {ex.Message}");
                }
                return InternalServerError();
            }

        }

        // PUT: api/Medicos/5
        public IHttpActionResult Put(int id, Models.Medico medico)
        {
            try
            {
                if (id != medico.Codigo)
                    return BadRequest("Código enviado no parâmetro é diferente do código do paciente.");
                if (medico.Nome == "" || medico.CRM == "")
                    return BadRequest("Nome e/ou CRM do médico não podem ser vazios.");

                int linhasAfetadas = 0;
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = Configurations.SQLServer.getConnectionString();
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand())
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

                if (linhasAfetadas == 0)
                    return NotFound();

                return Ok(medico);
            }
            catch (Exception ex)
            {

                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Configurations.Log.getFullPath(), true))
                {
                    sw.WriteLine($"\n------\nData:{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")} \n Mensagem: {ex.Message}");
                }
                return InternalServerError();
            }
        }

        // DELETE: api/Medicos/5
        public IHttpActionResult Delete(int id)
        {
            try
            {
                int linhasAfetadas = 0;

                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = Configurations.SQLServer.getConnectionString();
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "delete from medico where codigo=@codigo;";
                        cmd.Parameters.Add(new SqlParameter("@codigo", System.Data.SqlDbType.Int)).Value = id;

                        linhasAfetadas = cmd.ExecuteNonQuery();
                    }
                }
                if (linhasAfetadas == 0)
                    return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {

                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Configurations.Log.getFullPath(), true))
                {
                    sw.WriteLine($"\n------\nData:{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")} \n Mensagem: {ex.Message}");
                }
                return InternalServerError();
            }
        }
    }
}
