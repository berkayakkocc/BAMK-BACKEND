using BAMK.Application.DTOs;
using BAMK.Core.Common;
using BAMK.Core.Interfaces;
using BAMK.Domain.Entities;
using BAMK.Domain.Interfaces;
using AutoMapper;

namespace BAMK.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IGenericRepository<User> userRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<UserDto>> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return Result<UserDto>.Failure(Error.UserNotFound($"ID: {id}"));

            var userDto = _mapper.Map<UserDto>(user);
            return Result<UserDto>.Success(userDto);
        }

        public async Task<Result<IEnumerable<UserDto>>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
            return Result<IEnumerable<UserDto>>.Success(userDtos);
        }

        public async Task<Result<UserDto>> CreateAsync(CreateUserDto createUserDto)
        {
            // Email kontrolü
            var existingUser = await _userRepository.FirstOrDefaultAsync(u => u.Email == createUserDto.Email);
            if (existingUser != null)
                return Result<UserDto>.Failure(Error.UserAlreadyExists(createUserDto.Email));

            // Password hash
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);

            var user = new User
            {
                Email = createUserDto.Email,
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                PhoneNumber = createUserDto.PhoneNumber,
                PasswordHash = passwordHash,
                IsActive = true
            };

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var userDto = _mapper.Map<UserDto>(user);
            return Result<UserDto>.Success(userDto);
        }

        public async Task<Result<UserDto>> UpdateAsync(int id, UpdateUserDto updateUserDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return Result<UserDto>.Failure(Error.UserNotFound($"ID: {id}"));

            user.FirstName = updateUserDto.FirstName;
            user.LastName = updateUserDto.LastName;
            user.PhoneNumber = updateUserDto.PhoneNumber;
            user.UpdatedAt = DateTime.UtcNow;

            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();

            var userDto = _mapper.Map<UserDto>(user);
            return Result<UserDto>.Success(userDto);
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return Result.Failure(Error.UserNotFound($"ID: {id}"));

            user.IsDeleted = true;
            user.UpdatedAt = DateTime.UtcNow;

            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<UserDto>> GetByEmailAsync(string email)
        {
            var user = await _userRepository.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return Result<UserDto>.Failure(Error.UserNotFound(email));

            var userDto = _mapper.Map<UserDto>(user);
            return Result<UserDto>.Success(userDto);
        }

        public async Task<Result<bool>> ValidatePasswordAsync(string email, string password)
        {
            var user = await _userRepository.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return Result<bool>.Failure(Error.UserNotFound(email));

            var isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            return Result<bool>.Success(isValid);
        }
    }
}
