using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MinhasTarefaAPI.Models
{
    public class Tarefa
    {

        /// <summary>
        /// CADA APP VAI TER SEU PROPRIO BANCO DE DADOS EM CADA CELULAR
        /// </summary>
        ///

        [Key]
        public int IdTarefaApi { get; set; }

        public int IdTarefaApp { get; set; }

        public string Titulo { get; set; }
        public DateTime DataHora { get; set; }
        public string Local { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public bool Concluido { get; set; }
        public bool Excluido { get; set; }
        public DateTime Criado { get; set; }
        public DateTime Atualizado { get; set; }
        //public bool Sicronizado { get; set; }
        [ForeignKey("Usuario")]
        public string UsuarioId { get; set; }
        public virtual ApplicationUser Usuario { get; set; }




    }
}
