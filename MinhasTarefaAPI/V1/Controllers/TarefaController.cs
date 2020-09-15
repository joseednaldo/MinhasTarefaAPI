using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MinhasTarefaAPI.V1.Models;
using MinhasTarefaAPI.V1.Repositories.Contracts;

namespace MinhasTarefaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class TarefaController : ControllerBase
    {
        private readonly ITarefaRepository _tarefaRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public TarefaController(ITarefaRepository tarefaRepository, UserManager<ApplicationUser> userManager)
        {
            _tarefaRepository = tarefaRepository;
            _userManager = userManager;
        }

        [Authorize]
        [HttpPost("sincronizar")]
        public ActionResult Sincronizar([FromBody]List<Tarefa> tarefas)
        {
            /* Retorna todas as tarefas cadastrada , vamos gravar no app mobile*/          
            return Ok(_tarefaRepository.Sicronizacao(tarefas));
        }

        [Authorize]
        [HttpGet("restaurar")]
        public ActionResult Restaurar(DateTime data)
        {
            //Recuperar o usuario logado no app.
            var usuario = _userManager.GetUserAsync(HttpContext.User).Result;

            return Ok(_tarefaRepository.Restauracao(usuario, data));

        }

        [HttpGet("modelo")]
        public ActionResult Modelo()
        {
            return Ok(new Tarefa());
        }

    }
}