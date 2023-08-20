using System;
using System.Web.Http;

namespace SistemaWebAPI.Controllers
{
    public class MedicosController : ApiController
    {
        private readonly Repositories.Database.SQLServer.ADO.Medico repository;
        public MedicosController() 
        {
            try
            {
                repository = new Repositories.Database.SQLServer.ADO.Medico(Configurations.SQLServer.getConnectionString());
            }
            catch (Exception ex)
            {
                Logger.Log.write(ex, Configurations.Log.getFullPath());
            }
        }
        // GET: api/Medicos
        public IHttpActionResult Get()
        {
            try 
            {
                return Ok(repository.get());
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
                Models.Medico medico = repository.getById(id);
                if (medico.Codigo == 0)
                    return NotFound();

                return Ok(medico);
            }
            catch (Exception ex)
            {
                Logger.Log.write(ex, Configurations.Log.getFullPath());
                return InternalServerError();
            }

        }   
            
        

        // POST: api/Medicos
        public IHttpActionResult Post(Models.Medico medico)
        {
            try
            {
                if (medico.Nome.Trim() == "" || medico.CRM.Trim() == "")
                    return BadRequest("Nome e/ou CRM do médico não podem ser vazios.");

                repository.add(medico);

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

                int linhasAfetadas = repository.update(id, medico);

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
                int linhasAfetadas = repository.delete(id);
                
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
