﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinhasTarefaAPI.Models
{
    public  class Token
    {
        public int Id { get; set; }
        public string RefleshToken { get; set; }
        public ApplicationUser usuario { get; set; }
        public bool Utilizado { get; set; }
        public DateTime Criado { get; set; }
        public DateTime? Atualizado { get; set; }
    }
}