using SouthAfricanNumbers.Shared;
using System.Net.Http.Json;

namespace SouthAfricanNumbers.Client.Services
{
    public class NumberService : INumberService
    {
        private readonly HttpClient _http;

        public NumberService(HttpClient http)
        {
            _http = http;
        }

        public async Task<Number> GetNumberById(int Id)
        {
            Number CurrentNumber = await _http.GetFromJsonAsync<Number>($"api/NumberList/{Id}");
            return CurrentNumber;
        }

        public async Task<List<Number>> GetNumbers()
        {
            List<Number> CurrentNumbers = await _http.GetFromJsonAsync<List<Number>>("api/NumberList");
            return CurrentNumbers;
        }
    }
}
