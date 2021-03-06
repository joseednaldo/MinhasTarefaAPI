﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MinhasTarefaAPI.V1.Models
{
    public class Tarefa
    {

        /// <summary>
        /// CADA APP VAI TER SEU PROPRIO BANCO DE DADOS EM CADA CELULAR
        /// VAMOS USAR API PARA ATUALIZAR NO BANCO NA NUVEM E VICE-VERSA.
        /// </summary>
        ///

        [Key]
        public int IdTarefaApi { get; set; }  //ID DA TAREFA NO API (ID GERAL)
        public int IdTarefaApp { get; set; }  // ID DA TAREFA DO APP  (ID DA TAREFA DO USAURIO DO APP)
        public string Titulo { get; set; }
        public DateTime DataHora { get; set; }
        public string Local { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; }// TRABALHO,ESTUDO...
        public bool Concluido { get; set; }
        public bool Excluido { get; set; }   // EXCLUSÃO LÓGICA
        public DateTime Criado { get; set; }
        public DateTime Atualizado { get; set; }

        [ForeignKey("Usuario")]
        public string UsuarioId { get; set; }

        public virtual ApplicationUser Usuario { get; set; }




    }
}
