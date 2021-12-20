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
    public class Item_Controller_Testes
    {
        public LoginUserViewModel login = new LoginUserViewModel() { user_nome = "cassiano", user_accessKey = "123456" };

        [Fact]
        [Trait("Controller", "Válido")]
        public async Task TestarGetItemByIdValidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                var requestItem = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://localhost:7035/api/Item/{1}"),
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responseItem = await httpClient.SendAsync(requestItem).ConfigureAwait(false);


                if (responseItem.StatusCode != HttpStatusCode.OK)
                {
                    Assert.Fail();
                }
                else
                {
                    Assert.AreEqual(responseItem.StatusCode, HttpStatusCode.OK);
                }

            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }
        [Fact]
        [Trait("Controller", "Inválido")]
        public async Task TestarGetItemByIdInvalidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                var requestItem = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://localhost:7035/api/Item/{0}"), //Falta trocar request
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responseItem = await httpClient.SendAsync(requestItem).ConfigureAwait(false);


                if (responseItem.StatusCode != HttpStatusCode.NotFound)
                {
                    Assert.Fail();
                }
                else
                {
                    Assert.AreEqual(responseItem.StatusCode, HttpStatusCode.NotFound);
                }

            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }
        [Fact]
        [Trait("Controller", "Válido")] 
        public async Task TestarPostItemValidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                var jsonItem = JsonConvert.SerializeObject(new
                {
                    item_id = 1,
                    item_valor = 14,
                    item_qtdproduto = 2,
                    item_idPedido = 1,
                    item_idProduto = 2,
                });

                var requestItem = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"https://localhost:7035/api/Item"),
                    Content = new StringContent(jsonItem, Encoding.UTF8, "application/json"),
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

                var jsonItem = JsonConvert.SerializeObject(new
                {
                    item_id = 0,
                    item_valor = 0,
                    item_qtdproduto = 0,
                    item_idPedido = 0,
                    item_idProduto = 0,
                });

                var requestItem = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"https://localhost:7035/api/Item"),
                    Content = new StringContent(jsonItem, Encoding.UTF8, "application/json"),
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responseItem = await httpClient.SendAsync(requestItem).ConfigureAwait(false);

                if (responseItem.StatusCode != HttpStatusCode.NotFound)
                {
                    Assert.Fail();
                }
                Assert.AreEqual(HttpStatusCode.NotFound, responseItem.StatusCode);
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }
        [Fact]
        [Trait("Controller", "Válido")]
        public async Task TestarPatchItemValidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                var jsonItem = JsonConvert.SerializeObject(new
                {
                    item_qtdproduto = 7,
                });

                var requestItem = new HttpRequestMessage
                {
                    Method = HttpMethod.Patch,
                    RequestUri = new Uri($"https://localhost:7035/api/Item/{3}"),
                    Content = new StringContent(jsonItem, Encoding.UTF8, "application/json"),
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responseItem = await httpClient.SendAsync(requestItem).ConfigureAwait(false);


                if (responseItem.StatusCode != HttpStatusCode.OK)
                {
                    Assert.Fail();
                }
                else
                {
                    Assert.AreEqual(HttpStatusCode.OK, responseItem.StatusCode);
                }
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }
        [Fact]
        [Trait("Controller", "Inválido")]
        public async Task TestarPatchItemInvalidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                var jsonItem = JsonConvert.SerializeObject(new
                {
                    item_qtdproduto = 10,
                });

                var requestItem = new HttpRequestMessage
                {
                    Method = HttpMethod.Patch,
                    RequestUri = new Uri($"https://localhost:7035/api/Item/{0}"),
                    Content = new StringContent(jsonItem, Encoding.UTF8, "application/json"),
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responseItem = await httpClient.SendAsync(requestItem).ConfigureAwait(false);


                if (responseItem.StatusCode != HttpStatusCode.NotFound)
                {
                    Assert.Fail();
                }
                else
                {
                    Assert.AreEqual(HttpStatusCode.NotFound, responseItem.StatusCode);
                }
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }
        [Fact]
        [Trait("Controller", "Válido")]
        public async Task TestarDeleteItemByIdValidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                var jsonItemInsercao = JsonConvert.SerializeObject(new
                {
                    item_id = 1,
                    item_idPedido = 1,
                    item_qtdproduto = 3,
                    item_idProduto = 2,
                    item_valor = 2*3
                });

                var requestItemInsercao = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"https://localhost:7035/api/Item/"),
                    Content = new StringContent(jsonItemInsercao, Encoding.UTF8, "application/json"),
                };

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);
                var responseItemInsercao = await httpClient.SendAsync(requestItemInsercao).ConfigureAwait(false);
                var responseBodyitemInsercao = await responseItemInsercao.Content.ReadAsStringAsync().ConfigureAwait(false);
                var retornoItemInsercao = JsonConvert.DeserializeObject<Item>(responseBodyitemInsercao);

                var requestItem = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri($"https://localhost:7035/api/Item/{retornoItemInsercao.item_id}"),
                };

                var responseItem = await httpClient.SendAsync(requestItem).ConfigureAwait(false);


                if (responseItem.StatusCode != HttpStatusCode.OK)
                {
                    Assert.Fail();
                }
                else
                {
                    Assert.AreEqual(responseItem.StatusCode, HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }
        [Fact]
        [Trait("Controller", "Inválido")]
        public async Task TestarDeleteItemByIdInvalidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                var requestItem = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri($"https://localhost:7035/api/Item/{0}"), //Falta trocar request
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responseItem = await httpClient.SendAsync(requestItem).ConfigureAwait(false);


                if (responseItem.StatusCode != HttpStatusCode.NotFound)
                {
                    Assert.Fail();
                }
                else
                {
                    Assert.AreEqual(responseItem.StatusCode, HttpStatusCode.NotFound);
                }

            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }
    }
}
