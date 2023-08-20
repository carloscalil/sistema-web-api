using System;
using System.ComponentModel.DataAnnotations;


namespace Models
{
    public class Paciente
    {
        public int Codigo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Nome obrigatório.")]
        [StringLength(200, ErrorMessage = "Nome não pode conter mais de 200 caracteres.")]
        public string Nome { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email obrigatório.")]
        [StringLength(100, ErrorMessage = "Email não pode conter mais de 100 caracteres.")]
        public string Email { get; set; }

        public Paciente() {
            this.Codigo = 0;
            this.Nome = "";
            this.Email = "";
        }
    }
}