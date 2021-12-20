using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using netbullAPI.Entidade;
using netbullAPI.Interfaces;
using netbullAPI.MidwareDB;
using netbullAPI.Util;
using netbullAPI.ViewModels;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace netbullAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : BaseController
    {
        public ProdutoController(INotificador notificador) : base(notificador)
        {
        }


        /// <summary>
        /// Busca todos os produtos registrados
        /// </summary>
        /// <param name="ne_Produto"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromServices] NE_Produto ne_Produto)
        {
            try
            {
                var produtos = await ne_Produto.GetAllAsync();
                if (produtos == null)
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
                    return Ok(produtos);
                }
            }
            catch (Exception e)
            {
                Notificar("Falha ao buscar produto.");
                return StatusCode(500, Notificacoes());
            }
        }

        /// <summary>
        /// Busca um produto específico a partir do seu id
        /// </summary>
        /// <param name="ne_Produto"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPorIdAsync([FromServices] NE_Produto ne_Produto, int id)
        {
            try
            {
                var produto = await ne_Produto.GetPorIdAsync(id);
                if (produto != null)
                {
                    return Ok(produto);
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
                Notificar("Falha ao buscar produto.");
                return StatusCode(500, Notificacoes());
            }
        }

        /// <summary>
        /// Insere um novo registro de produto
        /// </summary>
        /// <param name="ne_Produto"></param>
        /// <param name="produto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromServices] NE_Produto ne_Produto, [FromBody] RegistrarProdutoViewModel produto)
        {
            try
            {
                var nvProduto = await ne_Produto.AdicionaProduto(produto);
                if (nvProduto != null)
                    return Created($"/{nvProduto.produto_id}", nvProduto);
                else
                    return BadRequest(
                            new
                            {
                                status = HttpStatusCode.BadRequest,
                                Error = Notificacoes()
                            });
            }
            catch (Exception e)
            {
                Notificar("Falha ao inserir produto.");
                return StatusCode(500, Notificacoes());
            }
        }

        /// <summary>
        /// Atualiza o nome de um registro de produto a partir do seu id
        /// </summary>
        /// <param name="ne_Produto"></param>
        /// <param name="produto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> PutAsync([FromServices] NE_Produto ne_Produto, [FromBody] Produto produto)
        {
            try
            {
                var atualizaProduto = await ne_Produto.AtualizaProduto(produto);
                if (atualizaProduto != null)
                    return Ok(atualizaProduto);
                else
                    return NotFound(
                            new
                            {
                                status = HttpStatusCode.NotFound,
                                Error = Notificacoes()
                            });
            }
            catch (Exception e)
            {
                Notificar("Falha ao alterar produto.");
                return StatusCode(500, Notificacoes());
            }
        }

        /// <summary>
        /// Atualiza o campo de nome de um produto a partir do id
        /// </summary>
        /// <param name="ne_Produto"></param>
        /// <param name="alterarProdutoNomeViewModel"></param>
        /// <returns></returns>
        [HttpPatch("Nome")]
        public async Task<IActionResult> AtualizaProdutoNomePatch([FromServices] NE_Produto ne_Produto, [FromBody] AlterarProdutoNomeViewModel alterarProdutoNomeViewModel)
        {
            try
            {
                var produto = new Produto() { produto_id = alterarProdutoNomeViewModel.produto_id, produto_nome = alterarProdutoNomeViewModel.produto_nome };

                var atualizaProduto = await ne_Produto.AtualizaProdutoPatch(Repository.CampoEditar.Nome, produto);

                if (atualizaProduto != null)
                {
                    return Ok(atualizaProduto);
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
                Notificar("Falha ao alterar nome do produto.");
                return StatusCode(500, Notificacoes());
            }
        }

        /// <summary>
        /// Atualiza o valor de um registro de produto a partir de seu id
        /// </summary>
        /// <param name="ne_Produto"></param>
        /// <param name="alterarProdutoValorViewModel"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch("Valor")]
        public async Task<IActionResult> AtualizaProdutoValorPatch([FromServices] NE_Produto ne_Produto, [FromBody] AlterarProdutoValorViewModel alterarProdutoValorViewModel)
        {
            try
            {
                var produto = new Produto() { produto_id = alterarProdutoValorViewModel.produto_id, produto_valor = alterarProdutoValorViewModel.produto_valor };

                var atualizaProduto = await ne_Produto.AtualizaProdutoPatch(Repository.CampoEditar.Valor, produto);

                if (atualizaProduto != null)
                {
                    return Ok(atualizaProduto);
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
                Notificar("Falha ao alterar valor do produto.");
                return StatusCode(500, Notificacoes());
            }
        }

        /// <summary>
        /// Deleta um registro de produto a partir do id
        /// </summary>
        /// <param name="ne_Produto"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromServices] NE_Produto ne_Produto, int id)
        {
            try
            {
                var resp = ne_Produto.DeletaProduto(id);
                if (resp)
                    return Ok(
                        new
                        {
                            mensagem = "Registro deletado com sucesso",
                            sucesso = true
                        });
                else
                    return NotFound(
                        new
                        {
                            status = HttpStatusCode.NotFound,
                            sucesso = false,
                            Error = Notificacoes()
                        });
            }
            catch (Exception e)
            {
                Notificar("Falha ao deletar produto.");
                return StatusCode(500, Notificacoes());
            }

        }
    }
}
