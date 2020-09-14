using Microsoft.AspNetCore.Identity;
using MinhasTarefaAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasTarefaAPI.Repositories
{
    public class UsuarioRepository : Contracts.IUsuarioRepository
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public UsuarioRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public ApplicationUser Obter(string email, string senha)
        {
            // Os metodos ultilizados são assicronos com o result no final tornamos ele  metodos sicronos.
            var usuario=_userManager.FindByEmailAsync(email).Result;
            if (_userManager.CheckPasswordAsync(usuario,senha).Result)
            {
                return usuario;
            }else
            {
                // Pra validação podemos usar o recurso "DOMAIN NOTIFICATION
                throw new Exception("Usuário nao localizado!");
            }
        }
        public void Cadastrar(ApplicationUser usuario, string senha)
        {
            var result = _userManager.CreateAsync(usuario, senha).Result;

            if (!result.Succeeded)
            {
                /*Pra validação podemos usar o recurso "DOMAIN NOTIFICATION*/
                StringBuilder sb = new StringBuilder();
                foreach (var erro in result.Errors)
                {
                    sb.Append(erro.Description);
                }
                throw new Exception($"Usuário nao cadastrado! {sb.ToString()}");
            }
        }

        public ApplicationUser Obter(string idUsuario)
        {
            return  _userManager.FindByIdAsync(idUsuario).Result;
        }
    }
}
