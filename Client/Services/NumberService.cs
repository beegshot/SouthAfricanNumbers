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

        public async Task<List<UpFileResponse>> UploadFile(UpFile request)
        {
            var result = await _http.PostAsJsonAsync("api/NumberList/Upload", request);
            return await result.Content.ReadFromJsonAsync<List<UpFileResponse>>();
        }

        public async Task<NumberResponse> EditNumber(Number request)
        {
            var result = await _http.PostAsJsonAsync("api/NumberList", request);
            return await result.Content.ReadFromJsonAsync<NumberResponse>();
        }

        public async Task<Number> GetNumberById(Guid Id)
        {
            var result = await _http.GetAsync($"api/NumberList/{Id}");
            if(result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var message = await result.Content.ReadAsStringAsync();
                return new Number { Id = Guid.Empty, PhoneNumber = message };
            }
            else
            {
                return await result.Content.ReadFromJsonAsync<Number>();
            }
        }

        public async Task<List<Number>> GetNumbers()
        {
            List<Number> CurrentNumbers = await _http.GetFromJsonAsync<List<Number>>("api/NumberList");
            return CurrentNumbers;
        }
    }
}
