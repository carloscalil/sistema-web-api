using System;
using System.ComponentModel.DataAnnotations;


namespace Models
{
    public class Medico
    {
        public int Codigo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage ="Nome obrigatório.")]
        [StringLength(200, ErrorMessage ="Nome não pode conter mais de 200 caracteres.")]
        public string Nome { get; set; }
        public DateTime? DataNascimento { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "CRM obrigatório.")]
        [StringLength(9, ErrorMessage = "Nome não pode conter mais de 9 caracteres.")]
        public string CRM { get; set; }
      

        public Medico(){
            this.Codigo = 0;
            this.Nome = "";
            this.DataNascimento= null;
            this.CRM = "";
        }
    }
}