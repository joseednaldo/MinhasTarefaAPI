using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using MinhasTarefaAPI.Models;
using MinhasTarefaAPI.Repositories.Contracts;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace MinhasTarefaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public usuarioController(IUsuarioRepository usuarioRepository, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager )
        {
            _usuarioRepository = usuarioRepository;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        
        [HttpPost("login")]
        public ActionResult Login([FromBody]UsuarioDTO usuarioDTO)
        {
            //removendo os campos que não vamos validar no mommento de fazer o login.
            ModelState.Remove("ConfirmacaoSenha");
            ModelState.Remove("Nome");
            
            if (ModelState.IsValid)
            {
                ApplicationUser usuario = _usuarioRepository.Obter(usuarioDTO.Email, usuarioDTO.Senha);
                if (usuario != null)
                {
                    //login no Idenity
                    _signInManager.SignInAsync(usuario, false);

                    //no futuro vamos retorna o token (JWT)

                    return Ok(BuildToken(usuario));

                }else
                {
                    return NotFound("Usuário não localizado!!!");
                }
            }else
            {
                return UnprocessableEntity(ModelState);
            }
        }

        private object BuildToken(ApplicationUser usuario)
        {
            var claims = new[]
            {
               new Claim(JwtRegisteredClaimNames.Email,usuario.Email)
           };

            #region  Cria chave

            /*O ideal é criar a chave(texto) no appsettings.js*/
            var key  = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("chave-api-jwt-minhas-tarefas"));
            var sign = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);   // criando assinatura
            var exp = DateTime.UtcNow.AddHours(1);

            //Classe que gera o token
            JwtSecurityToken token = new JwtSecurityToken(
                 issuer : null,  // nao quero dizer quem esta emitindo o token
                 audience: null, // não importa quem vai consumir o token
                 claims: claims, 
                 expires:exp,
                 signingCredentials: sign

                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return new { token = tokenString, expiracation = exp };
            #endregion
        }

        [HttpPost()]//rota padrao
        public ActionResult Cadastrar([FromBody]UsuarioDTO usuarioDTO)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser usuario = new ApplicationUser();
                usuario.FullName = usuarioDTO.Nome;
                usuario.UserName = usuarioDTO.Email;
                usuario.Email = usuarioDTO.Email;

                var resultado = _userManager.CreateAsync(usuario, usuarioDTO.Senha).Result;

                if (!resultado.Succeeded)
                {
                    //StringBuilder sb = new StringBuilder();
                    List<string> erros = new List<string>();
                    foreach (var erro in resultado.Errors)
                    {
                        erros.Add(erro.Description);
                    }

                    return UnprocessableEntity(erros);

                }else{
                    return Ok(usuario);
                }
                
            }else
            {
                return UnprocessableEntity(ModelState);
            }
        }

    }
}