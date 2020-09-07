using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MinhasTarefaAPI.Models;
using MinhasTarefaAPI.Repositories.Contracts;

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
            //removendo os campos que nao cou validar
            ModelState.Remove("ConfirmacaoSenha");
            ModelState.Remove("Nome");
            
            if (ModelState.IsValid)
            {

                ApplicationUser usuario = _usuarioRepository.Obter(usuarioDTO.Email, usuarioDTO.Senha);

                if (usuario != null)
                {
                    //login no Idenity
                    _signInManager.SignInAsync(usuario, false);

                    // no futuro vamos retorna o token (JWT)
                    return Ok();

                }
                else
                {
                    return NotFound("Usuário não localizado!!!");
                }
            }
            else
            {
                return UnprocessableEntity(ModelState);
            }
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
                    // StringBuilder sb = new StringBuilder();
                    List<string> erros = new List<string>();
                    foreach (var erro in resultado.Errors)
                    {
                        erros.Add(erro.Description);
                    }

                    return UnprocessableEntity(erros);
                }else
                {
                    return Ok(usuario);
                }
                
            }
            else
            {
                return UnprocessableEntity(ModelState);
            }
        }

    }
}