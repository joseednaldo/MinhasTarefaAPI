using MinhasTarefaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinhasTarefaAPI.Repositories.Contracts
{
    public interface ITarefaRepository
    {

        /*
         * Enviar tarefas novas pra api fazer seja bkp , exclusao etc...em unica ação 
         * ex: 10 tarefas novas, podemos enviar em uma unica requisição pra api...
         * podemos ter na lista de tarefas uma nova tarefa, uma exclusão etc...
         * pode ser enviado em uma unica requisição.
         * */
        void Sicronizacao(List<Tarefa> Tarefas);

        /// <summary>
        /// se data nao enviar no parametro eu pego tudo do banco e faço bkp.
        ///se tiver a data os pegamos tudo a parti dela...
        /// </summary>
        /// <param name="dataUltimaSocronizacao"></param>
        /// <returns></returns>
        List<Tarefa> Restauracao(DateTime dataUltimaSocronizacao);
        

    }
}
