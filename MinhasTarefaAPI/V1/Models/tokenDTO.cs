using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinhasTarefaAPI.V1.Models
{
    public class tokenDTO
    {
        public string Token  { get; set; }
        public DateTime Expiration  { get; set; }
        public string RefleshToken { get; set; }
        public DateTime ExpirationRefleshToken { get; set; }
    }
}
