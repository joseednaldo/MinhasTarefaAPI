using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinhasTarefaAPI.Models
{
    public  class Token
    {

        [Key]
        public int Id { get; set; }
        public string RefleshToken { get; set; }

        [ForeignKey("Usuario")]
        public string UsuarioId { get; set; }
        public ApplicationUser Usuario { get; set; }


        public bool Utilizado { get; set; }
        public DateTime ExpirationToken { get; set; }
        public DateTime ExpirationRefleshToken { get; set; }
        public DateTime Criado { get; set; }
        public DateTime? Atualizado { get; set; }
    }
}
