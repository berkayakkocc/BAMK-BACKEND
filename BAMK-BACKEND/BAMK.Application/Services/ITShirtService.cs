using BAMK.Application.DTOs.TShirt;
using BAMK.Core.Common;

namespace BAMK.Application.Services
{
    public interface ITShirtService
    {
        Task<Result<IEnumerable<TShirtDto>>> GetAllAsync();
        Task<Result<TShirtDto?>> GetByIdAsync(int id);
        Task<Result<TShirtDto>> CreateAsync(CreateTShirtDto createTShirtDto);
        Task<Result<TShirtDto>> UpdateAsync(int id, UpdateTShirtDto updateTShirtDto);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<IEnumerable<TShirtDto>>> GetActiveAsync();
        Task<Result<IEnumerable<TShirtDto>>> GetByColorAsync(string color);
        Task<Result<IEnumerable<TShirtDto>>> GetBySizeAsync(string size);
        Task<Result<bool>> UpdateStockAsync(int id, int quantity);
    }
}
