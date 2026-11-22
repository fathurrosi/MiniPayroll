using App.Application.Interfaces.Repositories;
using App.Application.Interfaces.Services;
using App.Application.Interfaces.Services.Settings;
using App.Domain.Entities;
using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using App.Infrastructure.Extensions;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace App.Infrastructure.Services.Settings
{
    public class BpjsKesehatanService : IBpjsKesehatanService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<TblBpjskesehatanConfig> _repo;
        private readonly ILogger<BpjsKesehatanService> _logger;
        private readonly IContextService _context;

        public BpjsKesehatanService(
            IGenericRepository<TblBpjskesehatanConfig> repo,
            IMapper mapper,
            ILogger<BpjsKesehatanService> logger,
            IContextService context)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
            _context = context;
        }

        public async Task<PagedResponse<BpjsKesehatanDto>> GetPagedAsync(DataTableRequest model)
        {
            try
            {
                var entityResult = await _repo.GetPagedAsync(model);
                return entityResult.MapPaged<TblBpjskesehatanConfig, BpjsKesehatanDto>(_mapper, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged BPJS Kesehatan configs");
                throw;
            }
        }

        public async Task<List<BpjsKesehatanDto>> GetListAsync()
        {
            try
            {
                var list = await _repo.GetListAsync();
                return _mapper.Map<List<BpjsKesehatanDto>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting BPJS Kesehatan list");
                throw;
            }
        }

        public async Task<BpjsKesehatanDto> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _repo.FindAsync(x => x.Id == id);
                return _mapper.Map<BpjsKesehatanDto>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting BPJS Kesehatan by ID {Id}", id);
                throw;
            }
        }

        public async Task<BpjsKesehatanDto> SaveAsync(BpjsKesehatanDto model)
        {
            try
            {
                if (model.Id <= 0)
                {
                    var entity = _mapper.Map<TblBpjskesehatanConfig>(model);
                    var added = await _repo.AddAsync(entity);
                    return _mapper.Map<BpjsKesehatanDto>(added);
                }
                else
                {
                    var existing = await _repo.FindAsync(x => x.Id == model.Id);
                    if (existing == null)
                        throw new KeyNotFoundException($"BPJS Kesehatan config {model.Id} not found");

                    _mapper.Map(model, existing);
                    var updated = await _repo.UpdateAsync(existing);
                    return _mapper.Map<BpjsKesehatanDto>(updated);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving BPJS Kesehatan config");
                throw;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            try
            {
                var entity = await _repo.FindAsync(x => x.Id == id);
                if (entity == null)
                    return 0;

                return await _repo.Remove(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting BPJS Kesehatan config {Id}", id);
                throw;
            }
        }
    }
}

