using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using System.Net.Sockets;
using System.IO;


namespace SistemaWebAPI.Controllers
{
    public class PacientesController : ApiController
    {
        // GET: api/Pacientes
        public IHttpActionResult Get()
        {
            try
            {
                List<Models.Paciente> pacientes = new List<Models.Paciente>();



                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = Configurations.SQLServer.getConnectionString();
                    conn.Open();


                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "select codigo,nome,email from paciente;";

                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            Models.Paciente paciente = new Models.Paciente();
                            paciente.Codigo = (int)dr["codigo"];
                            paciente.Nome = (string)dr["nome"];
                            paciente.Email = (string)dr["email"];

                            pacientes.Add(paciente);
                        }

                    }
                }

                return Ok(pacientes);
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

        // GET: api/Pacientes/5
        public IHttpActionResult Get(int id)
        {
            try
            { 
            Models.Paciente paciente = new Models.Paciente();

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = Configurations.SQLServer.getConnectionString();
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select codigo,nome,email from paciente where codigo=@codigo;";
                    cmd.Parameters.Add(new SqlParameter("@codigo", System.Data.SqlDbType.Int)).Value = id;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            paciente.Codigo = (int)dr["codigo"];
                            paciente.Nome = dr["nome"].ToString();
                            paciente.Email = dr["email"].ToString();
                        }
                    }
                }
            }
            if (paciente.Codigo == 0)
                return NotFound();

            return Ok(paciente);
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

        // POST: api/Pacientes
        public IHttpActionResult Post(Models.Paciente paciente)
        {
            try {
                if (paciente.Nome == "" || paciente.Email == "")
                    return BadRequest("Nome e/ou Email do paciente não podem ser vazios.");

                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = Configurations.SQLServer.getConnectionString();
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "insert into paciente (nome,email) values (@nome,@email); select convert (int,@@identity) as codigo;";
                        cmd.Parameters.Add(new SqlParameter("@codigo", System.Data.SqlDbType.Int)).Value = paciente.Codigo;
                        cmd.Parameters.Add(new SqlParameter("@nome", System.Data.SqlDbType.VarChar)).Value = paciente.Nome;
                        cmd.Parameters.Add(new SqlParameter("@email", System.Data.SqlDbType.VarChar)).Value = paciente.Email;
                        paciente.Codigo = (int)cmd.ExecuteScalar();
                    }
                }
                return Ok(paciente);
            }
            catch (Exception ex) 
            {
                {

                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(Configurations.Log.getFullPath(), true))
                    {
                        sw.WriteLine($"\n------\nData:{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")} \n Mensagem: {ex.Message}");
                    }
                    return InternalServerError();
                }
            }       
        }

        // PUT: api/Pacientes/5
        public IHttpActionResult Put(int id, Models.Paciente paciente)
        {
            if (id != paciente.Codigo)
                return BadRequest("Código enviado no parâmetro é diferente do código do paciente.");
            if (paciente.Nome.Trim() == "" || paciente.Email.Trim() == "")
                return BadRequest("Nome e/ou Email do paciente não podem ser vazios.");

            int linhasAfetadas = 0;

            using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = Configurations.SQLServer.getConnectionString();
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "update paciente set nome=@nome, email=@email where codigo = @codigo;";
                        cmd.Parameters.Add(new SqlParameter("@nome", System.Data.SqlDbType.VarChar)).Value = paciente.Nome;
                        cmd.Parameters.Add(new SqlParameter("@email", System.Data.SqlDbType.VarChar)).Value = paciente.Email;
                        cmd.Parameters.Add(new SqlParameter("@codigo", System.Data.SqlDbType.Int)).Value = id;

                       linhasAfetadas=cmd.ExecuteNonQuery();
                    }
                }
            if (linhasAfetadas == 0)
                return NotFound();

            return Ok(paciente);       
        }

        // DELETE: api/Pacientes/5
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
                    cmd.CommandText = "delete from paciente where codigo=@codigo;";
                    cmd.Parameters.Add(new SqlParameter("codigo", System.Data.SqlDbType.Int)).Value = id;

                    linhasAfetadas = cmd.ExecuteNonQuery();
                }
            }
            if (linhasAfetadas == 0)
            {
                return NotFound();
            }
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

