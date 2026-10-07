using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace chatApp.Models
{
    public class MakingHttpRequest
    {
        public interface IserviceTTS
        {
            Task<string> Get(string Name);
        }

        public class serviceTTS : IserviceTTS
        {
            private HttpClient _httpClient;

            public serviceTTS(HttpClient httpClient)
            {
                _httpClient = httpClient;
            }

            public async  Task<string> Get(string Name)
            {
                //ApiKEy
                string APIURL=$"?q={Name}";
                var response = await _httpClient.GetAsync(APIURL);
                var data=   await response.Content.ReadAsStringAsync();
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
