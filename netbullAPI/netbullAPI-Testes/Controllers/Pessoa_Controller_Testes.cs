using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using netbullAPI.Security.ViewModels;
using netbullAPI_Testes.Models;
using netbullAPI_Testes.Uitl;
using Newtonsoft.Json;
using System;
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
    public class Pessoa_Controller_Testes
    {
        public LoginUserViewModel login = new LoginUserViewModel() { user_nome = "cassiano", user_accessKey = "123456" };

        [Fact]
        [Trait("Controller", "Invalido")]
        public async Task TestarGetPessoaInvalidaAsync()
        {
            try
            {
                LoginUserViewModel login = new LoginUserViewModel() { user_nome = "cassiano", user_accessKey = "123456" };

                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var result = httpClient.GetAsync($"api/Pessoa/{0}").GetAwaiter().GetResult();
                var resultContent = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var retornoPessoa = JsonConvert.DeserializeObject<RetornoErrorPessoa>(resultContent);

                if (result.StatusCode != HttpStatusCode.NotFound)
                {
                    Assert.Fail();
                }
                Assert.AreNotEqual(0, retornoPessoa.Errors.Count());

            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }


        [Fact]
        [Trait("Controller", "Válido")]
        [TestCategory("Controller")]
        public async Task TestarGetPessoaValidaAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                // Requisição para pegar os telefones
                var requestPessoa = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://localhost:7035/api/Pessoa/{1}"),
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responsePessoa = await httpClient.SendAsync(requestPessoa).ConfigureAwait(false);
                

                if (responsePessoa.StatusCode == HttpStatusCode.NotFound)
                {
                    Assert.Fail();
                }
                else
                {
                    Assert.AreEqual(responsePessoa.StatusCode, HttpStatusCode.OK);
                }

            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        [Fact]
        [Trait("Controller", "Invalido")]
        [TestCategory("Controller")]
        public async Task TestarPostPessoaInvalidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);
                               
                var jsonPessoa = JsonConvert.SerializeObject(new
                {
                    pessoa_id = 1,
                    pessoa_documento = 12345,
                    pessoa_nome = "",
                    pessoa_tipopessoa = 0
                });

                var requestPessoa = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"https://localhost:7035/api/Pessoa"),
                    Content = new StringContent(jsonPessoa, Encoding.UTF8, "application/json"),
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responsePessoa = await httpClient.SendAsync(requestPessoa).ConfigureAwait(false);

                if (responsePessoa.StatusCode != HttpStatusCode.BadRequest)
                {
                    Assert.Fail();
                }
                Assert.AreEqual(HttpStatusCode.BadRequest, responsePessoa.StatusCode);
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        [Fact]
        [Trait("Controller", "Válido")]
        [TestCategory("Controller")]
        public async Task TestarPostPessoaValidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                // Requisição para pegar os telefones
                var jsonPessoa = JsonConvert.SerializeObject(new
                {
                    pessoa_id = 5,
                    pessoa_documento = 12345,
                    pessoa_nome = "teste",
                    pessoa_tipopessoa = 0
                });

                var requestPessoa = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"https://localhost:7035/api/Pessoa"),
                    Content = new StringContent(jsonPessoa, Encoding.UTF8, "application/json"),
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responsePessoa = await httpClient.SendAsync(requestPessoa).ConfigureAwait(false);
                

                if (responsePessoa.StatusCode != HttpStatusCode.OK)
                {
                    Assert.Fail();
                }
                else
                {
                    Assert.AreEqual(HttpStatusCode.OK, responsePessoa.StatusCode);
                }
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        [Fact]
        [Trait("Controller", "Invalido")]
        [TestCategory("Controller")]
        public async Task TestarPutPessoaInvalidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                // Requisição para pegar os telefones
                var jsonPessoa = JsonConvert.SerializeObject(new
                {
                    pessoa_id = 1,
                    pessoa_documento = 12345,
                    pessoa_nome = "",
                    pessoa_tipopessoa = 0
                });

                var requestPessoa = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"https://localhost:7035/api/Pessoa"),
                    Content = new StringContent(jsonPessoa, Encoding.UTF8, "application/json"),
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responsePessoa = await httpClient.SendAsync(requestPessoa).ConfigureAwait(false);
                var responseBodyPessoa = await responsePessoa.Content.ReadAsStringAsync().ConfigureAwait(false);
                var retornoPessoa = JsonConvert.DeserializeObject<RetornoNotFound>(responseBodyPessoa);

                if (retornoPessoa.status != HttpStatusCode.NotFound && retornoPessoa.status != HttpStatusCode.BadRequest)
                {
                    Assert.Fail();
                }
                else
                {
                    Assert.AreEqual(HttpStatusCode.NotFound, responsePessoa.StatusCode);
                }
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        [Fact]
        [Trait("Controller", "Válido")]
        [TestCategory("Controller")]
        public async Task TestarPutPessoaValidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                // Requisição para pegar os telefones
                var jsonPessoa = JsonConvert.SerializeObject(new
                {
                    pessoa_id = 1,
                    pessoa_documento = 12345,
                    pessoa_nome = "Teste",
                    pessoa_tipopessoa = 0
                });

                var requestPessoa = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"https://localhost:7035/api/Pessoa"),
                    Content = new StringContent(jsonPessoa, Encoding.UTF8, "application/json"),
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responsePessoa = await httpClient.SendAsync(requestPessoa).ConfigureAwait(false);
                var responseBodyPessoa = await responsePessoa.Content.ReadAsStringAsync().ConfigureAwait(false);
                var retornoPessoa = JsonConvert.DeserializeObject<RetornoNotFound>(responseBodyPessoa);

                if (responsePessoa.StatusCode == HttpStatusCode.NotFound || responsePessoa.StatusCode == HttpStatusCode.BadRequest)
                {
                    Assert.Fail();
                }
                else
                {
                    Assert.AreNotEqual(null, retornoPessoa);
                }
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        [Fact]
        [Trait("Controller", "Invalido")]
        [TestCategory("Controller")]
        public async Task TestarDeletePessoaInvalidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                
                var jsonPessoa = JsonConvert.SerializeObject(new
                {
                    pessoa_id = 0
                });

                var requestPessoa = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri($"https://localhost:7035/api/Pessoa/{0}"),
                    Content = new StringContent(jsonPessoa, Encoding.UTF8, "application/json"),
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responsePessoa = await httpClient.SendAsync(requestPessoa).ConfigureAwait(false);
                

                if (responsePessoa.StatusCode != HttpStatusCode.NotFound)
                {
                    Assert.Fail();
                }
                else
                {
                    Assert.AreEqual(HttpStatusCode.NotFound, responsePessoa.StatusCode);
                }
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }

        [Fact]
        [Trait("Controller", "Válido")]
        [TestCategory("Controller")]
        public async Task TestarDeletePessoaValidoAsync()
        {
            try
            {
                var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { });

                var httpClient = application.CreateClient();

                var usuario = await new RequestLoginTeste().RetornaUsuLoginAsync(login);

                // Requisição para pegar os telefones
                var jsonPessoa = JsonConvert.SerializeObject(new
                {
                    pessoa_id = 11
                });

                var requestPessoa = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri($"https://localhost:7035/api/Pessoa/11"),
                    Content = new StringContent(jsonPessoa, Encoding.UTF8, "application/json"),
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);

                var responsePessoa = await httpClient.SendAsync(requestPessoa).ConfigureAwait(false);

                if (responsePessoa.StatusCode != HttpStatusCode.OK)
                {
                    Assert.Fail();
                }
                else
                {
                    Assert.AreEqual(HttpStatusCode.OK, responsePessoa.StatusCode);
                }
            }
            catch (Exception ex)
            {
                string menssage = ex.Message;
            }
        }
    }
}
