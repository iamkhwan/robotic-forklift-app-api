using AutoMapper;
using ForkliftAPI.Application.DTOs;
using ForkliftAPI.Application.Interfaces;
using ForkliftAPI.Domain.Entities;

namespace ForkliftAPI.Application.Services
{
    public class ForkliftService : IForkliftService
    {
        private readonly IForkliftRepository _repository;
        private readonly IMapper _mapper;

        public ForkliftService(IForkliftRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ForkliftDto>> GetAllForkliftsAsync()
        {
            var forklifts = await _repository.GetAllAsync();
            var forkliftsDto = _mapper.Map<List<ForkliftDto>>(forklifts);
            return forkliftsDto;
        }

        public async Task<int> UploadForkliftsAsync(List<ForkliftDto> forkliftsDto)
        {
            var forklifts = _mapper.Map<List<Forklift>>(forkliftsDto);

            var incomingModelNumbers = forklifts.Select(f => f.ModelNumber).ToList();
            var existingModelNumbers = await _repository.GetExistingModelNumbersAsync(incomingModelNumbers);

            if (existingModelNumbers == null)
            {
                return 0;
            }

            var newForklifts = forklifts.Where(f => !existingModelNumbers.Contains(f.ModelNumber)).ToList();

            if (newForklifts.Any())
            {
                await _repository.AddRangeAsync(newForklifts);
            }

            return newForklifts.Count;
        }

        public async Task ClearAllForkliftsAsync()
        {
            await _repository.ClearAllForkliftsAsync();
        }
    }
}