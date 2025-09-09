using BAMK.Application.DTOs.User;
using BAMK.Core.Common;

namespace BAMK.Application.Services
{
    public interface IUserService
    {
        Task<Result<UserDto>> GetByIdAsync(int id);
        Task<Result<IEnumerable<UserDto>>> GetAllAsync();
        Task<Result<UserDto>> CreateAsync(CreateUserDto createUserDto);
        Task<Result<UserDto>> UpdateAsync(int id, UpdateUserDto updateUserDto);
        Task<Result> DeleteAsync(int id);
        Task<Result<UserDto>> GetByEmailAsync(string email);
        Task<Result<bool>> ValidatePasswordAsync(string email, string password);
    }
}
