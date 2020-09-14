using MinhasTarefaAPI.Database;
using MinhasTarefaAPI.Models;
using MinhasTarefaAPI.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinhasTarefaAPI.Repositories
{
    public class TokenRepositorie : ITokenRepositorie
    {
        private readonly MinhasTarefasContext _banco;

        public TokenRepositorie(MinhasTarefasContext contexto)
        {
            _banco= contexto;
        }

        public Token Obter(string refreshToken)
        {
            return _banco.Token.FirstOrDefault(a => a.RefleshToken == refreshToken && a.Utilizado==false);
        }

        public void Atualizar(Token token)
        {

            _banco.Token.Update(token);
            _banco.SaveChanges();

        }

        public void Cadastrar(Token token)
        {
            _banco.Token.Add(token);
            _banco.SaveChanges();
        }

    }
}
