using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using netbullAPI.Extensions;
using netbullAPI.Interfaces;
using netbullAPI.Security.MidwareDB;
using netbullAPI.Security.Models;
using netbullAPI.Security.Service;
using netbullAPI.Security.ViewModels;
using netbullAPI.Util;
using netbullAPI.ViewModels;
using System.Net;

namespace netbullAPI.Security.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaController : BaseController
    {
        public ContaController(INotificador notificador) : base(notificador) { }

       /// <summary>
       /// Retorna todos os usuários cadastrados.
       /// </summary>
       /// <param name="neUser"></param>
       /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> getAllUsersAsync([FromServices] NE_User neUser)
        {
            try
            {
                var listaUsu = await neUser.getAllUsersAsync();
                if (listaUsu == null)
                {
                    return NotFound(
                       new
                       {
                           status = HttpStatusCode.NotFound,
                           Error = Notificacoes()
                       });
                }
                else
                {
                    return Ok(listaUsu);
                }
            }
            catch(Exception ex)
            {
                Notificar("Falha ao buscar usuários.");
                return StatusCode(500, Notificacoes());
            }
        }

        /// <summary>
        /// Cadastrar usuário para acesso ao sistema.
        /// </summary>
        /// <param name="neUser"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("registrar")]
        public async Task<IActionResult> RegisterAsync([FromServices] NE_User neUser, 
                                      [FromBody] RegistrarUserViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<User>(ModelState.RecuperarErros()));

            try
            {
                User usu = new User()
                {
                    user_id = 0,
                    user_nome = viewModel.user_nome,
                    user_email = viewModel.user_email,
                    user_accessKey = viewModel.user_accessKey
                };

                usu = await neUser.CadastroDeUserAsync(usu);
                if (usu.user_id == 0)
                {
                    return UnprocessableEntity(
                       new
                       {
                           status = HttpStatusCode.UnprocessableEntity,
                           Error = Notificacoes()
                       });
                }
                else
                {
                    return Created($"/{usu.user_id}", usu);
                }
            }
            catch(Exception ex)
            {
                Notificar("Falha ao cadastrar usuário.");
                return StatusCode(500, Notificacoes());
            }
            
        }

        /// <summary>
        /// Login no sistema com uma conta existente.
        /// </summary>
        /// <param name="_tokenService"></param>
        /// <param name="neUsuario"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromServices] TokenService _tokenService,
                                  [FromServices] NE_User neUsuario,
                                  [FromBody] LoginUserViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<User>(ModelState.RecuperarErros()));

            try
            {
                User usu = new User()
                {
                    user_id = 0,
                    user_nome = viewModel.user_nome,
                    user_email = null,
                    user_accessKey = viewModel.user_accessKey
                };

                var usuConsulta = await neUsuario.VerificarUsuarioSenhaAsync(usu);

                if (usuConsulta != null)
                {
                    if(usuConsulta.user_id != 0)
                    {
                        var token = await _tokenService.GenerateToken(usuConsulta);
                        return Ok(
                            new
                            {
                                status = HttpStatusCode.OK,
                                Token = token
                            });
                    }
                    else
                    {
                        return Unauthorized(
                           new
                           {
                               status = HttpStatusCode.Unauthorized,
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
            catch(Exception ex)
            {
                Notificar("Falha ao realizar login.");
                return StatusCode(500,Notificacoes());
            }
        }

        /// <summary>
        /// Deletar um usuário do sistema passando o ID.
        /// </summary>
        /// <param name="neUser"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUserAsync([FromServices] NE_User neUser, int id)
        {
            try
            {
                var sucess = await neUser.DeleteUserAsync(id);

                if (!sucess)
                {
                    return NotFound(
                       new
                       {
                           status = HttpStatusCode.NotFound,
                           Error = Notificacoes()
                       });
                }
                else
                {
                    return Ok();
                }
            }
            catch(Exception ex)
            {
                Notificar("Falha ao deletar usuário.");
                return StatusCode(500, Notificacoes());
            }       
        }

        /// <summary>
        /// Alterar senha de um usuário.
        /// </summary>
        /// <param name="neUser"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPatch("alterarSenha")]
        public async Task<IActionResult> AlterarSenhaAsync([FromServices] NE_User neUser, [FromBody] AlterarUserSenhaViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<User>(ModelState.RecuperarErros()));

            try
            {
                User usu = new User()
                {
                    user_id = 0,
                    user_nome = viewModel.user_nome,
                    user_email = viewModel.user_email,
                    user_accessKey = viewModel.user_accessKey
                };
        
                var sucess = await neUser.alterarSenhaAsync(usu);

                if (!sucess)
                {
                    return NotFound(
                       new
                       {
                           status = HttpStatusCode.NotFound,
                           Error = Notificacoes()
                       });
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                Notificar("Falha ao alterar senha do usuário.");
                return BadRequest(Notificacoes());
            }
        }
    }
}
