using MinhasTarefaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinhasTarefaAPI.Repositories.Contracts
{
    public interface ITokenRepositorie
    {
        void Cadastrar(Token tokrn);
        Token Obter(string refreshToken);
        void Atualizar(Token token);
    }
}
