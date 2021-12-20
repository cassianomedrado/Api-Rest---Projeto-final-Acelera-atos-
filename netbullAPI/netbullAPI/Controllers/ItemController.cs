using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using netbullAPI.Entidade;
using netbullAPI.Interfaces;
using netbullAPI.MidwareDB;
using netbullAPI.Util;
using netbullAPI.ViewModels;
using System.Net;

namespace netbullAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : BaseController
    {
        public ItemController(INotificador notificador) : base(notificador) { }

        /// <summary>
        /// Busca Item
        /// </summary>
        /// <param name="ne_item"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPorID([FromServices] NE_Item ne_item, int id)
        {
            try
            {
                var itens = ne_item.BuscaItemPedido(id);
                if (itens == null)
                    return NotFound(new
                    {
                        status = HttpStatusCode.NotFound,
                        Error = Notificacoes()
                    });
                else return Ok(new
                {
                    itens = ne_item.BuscaItemPedido(id),
                    status = HttpStatusCode.OK,
                    Error = Notificacoes()
                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    status = HttpStatusCode.BadRequest,
                    Error = Notificacoes()
                });
            }
        }

        /// <summary>
        /// Criação de um Item
        /// </summary>
        /// <param name="ne_item"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> post([FromServices] NE_Item ne_item, [FromBody] Item item)
        {
            try
            {
                var new_item = ne_item.AdicionaItem(item);
                if (new_item != null)
                    return Created($"/{new_item.item_id}", new_item);
                else
                    return NotFound(new
                    {
                        status = HttpStatusCode.BadRequest,
                        Error = Notificacoes()
                    });
            }
            catch(Exception e)
            {
                return BadRequest(new
                {
                    status = HttpStatusCode.BadRequest,
                    Error = Notificacoes()
                });
            }
        }

        /// <summary>
        /// Altera quantidade Item
        /// </summary>
        /// <param name="ne_item"></param>
        /// <param name="item"></param>
        /// <param name="quantidade"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch([FromServices] NE_Item ne_item, int id, [FromBody] AlterarQtditemViewModel quantidade)
        {
            try
            {
                var result = ne_item.AlteraQuantidadeProduto(id, quantidade.item_qtdproduto);
                if (result)
                    return Ok(new
                    {
                        status = HttpStatusCode.OK,
                        Message = "Quantidade alterada com sucesso."
                    });
                else
                    return NotFound(new
                    {
                        status = HttpStatusCode.NotFound,
                        Error = Notificacoes()
                    });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    status = HttpStatusCode.BadRequest,
                    Error = Notificacoes()
                });
            }
        }

        /// <summary>
        /// Deleta Item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromServices] NE_Item ne_item, int id)
        {
            try
            {
                if (ne_item.DeletaItem(id))
                    return Ok(new
                    {
                        status = HttpStatusCode.OK,
                        Error = Notificacoes()
                    });
                else
                    return NotFound(new
                    {
                        status = HttpStatusCode.BadRequest,
                        Error = Notificacoes()
                    });
            }
            catch(Exception e)
            {
                return BadRequest(new
                {
                    status = HttpStatusCode.BadRequest,
                    Error = Notificacoes()
                });
            }
        }
    }
}
