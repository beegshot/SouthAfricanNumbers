using SouthAfricanNumbers.Shared;

namespace SouthAfricanNumbers.Client.Services
{
    public interface INumberService
    {
        Task<List<Number>> GetNumbers();

        Task<Number> GetNumberById(int Id);
    }
}
