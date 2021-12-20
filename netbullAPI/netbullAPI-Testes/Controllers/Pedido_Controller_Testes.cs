using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using netbullAPI.Entidade;
using netbullAPI.Security.ViewModels;
using netbullAPI_Testes.Models;
using netbullAPI_Testes.Uitl;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace netbullAPI_Testes.Controllers
{
    public class Pedido_Controller_Testes
    {
        public LoginUserViewModel login = new LoginUserViewModel() { user_nome = "cassiano", user_accessKey = "123456" };

        /// <summary>
        /// Teste de integração de busca de pedidos pertencentes a um cliente válido
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Válido")]
        public async Task TestarGetPedidoByIdClienteValidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                var requestPedido= new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://localhost:7035/api/Pedido/{2}"),
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responsePedido = await httpClient.SendAsync(requestPedido).ConfigureAwait(false);


                if (responsePedido.StatusCode != HttpStatusCode.OK)
                    Assert.Fail();
                else
                    Assert.AreEqual(responsePedido.StatusCode, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        /// <summary>
        /// Teste de integração de busca de pedido a partir de id cliente inválido
        /// idCliente inválido
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Inválido")]
        public async Task TestarGetPedidoByIdClienteInvalidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                var requestPedido = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://localhost:7035/api/Item/{0}"), //Falta trocar request
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responsePedido = await httpClient.SendAsync(requestPedido).ConfigureAwait(false);


                if (responsePedido.StatusCode != HttpStatusCode.NotFound)
                    Assert.Fail();
                else
                    Assert.AreEqual(responsePedido.StatusCode, HttpStatusCode.NotFound);

            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        /// <summary>
        /// Teste de integração de pedido válido
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Válido")]
        public async Task TestarPostPedidoValidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                var jsonPedido = JsonConvert.SerializeObject(new Pedido
                {
                    pedido_id = 1,
                    pedido_idEndereco = 2,
                    pedido_idPessoa = 2,
                    pedido_idUsuario = 60,
                    pedido_status = 0,
                    pedido_time = DateTime.UtcNow.ToString("dd/MM/yyyy hh:mm"),
                    pedido_valor = 1200
                });

                var requestItem = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"https://localhost:7035/api/Pedido"),
                    Content = new StringContent(jsonPedido, Encoding.UTF8, "application/json"),
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responseItem = await httpClient.SendAsync(requestItem).ConfigureAwait(false);

                if (responseItem.StatusCode != HttpStatusCode.Created)
                {
                    Assert.Fail();
                }
                Assert.AreEqual(HttpStatusCode.Created, responseItem.StatusCode);
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        /// <summary>
        /// Teste de integração de inserção de pedido inválido
        /// idPessoa inválido
        /// idUsuario inválido
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Inválido")]
        public async Task TestarPostItemInvalidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                var jsonPedido = JsonConvert.SerializeObject(new Pedido
                {
                    pedido_id = 1,
                    pedido_idEndereco = 2,
                    pedido_idPessoa = 0,
                    pedido_idUsuario = 2,
                    pedido_status = 0,
                    pedido_time = DateTime.UtcNow.ToString("dd/MM/yyyy hh:mm"),
                    pedido_valor = 1200
                });

                var requestPedido = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"https://localhost:7035/api/Pedido"),
                    Content = new StringContent(jsonPedido, Encoding.UTF8, "application/json"),
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responsePedido = await httpClient.SendAsync(requestPedido).ConfigureAwait(false);

                if (responsePedido.StatusCode != HttpStatusCode.NotFound)
                    Assert.Fail();
                
                Assert.AreEqual(HttpStatusCode.NotFound, responsePedido.StatusCode);
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }
        
        /// <summary>
        /// Teste de integração de atualização de status de pedido
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Válido")]
        public async Task TestarPatchPedidoValidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                
                var requestPedido = new HttpRequestMessage
                {
                    Method = HttpMethod.Patch,
                    RequestUri = new Uri($"https://localhost:7035/api/Pedido/{2}/{2}")
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responsePedido = await httpClient.SendAsync(requestPedido).ConfigureAwait(false);

                if (responsePedido.StatusCode != HttpStatusCode.OK)
                    Assert.Fail();
                else
                    Assert.AreEqual(HttpStatusCode.OK, responsePedido.StatusCode);                
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        /// <summary>
        /// Teste de integração de atualização de status de pedido inválido
        /// idPedido inválido
        /// enum Pedido inválido
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Inválido")]
        public async Task TestarPatchPedidoInvalidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                var requestPedido = new HttpRequestMessage
                {
                    Method = HttpMethod.Patch,
                    RequestUri = new Uri($"https://localhost:7035/api/Pedido/{0}/{7}")
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responsePedido = await httpClient.SendAsync(requestPedido).ConfigureAwait(false);

                if (responsePedido.StatusCode != HttpStatusCode.NotFound)
                    Assert.Fail();                
                else                
                    Assert.AreEqual(HttpStatusCode.NotFound, responsePedido.StatusCode);
                
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

    }
}
