using System;

namespace MinhasTarefaAPI.Models
{
    public  class Token
    {
        public int Id { get; set; }
        public string RefleshToken { get; set; }
        public ApplicationUser Usuario { get; set; }
        public bool Utilizado { get; set; }
        public DateTime Criado { get; set; }
        public DateTime? Atualizado { get; set; }
        public DateTime ExpirationToken { get; set; }
        public DateTime ExpirationRefleshToken { get; set; }
    }
}
