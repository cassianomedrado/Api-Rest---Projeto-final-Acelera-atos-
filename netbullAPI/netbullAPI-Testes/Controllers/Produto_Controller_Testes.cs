using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using netbullAPI.Entidade;
using netbullAPI.Security.ViewModels;
using netbullAPI.ViewModels;
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
    [TestClass]
    public class Produto_Controller_Testes // TESTES DE INTEGRAÇÃO
    {
        /// <summary>
        /// Teste integração para buscar todos produtos para teste válido
        /// result.StatusCode == HttpStatusCode.OK && produtos.Count != 0
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Válido")]
        public async Task TesteGetAllUserValidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var _Client = application.CreateClient();

                LoginUserViewModel login = new LoginUserViewModel()
                {
                    user_nome = "cassiano",
                    user_accessKey = "123456"
                };

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                _Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var result = _Client.GetAsync("api/Produto").GetAwaiter().GetResult();
                var resultContent = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                List<Produto> produtos = JsonConvert.DeserializeObject<List<Produto>>(resultContent);

                if (result.StatusCode != HttpStatusCode.OK)
                {
                    Assert.Fail();
                }

                Assert.AreNotEqual(0, produtos.Count);
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        /// <summary>
        /// Teste integração para buscar produto por id para Válido
        /// result.StatusCode == HttpStatusCode.OK && produto != null
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Válido")]
        public async Task TesteGetPorIdVálidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var _Client = application.CreateClient();

                LoginUserViewModel login = new LoginUserViewModel()
                {
                    user_nome = "cassiano",
                    user_accessKey = "123456"
                };

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                _Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var result = _Client.GetAsync($"api/Produto/{1}").GetAwaiter().GetResult();
                var resultContent = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                Produto produto = JsonConvert.DeserializeObject<Produto>(resultContent);

                if (result.StatusCode != HttpStatusCode.OK)
                {
                    Assert.Fail();
                }

                Assert.AreNotEqual(null, produto);
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        /// <summary>
        /// Teste integração para buscar produto por id para Inválido
        /// result.StatusCode == HttpStatusCode.NotFound && produto.error.Count == 0
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Inválido")]
        public async Task TesteGetPorIdInválidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var _Client = application.CreateClient();

                LoginUserViewModel login = new LoginUserViewModel()
                {
                    user_nome = "cassiano",
                    user_accessKey = "123456"
                };

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                _Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var result = _Client.GetAsync($"api/Produto/{0}").GetAwaiter().GetResult();
                var resultContent = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                RetornoNotFound produto = JsonConvert.DeserializeObject<RetornoNotFound>(resultContent);

                if (result.StatusCode != HttpStatusCode.NotFound)
                {
                    Assert.Fail();
                }

                Assert.AreNotEqual(0, produto.error.Count);
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        /// <summary>
        /// Teste integração para cadastrar um produto para válido
        /// result.StatusCode == HttpStatusCode.Created
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Válido")]
        public async Task TestePostVálidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
                 .WithWebHostBuilder(builder => { });

                var _Client = application.CreateClient();

                RegistrarProdutoViewModel postProduto = new RegistrarProdutoViewModel()
                {
                    produto_nome = "camisa produto teste",
                    produto_valor = 10
                };

                var jsonCorpo = JsonConvert.SerializeObject(postProduto);

                LoginUserViewModel login = new LoginUserViewModel()
                {
                    user_nome = "cassiano",
                    user_accessKey = "123456"
                };

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                _Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://localhost:7035/api/Produto"),
                    Content = new StringContent(jsonCorpo, Encoding.UTF8, "application/json"),
                };

                var response = await _Client.SendAsync(request).ConfigureAwait(false);
                var resultContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                if (response.StatusCode != HttpStatusCode.Created)
                {
                    Assert.Fail();
                }

                // DELETANDO TESTE CRIADO
                var prodCreated = JsonConvert.DeserializeObject<Produto>(resultContent);
                var result = _Client.DeleteAsync($"api/Produto/{prodCreated.produto_id}").GetAwaiter().GetResult();

                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        /// <summary>
        /// Teste integração para cadastrar um produto para Inválido
        /// result.StatusCode == HttpStatusCode.BadRequest
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Inválido")]
        public async Task TestePostInválidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
                 .WithWebHostBuilder(builder => { });

                var _Client = application.CreateClient();

                RegistrarProdutoViewModel postProduto = new RegistrarProdutoViewModel()
                {
                    produto_nome = "",
                    produto_valor = 10
                };

                var jsonCorpo = JsonConvert.SerializeObject(postProduto);

                LoginUserViewModel login = new LoginUserViewModel()
                {
                    user_nome = "cassiano",
                    user_accessKey = "123456"
                };

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                _Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://localhost:7035/api/Produto"),
                    Content = new StringContent(jsonCorpo, Encoding.UTF8, "application/json"),
                };

                var response = await _Client.SendAsync(request).ConfigureAwait(false);

                if (response.StatusCode != HttpStatusCode.BadRequest)
                {
                    Assert.Fail();
                }

                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            }
            catch (Exception ex)
            {
                string mensagem = ex.Message;
            }
        }

        /// <summary>
        /// Teste de integração para alterar nome de produto válido
        /// result.StatusCode == HttpStatusCode.OK
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Válido")]
        [TestCategory("Controller")]
        public async Task TestarPatchProdutoNomeValido()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                LoginUserViewModel login = new LoginUserViewModel()
                {
                    user_nome = "cassiano",
                    user_accessKey = "123456"
                };
                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                var jsonProdutoNome = JsonConvert.SerializeObject(new
                {
                    produto_id = 1,
                    produto_nome = "camisa produto teste muda nome"
                });

                var requestProduto = new HttpRequestMessage
                {
                    Method = HttpMethod.Patch,
                    RequestUri = new Uri($"https://localhost:7035/api/Produto/Nome/"),
                    Content = new StringContent(jsonProdutoNome, Encoding.UTF8, "application/json"),
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responseProduto = await httpClient.SendAsync(requestProduto).ConfigureAwait(false);
                var responseBodyProduto = await responseProduto.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (responseProduto.StatusCode != HttpStatusCode.OK)
                    Assert.Fail();

                Assert.AreEqual(HttpStatusCode.OK, responseProduto.StatusCode);

            }
            catch (Exception ex)
            {
                string mensagem = ex.Message;
            }
        }

        /// <summary>
        /// Teste integração para alterar nome de produto inválido
        /// result.StatusCode != HttpStatusCode.NotFound
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Invalido")]
        [TestCategory("Controller")]
        public async Task TestarPatchProdutoNomeInvalidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                LoginUserViewModel login = new LoginUserViewModel()
                {
                    user_nome = "cassiano",
                    user_accessKey = "123456"
                };
                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                var jsonProduto = JsonConvert.SerializeObject(new
                {
                    produto_id = 0,
                    produto_nome = "camisa produto testeee"
                });

                var requestProduto = new HttpRequestMessage
                {
                    Method = HttpMethod.Patch,
                    RequestUri = new Uri($"https://localhost:7035/api/Produto/Nome/"),
                    Content = new StringContent(jsonProduto, Encoding.UTF8, "application/json"),
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responseProduto = await httpClient.SendAsync(requestProduto).ConfigureAwait(false);
                var responseBodyProduto = await responseProduto.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (responseProduto.StatusCode != HttpStatusCode.NotFound)
                {
                    Assert.Fail();
                }

                Assert.AreEqual(HttpStatusCode.NotFound, responseProduto.StatusCode);

            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        /// <summary>
        /// Teste Integração para alterar Valor de Produto Válido
        /// result.StatusCode != HttpStatusCode.OK
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Válido")]
        [TestCategory("Controller")]
        public async Task TestarPatchProdutoValorValido()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                LoginUserViewModel login = new LoginUserViewModel()
                {
                    user_nome = "cassiano",
                    user_accessKey = "123456"
                };
                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                var jsonProduto = JsonConvert.SerializeObject(new
                {
                    produto_id = 1,
                    produto_valor = 10000
                });

                var requestProduto = new HttpRequestMessage
                {
                    Method = HttpMethod.Patch,
                    RequestUri = new Uri($"https://localhost:7035/api/Produto/Valor"),
                    Content = new StringContent(jsonProduto, Encoding.UTF8, "application/json"),
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responseProduto = await httpClient.SendAsync(requestProduto).ConfigureAwait(false);

                if (responseProduto.StatusCode != HttpStatusCode.OK)
                    Assert.Fail();

                Assert.AreEqual(HttpStatusCode.OK, responseProduto.StatusCode);
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        /// <summary>
        /// Teste integração para alterar valor de produto inválido
        /// result.StatusCode != HttpStatusCode.NotFound
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Invalido")]
        [TestCategory("Controller")]
        public async Task TestarPatchProdutoValorInvalidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                LoginUserViewModel login = new LoginUserViewModel()
                {
                    user_nome = "cassiano",
                    user_accessKey = "123456"
                };
                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                var jsonProduto = JsonConvert.SerializeObject(new
                {
                    produto_id = 0,
                    produto_valor = 10
                });

                var requestProduto = new HttpRequestMessage
                {
                    Method = HttpMethod.Patch,
                    RequestUri = new Uri($"https://localhost:7035/api/Produto/Valor"),
                    Content = new StringContent(jsonProduto, Encoding.UTF8, "application/json"),
                };

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responseProduto = await httpClient.SendAsync(requestProduto).ConfigureAwait(false);

                if (responseProduto.StatusCode != HttpStatusCode.NotFound)
                    Assert.Fail();

                Assert.AreEqual(HttpStatusCode.NotFound, responseProduto.StatusCode);
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        /// <summary>
        /// Teste integração delete produto valido
        /// result.StatusCode != HttpStatusCode.OK
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Válido")]
        [TestCategory("Controller")]
        public async Task TestarDeleteProdutoValidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                LoginUserViewModel login = new LoginUserViewModel()
                {
                    user_nome = "cassiano",
                    user_accessKey = "123456"
                };

                RegistrarProdutoViewModel postProduto = new RegistrarProdutoViewModel()
                {
                    produto_nome = "camisa teste",
                    produto_valor = 10
                };

                var jsonCorpo = JsonConvert.SerializeObject(postProduto);

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var requestProdutoPost = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://localhost:7035/api/Produto"),
                    Content = new StringContent(jsonCorpo, Encoding.UTF8, "application/json"),
                };

                var response = await httpClient.SendAsync(requestProdutoPost).ConfigureAwait(false);
                var resultContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                var produto = JsonConvert.DeserializeObject<Produto>(resultContent);
                var responseProdutoDelete = httpClient.DeleteAsync($"api/Produto/{produto.produto_id}").GetAwaiter().GetResult();

                if (responseProdutoDelete.StatusCode != HttpStatusCode.OK)
                {
                    Assert.Fail();
                }
                else
                {
                    Assert.AreEqual(HttpStatusCode.OK, responseProdutoDelete.StatusCode);
                }
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        /// <summary>
        /// Teste integração delete produto invalido
        /// result.StatusCode != HttpStatusCode.NotFound
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Invalido")]
        [TestCategory("Controller")]
        public async Task TestarDeleteProdutoInvalidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                LoginUserViewModel login = new LoginUserViewModel()
                {
                    user_nome = "cassiano",
                    user_accessKey = "123456"
                };
                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);
                var responseProdutoDelete = httpClient.DeleteAsync($"api/Produto/delete/{0}").GetAwaiter().GetResult();

                if (responseProdutoDelete.StatusCode != HttpStatusCode.NotFound)
                {
                    Assert.Fail();
                }
                Assert.AreEqual(HttpStatusCode.NotFound, responseProdutoDelete.StatusCode);

            }
            catch (Exception ex)
            {
                string mensagem = ex.Message;
            }
        }
    }
}
