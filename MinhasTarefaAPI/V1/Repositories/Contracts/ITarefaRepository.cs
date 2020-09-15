using MinhasTarefaAPI.V1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinhasTarefaAPI.V1.Repositories.Contracts
{
    public interface ITarefaRepository
    {

        /*
         * Enviar tarefas novas pra api fazer seja bkp , exclusao etc...em unica ação 
         * ex: 10 tarefas novas, podemos enviar em uma unica requisição pra api...
         * podemos ter na lista de tarefas uma nova tarefa, uma exclusão etc...
         * pode ser enviado em uma unica requisição.
         * */
        List<Tarefa> Sicronizacao(List<Tarefa> Tarefas);

        /// <summary>
        /// se data nao enviar no parametro eu pego tudo do banco e faço bkp.
        ///se tiver a data os pegamos todos os registros a parti dessa data...
        /// </summary>
        /// <param name="dataUltimaSocronizacao"></param>
        /// <returns>Lista de tarefas</returns>
        List<Tarefa> Restauracao(ApplicationUser usuario,DateTime dataUltimaSocronizacao);
        

    }
}
