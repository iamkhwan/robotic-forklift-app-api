using ForkliftAPI.Domain.Entities;

namespace ForkliftAPI.Application.Interfaces
{
    public interface IForkliftRepository
    {
        Task<List<Forklift>> GetAllAsync();
        Task AddRangeAsync(IEnumerable<Forklift> forklifts);
        Task<List<string>> GetExistingModelNumbersAsync(List<string> modelNumbers);
        Task ClearAllForkliftsAsync();
        Task<bool> ExistsAsync(string modelNumber);
    }
}