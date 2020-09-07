using MinhasTarefaAPI.Database;
using MinhasTarefaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinhasTarefaAPI.Repositories
{
    public class TarefaRepository : Contracts.ITarefaRepository
    {
        private readonly MinhasTarefasContext _banco;

        public TarefaRepository(MinhasTarefasContext contexto)
        {
            _banco = contexto;
        }

        public List<Tarefa> Restauracao(ApplicationUser usuario, DateTime dataUltimaSocronizacao)
        {
            var query = _banco.Tarefas.Where(u=>u.UsuarioId == usuario.Id).AsQueryable();
            if (dataUltimaSocronizacao != null)
            {
                query.Where(a => a.Criado >= dataUltimaSocronizacao || a.Atualizado >= dataUltimaSocronizacao);
            }
            return query.ToList<Tarefa>();
        }


        /*Tarefa IdTarefaAPI - App tem recebe o IdTarefaAPI = TarefaLocal*/
        public List<Tarefa> Sicronizacao(List<Tarefa> Tarefas)
        {

            #region  Cadastro de novas tarefas
            var tarefasNovas = Tarefas.Where(a => a.IdTarefaApi == 0).ToList();
            var tarefaExcluidaAtualziadas = Tarefas.Where(t => t.IdTarefaApi != 0).ToList();

            if (tarefasNovas.Count() > 0)
            {
                foreach (var tarefa in tarefasNovas)
                {
                    _banco.Tarefas.Add(tarefa);
                }
                //_banco.SaveChanges();
            }
            #endregion

            #region  Atualização de registro / Exclusão lógica do resgistro
           
            if (tarefaExcluidaAtualziadas.Count() > 0)
            {
                foreach (var tarefa in tarefaExcluidaAtualziadas)
                {
                    _banco.Tarefas.Update(tarefa);
                }
            }
            #endregion

            _banco.SaveChanges();

            return tarefasNovas.ToList();
 
        }
    }
}
