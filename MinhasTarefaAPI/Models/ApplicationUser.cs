using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinhasTarefaAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int FullName { get; set; }
        public virtual ICollection<Tarefa> Tarefas { get; set; }
    }
}
