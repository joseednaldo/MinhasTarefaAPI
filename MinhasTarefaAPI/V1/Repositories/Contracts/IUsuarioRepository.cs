using MinhasTarefaAPI.V1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinhasTarefaAPI.V1.Repositories.Contracts
{
    public interface IUsuarioRepository
    {
        void Cadastrar(ApplicationUser usuario, string senha);
        ApplicationUser Obter(string email, string senha);

        ApplicationUser Obter(string id);
    }
}
