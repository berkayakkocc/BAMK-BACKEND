using AutoMapper;
using BAMK.Application.DTOs.TShirt;
using BAMK.Core.Common;
using BAMK.Core.Interfaces;
using BAMK.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BAMK.Application.Services
{
    public class TShirtService : ITShirtService
    {
        private readonly IGenericRepository<TShirt> _tShirtRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TShirtService> _logger;

        public TShirtService(
            IGenericRepository<TShirt> tShirtRepository,
            IMapper mapper,
            ILogger<TShirtService> logger)
        {
            _tShirtRepository = tShirtRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<TShirtDto>>> GetAllAsync()
        {
            try
            {
                var tShirts = await _tShirtRepository.GetAllAsync();
                var tShirtDtos = _mapper.Map<IEnumerable<TShirtDto>>(tShirts);
                return Result<IEnumerable<TShirtDto>>.Success(tShirtDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tüm t-shirt'leri getirirken hata oluştu");
                return Result<IEnumerable<TShirtDto>>.Failure(Error.Create(ErrorCode.InvalidOperation, "T-shirt'ler getirilemedi"));
            }
        }

        public async Task<Result<TShirtDto?>> GetByIdAsync(int id)
        {
            try
            {
                var tShirt = await _tShirtRepository.GetByIdAsync(id);
                if (tShirt == null)
                {
                    return Result<TShirtDto?>.Failure(Error.NotFound("TShirt.NotFound", $"ID {id} olan t-shirt bulunamadı"));
                }

                var tShirtDto = _mapper.Map<TShirtDto>(tShirt);
                return Result<TShirtDto?>.Success(tShirtDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "T-shirt getirirken hata oluştu. ID: {Id}", id);
                return Result<TShirtDto?>.Failure(Error.Create(ErrorCode.InvalidOperation, "T-shirt getirilemedi"));
            }
        }

        public async Task<Result<TShirtDto>> CreateAsync(CreateTShirtDto createTShirtDto)
        {
            try
            {
                var tShirt = _mapper.Map<TShirt>(createTShirtDto);
                tShirt.IsActive = true;
                tShirt.CreatedAt = DateTime.UtcNow;

                await _tShirtRepository.AddAsync(tShirt);
                await _tShirtRepository.SaveChangesAsync();

                var tShirtDto = _mapper.Map<TShirtDto>(tShirt);
                return Result<TShirtDto>.Success(tShirtDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "T-shirt oluştururken hata oluştu");
                return Result<TShirtDto>.Failure(Error.Create(ErrorCode.InvalidOperation, "T-shirt oluşturulamadı"));
            }
        }

        public async Task<Result<TShirtDto>> UpdateAsync(int id, UpdateTShirtDto updateTShirtDto)
        {
            try
            {
                var existingTShirt = await _tShirtRepository.GetByIdAsync(id);
                if (existingTShirt == null)
                {
                    return Result<TShirtDto>.Failure(Error.NotFound("TShirt.NotFound", $"ID {id} olan t-shirt bulunamadı"));
                }

                _mapper.Map(updateTShirtDto, existingTShirt);
                existingTShirt.UpdatedAt = DateTime.UtcNow;

                _tShirtRepository.Update(existingTShirt);
                await _tShirtRepository.SaveChangesAsync();

                var tShirtDto = _mapper.Map<TShirtDto>(existingTShirt);
                return Result<TShirtDto>.Success(tShirtDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "T-shirt güncellenirken hata oluştu. ID: {Id}", id);
                return Result<TShirtDto>.Failure(Error.Create(ErrorCode.InvalidOperation, "T-shirt güncellenemedi"));
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            try
            {
                var tShirt = await _tShirtRepository.GetByIdAsync(id);
                if (tShirt == null)
                {
                    return Result<bool>.Failure(Error.NotFound("TShirt.NotFound", $"ID {id} olan t-shirt bulunamadı"));
                }

                _tShirtRepository.Remove(tShirt);
                await _tShirtRepository.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "T-shirt silinirken hata oluştu. ID: {Id}", id);
                return Result<bool>.Failure(Error.Create(ErrorCode.InvalidOperation, "T-shirt silinemedi"));
            }
        }

        public async Task<Result<IEnumerable<TShirtDto>>> GetActiveAsync()
        {
            try
            {
                var tShirts = await _tShirtRepository.FindAsync(t => t.IsActive);
                var tShirtDtos = _mapper.Map<IEnumerable<TShirtDto>>(tShirts);
                return Result<IEnumerable<TShirtDto>>.Success(tShirtDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Aktif t-shirt'leri getirirken hata oluştu");
                return Result<IEnumerable<TShirtDto>>.Failure(Error.Create(ErrorCode.InvalidOperation, "Aktif t-shirt'ler getirilemedi"));
            }
        }

        public async Task<Result<IEnumerable<TShirtDto>>> GetByColorAsync(string color)
        {
            try
            {
                var tShirts = await _tShirtRepository.FindAsync(t => t.Color == color && t.IsActive);
                var tShirtDtos = _mapper.Map<IEnumerable<TShirtDto>>(tShirts);
                return Result<IEnumerable<TShirtDto>>.Success(tShirtDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Renk bazlı t-shirt'leri getirirken hata oluştu. Renk: {Color}", color);
                return Result<IEnumerable<TShirtDto>>.Failure(Error.Create(ErrorCode.InvalidOperation, "Renk bazlı t-shirt'ler getirilemedi"));
            }
        }

        public async Task<Result<IEnumerable<TShirtDto>>> GetBySizeAsync(string size)
        {
            try
            {
                var tShirts = await _tShirtRepository.FindAsync(t => t.Size == size && t.IsActive);
                var tShirtDtos = _mapper.Map<IEnumerable<TShirtDto>>(tShirts);
                return Result<IEnumerable<TShirtDto>>.Success(tShirtDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Beden bazlı t-shirt'leri getirirken hata oluştu. Beden: {Size}", size);
                return Result<IEnumerable<TShirtDto>>.Failure(Error.Create(ErrorCode.InvalidOperation, "Beden bazlı t-shirt'ler getirilemedi"));
            }
        }

        public async Task<Result<bool>> UpdateStockAsync(int id, int quantity)
        {
            try
            {
                var tShirt = await _tShirtRepository.GetByIdAsync(id);
                if (tShirt == null)
                {
                    return Result<bool>.Failure(Error.NotFound("TShirt.NotFound", $"ID {id} olan t-shirt bulunamadı"));
                }

                tShirt.StockQuantity = quantity;
                tShirt.UpdatedAt = DateTime.UtcNow;

                _tShirtRepository.Update(tShirt);
                await _tShirtRepository.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "T-shirt stok güncellenirken hata oluştu. ID: {Id}, Miktar: {Quantity}", id, quantity);
                return Result<bool>.Failure(Error.Create(ErrorCode.InvalidOperation, "Stok güncellenemedi"));
            }
        }

        public async Task<Result<IEnumerable<TShirtDto>>> SearchAsync(string searchTerm)
        {
            try
            {
                var tShirts = await _tShirtRepository.FindAsync(t => 
                    t.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (t.Description != null && t.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (t.Color != null && t.Color.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                );
                
                var tShirtDtos = _mapper.Map<IEnumerable<TShirtDto>>(tShirts);
                return Result<IEnumerable<TShirtDto>>.Success(tShirtDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "T-shirt arama sırasında hata oluştu. Arama terimi: {SearchTerm}", searchTerm);
                return Result<IEnumerable<TShirtDto>>.Failure(Error.Create(ErrorCode.InvalidOperation, "Arama yapılamadı"));
            }
        }

        public async Task<Result<IEnumerable<TShirtDto>>> GetByCategoryAsync(string category)
        {
            try
            {
                var tShirts = await _tShirtRepository.FindAsync(t => 
                    t.Color != null && t.Color.Equals(category, StringComparison.OrdinalIgnoreCase) && t.IsActive
                );
                
                var tShirtDtos = _mapper.Map<IEnumerable<TShirtDto>>(tShirts);
                return Result<IEnumerable<TShirtDto>>.Success(tShirtDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategori bazlı t-shirt'leri getirirken hata oluştu. Kategori: {Category}", category);
                return Result<IEnumerable<TShirtDto>>.Failure(Error.Create(ErrorCode.InvalidOperation, "Kategori bazlı t-shirt'ler getirilemedi"));
            }
        }

        public async Task<Result<IEnumerable<TShirtDto>>> GetPagedAsync(int page, int limit, string? search = null, string? category = null)
        {
            try
            {
                var allTShirts = await _tShirtRepository.GetAllAsync();
                var products = allTShirts.ToList();
                
                // Apply search filter
                if (!string.IsNullOrEmpty(search))
                {
                    products = products.Where(p => 
                        p.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        (p.Description != null && p.Description.Contains(search, StringComparison.OrdinalIgnoreCase))
                    ).ToList();
                }

                // Apply category filter
                if (!string.IsNullOrEmpty(category))
                {
                    products = products.Where(p => 
                        p.Color != null && p.Color.Equals(category, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }

                // Apply pagination
                var paginatedProducts = products
                    .Skip((page - 1) * limit)
                    .Take(limit);

                var tShirtDtos = _mapper.Map<IEnumerable<TShirtDto>>(paginatedProducts);
                return Result<IEnumerable<TShirtDto>>.Success(tShirtDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sayfalanmış t-shirt'leri getirirken hata oluştu");
                return Result<IEnumerable<TShirtDto>>.Failure(Error.Create(ErrorCode.InvalidOperation, "Sayfalanmış t-shirt'ler getirilemedi"));
            }
        }

        public async Task<Result<IEnumerable<TShirtDto>>> GetFeaturedAsync(int limit = 8)
        {
            try
            {
                var tShirts = await _tShirtRepository.FindAsync(t => t.IsActive);
                var featuredTShirts = tShirts.Take(limit);
                
                var tShirtDtos = _mapper.Map<IEnumerable<TShirtDto>>(featuredTShirts);
                return Result<IEnumerable<TShirtDto>>.Success(tShirtDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Öne çıkan t-shirt'leri getirirken hata oluştu");
                return Result<IEnumerable<TShirtDto>>.Failure(Error.Create(ErrorCode.InvalidOperation, "Öne çıkan t-shirt'ler getirilemedi"));
            }
        }
    }
}