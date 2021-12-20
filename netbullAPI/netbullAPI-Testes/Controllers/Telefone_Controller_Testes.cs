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
    [TestClass]
    public class Telefone_Controller_Testes
    {
        public LoginUserViewModel login = new LoginUserViewModel() { user_nome = "bruna", user_accessKey = "123456" };

        /// <summary>
        /// Teste integração de busca de telefones para um cliente inválido
        /// id_pessoa_invalido = 0
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Inválido")]
        public async Task TestarTelefoneByClienteInvalidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                // Requisição para pegar os telefones
                var requestTelefone = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://localhost:7035/api/Telefone/{0}"),
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responseTelefone = await httpClient.SendAsync(requestTelefone).ConfigureAwait(false);
                var responseBodytelefone = await responseTelefone.Content.ReadAsStringAsync().ConfigureAwait(false);
                var retornoTelefone = JsonConvert.DeserializeObject<RetornoNotFound>(responseBodytelefone);

                if (retornoTelefone.status != HttpStatusCode.NotFound && retornoTelefone.status != HttpStatusCode.BadRequest)
                    Assert.Fail();

                Assert.AreNotEqual(0, retornoTelefone.error?.Count); 
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        /// <summary>
        /// Teste integração de busca de telefones para um cliente válido
        /// id_pessoa_valido = 1
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Válido")]
        public async Task TestarTelefoneByClienteValidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                // Requisição para pegar os telefones
                var requestTelefone = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://localhost:7035/api/Telefone/{2}"),
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responseTelefone = await httpClient.SendAsync(requestTelefone).ConfigureAwait(false);
                var responseBodytelefone = await responseTelefone.Content.ReadAsStringAsync().ConfigureAwait(false);
                var retornoTelefone = JsonConvert.DeserializeObject<RetornoGetTelefone>(responseBodytelefone);

                if (retornoTelefone.status == HttpStatusCode.NotFound && retornoTelefone.status == HttpStatusCode.BadRequest)
                    Assert.Fail();

                Assert.AreNotEqual(0, retornoTelefone.telefones.ToList().Count);
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        /// <summary>
        /// Teste integração de post de novo telefone inválido
        /// telefone_invalido = 0
        /// telefone_invalido = 12345
        /// id_pessoa_invalido = 0
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Inválido")]
        public async Task TestarPostInvalidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                // Requisição para pegar os telefones
                var jsonTelefone = JsonConvert.SerializeObject(new 
                {
                    telefone_idPessoa = 1,
                    telefone_numero = 12345,
                });

                var requestTelefone = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"https://localhost:7035/api/Telefone/"),
                    Content = new StringContent(jsonTelefone, Encoding.UTF8, "application/json"),
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responseTelefone = await httpClient.SendAsync(requestTelefone).ConfigureAwait(false);
                var responseBodytelefone = await responseTelefone.Content.ReadAsStringAsync().ConfigureAwait(false);
                var retornoTelefone = JsonConvert.DeserializeObject<RetornoNotFound>(responseBodytelefone);

                if (retornoTelefone.status != HttpStatusCode.NotFound && retornoTelefone.status != HttpStatusCode.BadRequest)
                    Assert.Fail();

                Assert.AreNotEqual(0, retornoTelefone.error.Count);
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        /// <summary>
        /// Teste integração de post de novo telefone válido
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Válido")]
        public async Task TestarPostValidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                // Requisição para pegar os telefones
                var jsonTelefone = JsonConvert.SerializeObject(new
                {
                    telefone_idPessoa = 1,
                    telefone_numero = 123456789,
                });

                var requestTelefone = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"https://localhost:7035/api/Telefone/"),
                    Content = new StringContent(jsonTelefone, Encoding.UTF8, "application/json"),
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responseTelefone = await httpClient.SendAsync(requestTelefone).ConfigureAwait(false);
                var responseBodytelefone = await responseTelefone.Content.ReadAsStringAsync().ConfigureAwait(false);
                var retornoTelefone = JsonConvert.DeserializeObject<Telefone>(responseBodytelefone);

                if (responseTelefone.StatusCode == HttpStatusCode.NotFound || responseTelefone.StatusCode == HttpStatusCode.BadRequest)
                    Assert.Fail();
                else if (responseTelefone.StatusCode != HttpStatusCode.Created)
                    Assert.Fail();

                Assert.AreNotEqual(null, retornoTelefone);
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        /// <summary>
        /// Teste integração de put de novo telefone inválido
        /// id_telefone válido
        /// num_telefone válido
        /// pesso_id vinculada inválida
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Inválido")]
        public async Task TestarPutInvalidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                // Requisição para pegar os telefones
                //Numero válido e atribuicao com pessoa invalido
                var jsonTelefone = JsonConvert.SerializeObject(new Telefone()
                {
                    telefone_id = 1,
                    telefone_idPessoa = 2,
                    telefone_numero = 111111111,
                });

                var requestTelefone = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"https://localhost:7035/api/Telefone/"),
                    Content = new StringContent(jsonTelefone, Encoding.UTF8, "application/json"),
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responseTelefone = await httpClient.SendAsync(requestTelefone).ConfigureAwait(false);
                var responseBodytelefone = await responseTelefone.Content.ReadAsStringAsync().ConfigureAwait(false);
                var retornoTelefone = JsonConvert.DeserializeObject<RetornoNotFound>(responseBodytelefone);

                if (retornoTelefone.status != HttpStatusCode.NotFound && retornoTelefone.status != HttpStatusCode.BadRequest)
                    Assert.Fail();

                Assert.AreNotEqual(0, retornoTelefone.error?.Count);
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        /// <summary>
        /// Teste integração de put de novo telefone válido
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Válido")]
        public async Task TestarPutValidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                // Requisição para pegar os telefones
                var jsonTelefone = JsonConvert.SerializeObject(new Telefone()
                {
                    telefone_id = 1,
                    telefone_idPessoa = 1,
                    telefone_numero = 111111111,
                });

                var requestTelefone = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"https://localhost:7035/api/Telefone/"),
                    Content = new StringContent(jsonTelefone, Encoding.UTF8, "application/json"),
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responseTelefone = await httpClient.SendAsync(requestTelefone).ConfigureAwait(false);
                var responseBodytelefone = await responseTelefone.Content.ReadAsStringAsync().ConfigureAwait(false);
                var retornoTelefone = JsonConvert.DeserializeObject<Telefone>(responseBodytelefone);

                if (responseTelefone.StatusCode == HttpStatusCode.NotFound || responseTelefone.StatusCode== HttpStatusCode.BadRequest)
                    Assert.Fail();
                else if(responseTelefone.StatusCode != HttpStatusCode.OK)
                    Assert.Fail();

                Assert.AreNotEqual(null, retornoTelefone);
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        /// <summary>
        /// Teste de integração deleção de telefone inválido
        /// id_telefone_invalido
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Inválido")]
        public async Task TestarDeleteInvalidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                // Requisição para pegar os telefones
                var requestTelefone = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri($"https://localhost:7035/api/Telefone/{0}"),
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responseTelefone = await httpClient.SendAsync(requestTelefone).ConfigureAwait(false);
                var responseBodytelefone = await responseTelefone.Content.ReadAsStringAsync().ConfigureAwait(false);
                var retornoTelefone = JsonConvert.DeserializeObject<RetornoNotFound>(responseBodytelefone);

                if (retornoTelefone.status != HttpStatusCode.NotFound && retornoTelefone.status != HttpStatusCode.BadRequest)
                    Assert.Fail();

                Assert.AreNotEqual(0, retornoTelefone.error.Count);
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        /// <summary>
        /// Teste de integração deleção de telefone válido
        /// id_telefone_valido
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Controller", "Válido")]
        public async Task TestarDeleteValidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                //Inclusao de telefone
                var jsonTelefoneInsercao = JsonConvert.SerializeObject(new
                {
                    telefone_idPessoa = 1,
                    telefone_numero = 123456789,
                });

                var requestTelefoneInsercao = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"https://localhost:7035/api/Telefone/"),
                    Content = new StringContent(jsonTelefoneInsercao, Encoding.UTF8, "application/json"),
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);
                var responseTelefoneInsercao = await httpClient.SendAsync(requestTelefoneInsercao).ConfigureAwait(false);
                var responseBodytelefoneInsercao = await responseTelefoneInsercao.Content.ReadAsStringAsync().ConfigureAwait(false);
                var retornoTelefoneInsercao = JsonConvert.DeserializeObject<Telefone>(responseBodytelefoneInsercao);

                // Delecao do telefone 
                var requestTelefone = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri($"https://localhost:7035/api/Telefone/{retornoTelefoneInsercao.telefone_id}"),
                };

                var responseTelefone = await httpClient.SendAsync(requestTelefone).ConfigureAwait(false);
                var responseBodytelefone = await responseTelefone.Content.ReadAsStringAsync().ConfigureAwait(false);
                var retornoTelefone = JsonConvert.DeserializeObject<RetornoDeleteTelefone>(responseBodytelefone);

                if (responseTelefone.StatusCode == HttpStatusCode.NotFound || responseTelefone.StatusCode == HttpStatusCode.BadRequest)
                    Assert.Fail();
                else if (responseTelefone.StatusCode != HttpStatusCode.OK)
                    Assert.Fail();

                Assert.AreEqual(true, retornoTelefone.sucesso);
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }
    }
}