using Newtonsoft.Json;
using RouletteRecorder.Network.DungeonLogger.Structures;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RouletteRecorder.Network.DungeonLogger
{
    public interface IDungeonLoggerClient
    {
        Task<Response<Auth>> PostLogin(string password, string username);
        Task<object> PostRecord(int mazeId, string profKey);
        Task<Response<List<StatProf>>> GetStatProf();
        Task<Response<List<StatMaze>>> GetStatMaze();
    }

    public class DungeonLoggerClient : IDungeonLoggerClient
    {
        private readonly HttpClient _client;
        private readonly CookieContainer cookieContainer;

        public DungeonLoggerClient()
        {
            cookieContainer = new CookieContainer();
            _client = new HttpClient(new HttpClientHandler()
            {
                CookieContainer = cookieContainer
            })
            {
                BaseAddress = new System.Uri("https://dlog.luyulight.cn"),
            };
        }

        public async Task<Response<Auth>> PostLogin(string password, string username)
        {
            var response = await _client.PostAsync("/api/login", new StringContent(JsonConvert.SerializeObject(new
            {
                password,
                username
            }), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Response<Auth>>(content);
        }

        public async Task<object> PostRecord(int mazeId, string profKey)
        {
            var response = await _client.PostAsync("/api/record", new StringContent(JsonConvert.SerializeObject(new
            {
                mazeId,
                profKey
            }), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject(content);
        }

        public async Task<Response<List<StatProf>>> GetStatProf()
        {
            var response = await _client.GetAsync("/api/stat/prof");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Response<List<StatProf>>>(content);
        }

        public async Task<Response<List<StatMaze>>> GetStatMaze()
        {
            var response = await _client.GetAsync("/api/stat/maze");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Response<List<StatMaze>>>(content);
        }
    }
}
