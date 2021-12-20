using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using netbullAPI.Entidade;
using netbullAPI.Extensions;
using netbullAPI.Interfaces;
using netbullAPI.MidwareDB;
using netbullAPI.Util;
using netbullAPI.ViewModels;
using System.Net;

namespace netbullAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PessoaController : BaseController
    {
        public PessoaController(INotificador notificador) : base(notificador) { }

        /// <summary>
        /// Busca lista de todas as pessoas casdatradas
        /// </summary>
        /// <param name="nePessoa"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromServices] NE_Pessoa nePessoa)
        {
            try
            {
                var pessoas = await nePessoa.BuscaPessoas();
                if (pessoas != null)
                {
                    return Ok(pessoas);
                }
                else
                {
                    return NotFound(
                new
                {
                    status = HttpStatusCode.NotFound,
                    Error = Notificacoes()
                });
                }

            }
            catch (Exception e)
            {
                return NotFound(
                new
                {
                    status = HttpStatusCode.NotFound,
                    Error = Notificacoes()
                });
            }

        }

        /// <summary>
        /// Busca pessas atribuida ao parametro id informado
        /// </summary>
        /// <param name="nePessoa"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromServices] NE_Pessoa nePessoa, int id)
        {
            try
            {
                 var pessoa = await nePessoa.BuscaPessoaPorId(id);
                if (pessoa != null)
                {
                    return Ok(pessoa);
                }
                else
                {
                    return NotFound(
                    new
                    {
                        status = HttpStatusCode.NotFound,
                        Error = Notificacoes()
                    });
                }


            }
            catch
            {
                return BadRequest(
                new
                {
                    status = HttpStatusCode.BadRequest,
                    Error = Notificacoes()
                });
            }

        }

        /// <summary>
        /// Cadastra cliente utilizando os parametros informados no objeto pessoa
        /// </summary>
        /// <param name="nePessoa"></param>
        /// <param name="pessoa"></param>
        /// <param name="viewModel"></param> 
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromServices] NE_Pessoa nePessoa,
                                              [FromBody] CadastrarPessoaViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Pessoa>(ModelState.RecuperarErros()));
            try
            {
                Pessoa pessoa = new Pessoa()
                {
                    pessoa_id = 0,
                    pessoa_documento = viewModel.pessoa_documento,
                    pessoa_nome = viewModel.pessoa_nome,
                    pessoa_tipopessoa = viewModel.pessoa_tipopessoa
                };
                if (pessoa != null)
                {
                    var addPessoa = await nePessoa.InserirPessoa(pessoa);
                    if (addPessoa)
                    {
                        return Ok(pessoa);
                    }
                    else
                    {
                        return BadRequest(
                    new
                    {
                        status = HttpStatusCode.BadRequest,
                        Error = Notificacoes()
                    });
                    }

                }
                else
                {
                    return NotFound(
                    new
                    {
                        status = HttpStatusCode.BadRequest,
                        Error = Notificacoes()
                    });
                }
            }
            catch
            {
                return BadRequest(
                new
                {
                    status = HttpStatusCode.BadRequest,
                    Error = Notificacoes()
                });
            }

        }

        /// <summary>
        /// Realiza alteração na pessoa com o id informa e altera os demais parametros atribuidos
        /// </summary>
        /// <param name="nePessoa"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromServices] NE_Pessoa nePessoa, Pessoa pessoa)
        {
            try
            {
                if (pessoa != null)
                {
                    var attPessoa = await nePessoa.AtualizarPessoa(pessoa);
                    if (attPessoa != null)
                    {
                        return Ok(attPessoa);
                    }
                    else
                    {
                        return NotFound(
                    new
                    {
                        status = HttpStatusCode.NotFound,
                        Error = Notificacoes()
                    });
                    }
                }
                else
                {
                    return NotFound(
                    new
                    {
                        status = HttpStatusCode.NotFound,
                        Error = Notificacoes()
                    });
                }
            }
            catch
            {
                return BadRequest(
                new
                {
                    status = HttpStatusCode.BadRequest,
                    Error = Notificacoes()
                });
            }

        }

        /// <summary>
        /// Deleta a pessoa atribuida ao parametro id informado
        /// </summary>
        /// <param name="nePessoa"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromServices] NE_Pessoa nePessoa, int id)
        {
            try
            {
                var delPessoa = await nePessoa.DeletarPessoa(id);
                if (delPessoa)
                {
                    return Ok("Pessoa Deletada com Sucesso!");
                }
                else
                {
                    return NotFound(
                new
                {
                    status = HttpStatusCode.NotFound,
                    Error = Notificacoes()
                });
                }
            }
            catch
            {
                return BadRequest(
                new
                {
                    status = HttpStatusCode.BadRequest,
                    Error = Notificacoes()
                });
            }
        }
    };
}