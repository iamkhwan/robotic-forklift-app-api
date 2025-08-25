using ForkliftAPI.Application.Interfaces;
using ForkliftAPI.Domain.Entities;
using ForkliftAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ForkliftAPI.Infrastructure.Repositories
{
    public class ForkliftRepository : IForkliftRepository
    {
        private readonly ForkliftContext _context;

        public ForkliftRepository(ForkliftContext context)
        {
            _context = context;
        }

        public async Task<List<Forklift>> GetAllAsync()
        {
            return await _context.Forklifts.ToListAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Forklift> forklifts)
        {
            await _context.Forklifts.AddRangeAsync(forklifts);
            await _context.SaveChangesAsync();
        }

        public async Task<List<string>> GetExistingModelNumbersAsync(List<string> modelNumbers)
        {
            return await _context.Forklifts
                        .Where(f => modelNumbers.Contains(f.ModelNumber))
                        .Select(f => f.ModelNumber)
                        .ToListAsync();
        }

        public async Task ClearAllForkliftsAsync()
        {
            _context.Forklifts.RemoveRange(_context.Forklifts);
            await _context.SaveChangesAsync();
        }

        public Task<bool> ExistsAsync(string modelNumber)
        {
            return _context.Forklifts.AnyAsync(f => f.ModelNumber == modelNumber);
        }

        public Task<List<ForkliftCommand>> GetCommandsByIdAsync(string modelNumber)
        {
            return _context.ForkliftCommands
                .Where(c => c.ModelNumber == modelNumber)
                .OrderByDescending(c => c.ActionDate)
                .ToListAsync();
        }

        public async Task<bool> AddCommandAsync(ForkliftCommand command)
        {
            await _context.ForkliftCommands.AddAsync(command);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
