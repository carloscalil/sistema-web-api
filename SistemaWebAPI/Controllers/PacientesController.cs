using System;
using System.Web.Http;



namespace SistemaWebAPI.Controllers
{
    public class PacientesController : ApiController
    {
        private readonly Repositories.Database.SQLServer.ADO.Paciente repository;
        public PacientesController() 
        {
            try
            {
                repository = new Repositories.Database.SQLServer.ADO.Paciente(Configurations.SQLServer.getConnectionString());
            }
            catch (Exception ex)
            {
                Logger.Log.write(ex, Configurations.Log.getFullPath());
            }
        }
        // GET: api/Pacientes
        public IHttpActionResult Get()
        {
            try
            {    
                return Ok(repository.get());              
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
               Models.Paciente paciente=repository.getById(id);
            
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
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                repository.add(paciente);

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
            try {
                if (id != paciente.Codigo)
                    ModelState.AddModelError("Código","Código enviado no parâmetro é diferente do código do paciente.");                

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);


                int linhasAfetadas = repository.update(id, paciente);
                if (linhasAfetadas == 0)
                    return NotFound();

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

        // DELETE: api/Pacientes/5
        public IHttpActionResult Delete(int id)
        {
            try 
            {
                int linhasAfetadas = repository.delete(id);           
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

