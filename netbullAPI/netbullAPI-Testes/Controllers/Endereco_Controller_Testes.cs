using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using netbullAPI.Entidade;
using netbullAPI.Persistencia;
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
    public class Endereco_Controller_Testes
    {
        public LoginUserViewModel login = new LoginUserViewModel() { user_nome = "ale", user_accessKey = "123456" };

        /// <summary>
        /// Teste integração de busca de endereços para um cliente válido
        /// responseEndereco.StatusCode == HttpStatusCode.OK
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Válido")]
        public async Task TestarEnderecoGetPorIdPessoaValidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                // Requisição para pegar os telefones
                var requestEndereco = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://localhost:7035/api/Endereco/{2}"),
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responseEndereco = await httpClient.SendAsync(requestEndereco).ConfigureAwait(false);
                var responseBodytelefone = await responseEndereco.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (responseEndereco.StatusCode != HttpStatusCode.OK)
                    Assert.Fail();

                Assert.AreEqual(HttpStatusCode.OK, responseEndereco.StatusCode);
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        /// <summary>
        /// Teste integração de busca de endereços para um cliente Inválido
        /// responseEndereco.StatusCode == HttpStatusCode.NotFound
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Inválido")]
        public async Task TestarEnderecoGetPorIdPessoaInvalidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                var requestEndereco = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://localhost:7035/api/Endereco/{0}"),
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responseEndereco = await httpClient.SendAsync(requestEndereco).ConfigureAwait(false);
                var responseBodytelefone = await responseEndereco.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (responseEndereco.StatusCode != HttpStatusCode.NotFound)
                    Assert.Fail();

                Assert.AreEqual(HttpStatusCode.NotFound, responseEndereco.StatusCode);
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        /// <summary>
        /// Teste integração de cadastro de endereço válido
        /// responseEndereco.StatusCode == HttpStatusCode.Created
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Válido")]
        public async Task TestarCadastrarNovoEnderecoValidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                RegistrarEnderecoViewModel endereco = new RegistrarEnderecoViewModel()
                {
                    endereco_logradouro = "LOGRADOUTO_TESTE",
                    endereco_numero = 111,
                    endereco_complemento = "CACA",
                    endereco_idpessoa = 2
                };

                var jsonCorpo = JsonConvert.SerializeObject(endereco);

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                var requestEndereco = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"https://localhost:7035/api/Endereco"),
                    Content = new StringContent(jsonCorpo, Encoding.UTF8, "application/json"),
                };

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responseEndereco = await httpClient.SendAsync(requestEndereco).ConfigureAwait(false);
                var responseBodytelefone = await responseEndereco.Content.ReadAsStringAsync().ConfigureAwait(false);

                // DELETANDO TESTE CRIADO
                var config = new ConfigurationBuilder()
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = config.GetConnectionString("NetBullConnection");

                var usuCreated = JsonConvert.DeserializeObject<Endereco>(responseBodytelefone);

                var contextOptions = new DbContextOptionsBuilder<netbullDBContext>()
                                .UseNpgsql(connectionString)
                                .Options;

                using var context = new netbullDBContext(contextOptions);

                var enderecoCriado = context.Enderecos.Where(end => end.endereco_logradouro == endereco.endereco_logradouro &&
                                                                              end.endereco_numero == endereco.endereco_numero &&
                                                                              end.endereco_complemento == endereco.endereco_complemento &&
                                                                              end.endereco_idpessoa == endereco.endereco_idpessoa).FirstOrDefault();

                usuCreated.endereco_id = enderecoCriado.endereco_id;

                var result = httpClient.DeleteAsync($"api/Endereco/{usuCreated.endereco_id}").GetAwaiter().GetResult();

                if (responseEndereco.StatusCode != HttpStatusCode.Created)
                    Assert.Fail();

                Assert.AreEqual(HttpStatusCode.Created, responseEndereco.StatusCode);
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        /// <summary>
        /// Teste integração de cadastro de endereço Inválido
        /// responseEndereco.StatusCode == HttpStatusCode.BadRequest
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Inválido")]
        public async Task TestarCadastrarNovoEnderecoInvalidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                RegistrarEnderecoViewModel endereco = new RegistrarEnderecoViewModel()
                {
                    endereco_logradouro = "",
                    endereco_numero = 0,
                    endereco_complemento = "",
                    endereco_idpessoa = 0
                };

                var jsonCorpo = JsonConvert.SerializeObject(endereco);

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                var requestEndereco = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"https://localhost:7035/api/Endereco"),
                    Content = new StringContent(jsonCorpo, Encoding.UTF8, "application/json"),
                };

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responseEndereco = await httpClient.SendAsync(requestEndereco).ConfigureAwait(false);
                var responseBodytelefone = await responseEndereco.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (responseEndereco.StatusCode != HttpStatusCode.BadRequest)
                    Assert.Fail();

                Assert.AreEqual(HttpStatusCode.BadRequest, responseEndereco.StatusCode);
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        /// <summary>
        /// Teste integração de atualiazção PUT de endereço Válido
        /// responseEndereco.StatusCode == HttpStatusCode.OK
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Válido")]
        public async Task TestaroAtualizaEnderecoValidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                AlterarEnderecoViewModel enderecoAtualizar = new AlterarEnderecoViewModel()
                {
                    endereco_logradouro = "testandoAlt",
                    endereco_numero = 929,
                    endereco_complemento = "testeAlterar",
                };

                var jsonCorpo = JsonConvert.SerializeObject(enderecoAtualizar);

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                var requestEndereco = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"https://localhost:7035/api/Endereco/{27}"),
                    Content = new StringContent(jsonCorpo, Encoding.UTF8, "application/json"),
                };

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responseEndereco = await httpClient.SendAsync(requestEndereco).ConfigureAwait(false);
                var responseBodytelefone = await responseEndereco.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (responseEndereco.StatusCode != HttpStatusCode.OK)
                    Assert.Fail();

                Assert.AreEqual(HttpStatusCode.OK, responseEndereco.StatusCode);
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        /// <summary>
        /// Teste integração de atualiazção PUT de endereço Inválido
        /// responseEndereco.StatusCode == HttpStatusCode.OK
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Inválido")]
        public async Task TestaroAtualizaEnderecoInvalidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                AlterarEnderecoViewModel enderecoAtualizar = new AlterarEnderecoViewModel()
                {
                    endereco_logradouro = "testandoAlt",
                    endereco_numero = 929,
                    endereco_complemento = "testeAlterar",
                };

                var jsonCorpo = JsonConvert.SerializeObject(enderecoAtualizar);

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                var requestEndereco = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"https://localhost:7035/api/Endereco/{1}"),
                    Content = new StringContent(jsonCorpo, Encoding.UTF8, "application/json"),
                };

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responseEndereco = await httpClient.SendAsync(requestEndereco).ConfigureAwait(false);
                var responseBodytelefone = await responseEndereco.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (responseEndereco.StatusCode != HttpStatusCode.NotFound)
                    Assert.Fail();

                Assert.AreEqual(HttpStatusCode.NotFound, responseEndereco.StatusCode);
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        /// <summary>
        /// Teste integração de atualiazção PATCH de endereço Válido
        /// responseEndereco.StatusCode == HttpStatusCode.OK
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Válido")]
        public async Task TestaroAtualizaEnderecoPatchValidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();
                string logradouro = "Teste Logradouro";

                var jsonCorpo = JsonConvert.SerializeObject(logradouro);

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                var requestEndereco = new HttpRequestMessage
                {
                    Method = HttpMethod.Patch,
                    RequestUri = new Uri($"https://localhost:7035/api/Endereco/{27}"),
                    Content = new StringContent(jsonCorpo, Encoding.UTF8, "application/json"),
                };

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responseEndereco = await httpClient.SendAsync(requestEndereco).ConfigureAwait(false);
                var responseBodytelefone = await responseEndereco.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (responseEndereco.StatusCode != HttpStatusCode.OK)
                    Assert.Fail();

                Assert.AreEqual(HttpStatusCode.OK, responseEndereco.StatusCode);
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        /// <summary>
        /// Teste integração de atualiazção PUT de endereço Inválido
        /// responseEndereco.StatusCode == HttpStatusCode.OK
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Inválido")]
        public async Task TestaroAtualizaEnderecoPatchInvalidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
                               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();
                string logradouro = "Teste Logradouro";

                var jsonCorpo = JsonConvert.SerializeObject(logradouro);

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                var requestEndereco = new HttpRequestMessage
                {
                    Method = HttpMethod.Patch,
                    RequestUri = new Uri($"https://localhost:7035/api/Endereco/{0}"),
                    Content = new StringContent(jsonCorpo, Encoding.UTF8, "application/json"),
                };

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responseEndereco = await httpClient.SendAsync(requestEndereco).ConfigureAwait(false);
                var responseBodytelefone = await responseEndereco.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (responseEndereco.StatusCode != HttpStatusCode.OK)
                    Assert.Fail();

                Assert.AreEqual(HttpStatusCode.OK, responseEndereco.StatusCode);
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        /// <summary>
        /// Teste de integração deleção de endereço válido
        /// id_endereco_valido
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Válido")]
        public async Task TestarDeleteEnderecoValidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                RegistrarEnderecoViewModel endereco = new RegistrarEnderecoViewModel()
                {
                    endereco_logradouro = "LOGRADOUTO_TESTE",
                    endereco_numero = 111,
                    endereco_complemento = "CACA",
                    endereco_idpessoa = 2
                };

                var jsonCorpo = JsonConvert.SerializeObject(endereco);

                var requestEndereco = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"https://localhost:7035/api/Endereco/"),
                    Content = new StringContent(jsonCorpo, Encoding.UTF8, "application/json"),
                };

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responseEndereco = await httpClient.SendAsync(requestEndereco).ConfigureAwait(false);
                var responseBodytelefone = await responseEndereco.Content.ReadAsStringAsync().ConfigureAwait(false);

                // DELETANDO TESTE CRIADO
                var config = new ConfigurationBuilder()
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = config.GetConnectionString("NetBullConnection");

                var usuCreated = JsonConvert.DeserializeObject<Endereco>(responseBodytelefone);

                var contextOptions = new DbContextOptionsBuilder<netbullDBContext>()
                                .UseNpgsql(connectionString)
                                .Options;

                using var context = new netbullDBContext(contextOptions);

                var enderecoCriado = context.Enderecos.Where(end => end.endereco_logradouro == endereco.endereco_logradouro &&
                                                                              end.endereco_numero == endereco.endereco_numero &&
                                                                              end.endereco_complemento == endereco.endereco_complemento &&
                                                                              end.endereco_idpessoa == endereco.endereco_idpessoa).FirstOrDefault();

                usuCreated.endereco_id = enderecoCriado.endereco_id;

                var result = httpClient.DeleteAsync($"api/Endereco/{usuCreated.endereco_id}").GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        /// <summary>
        /// Teste de integração deleção de endereço inválido
        /// id_endereco_invalido
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Válido")]
        public async Task TestarDeleteEnderecoInvalidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var result = httpClient.DeleteAsync($"api/Endereco/{0}").GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }
    }
}