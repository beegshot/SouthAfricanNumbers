using SouthAfricanNumbers.Shared;

namespace SouthAfricanNumbers.Client.Services
{
    public interface INumberService
    {
        Task<List<Number>> GetNumbers();

        Task<Number> GetNumberById(Guid Id);

        Task<Number> EditNumber(Number request);
    }
}
