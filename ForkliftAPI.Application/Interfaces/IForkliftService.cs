using ForkliftAPI.Application.DTOs;
using ForkliftAPI.Domain.Entities;

namespace ForkliftAPI.Application.Interfaces
{
    public interface IForkliftService
    {
        Task<List<ForkliftDto>> GetAllForkliftsAsync();
        Task<int> UploadForkliftsAsync(List<ForkliftDto> forkliftsDto);
        Task ClearAllForkliftsAsync();
    }
}