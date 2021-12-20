using Microsoft.AspNetCore.Mvc.Testing;
using netbullAPI.Security.ViewModels;
using netbullAPI_Testes.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace netbullAPI_Testes.Uitl
{
    public class RequestLoginTeste
    {
        public async Task<RetornoLogin> RetornaUsuLoginAsync(LoginUserViewModel usu)
        {
            var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder => { }); 
            
            var _Client = application.CreateClient();

            var jsonCorpo = JsonConvert.SerializeObject(usu);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7035/api/Conta/login"),
                Content = new StringContent(jsonCorpo, Encoding.UTF8, "application/json"),
            };

            var response = await _Client.SendAsync(request).ConfigureAwait(false);
            var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var retornoLogin = JsonConvert.DeserializeObject<RetornoLogin>(responseBody);

            return retornoLogin;
        }
    }
}
