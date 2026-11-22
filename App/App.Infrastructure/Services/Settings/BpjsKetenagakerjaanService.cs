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
    public class BpjsKetenagakerjaanService : IBpjsKetenagakerjaanService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<TblBpjsketenagakerjaanConfig> _repo;
        private readonly ILogger<BpjsKetenagakerjaanService> _logger;
        private readonly IContextService _context;

        public BpjsKetenagakerjaanService(
            IGenericRepository<TblBpjsketenagakerjaanConfig> repo,
            IMapper mapper,
            ILogger<BpjsKetenagakerjaanService> logger,
            IContextService context)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
            _context = context;
        }

        public async Task<PagedResponse<BpjsKetenagakerjaanDto>> GetPagedAsync(DataTableRequest model)
        {
            try
            {
                var entityResult = await _repo.GetPagedAsync(model);
                return entityResult.MapPaged<TblBpjsketenagakerjaanConfig, BpjsKetenagakerjaanDto>(_mapper, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged BPJS Ketenagakerjaan configs");
                throw;
            }
        }

        public async Task<List<BpjsKetenagakerjaanDto>> GetListAsync()
        {
            try
            {
                var list = await _repo.GetListAsync();
                return _mapper.Map<List<BpjsKetenagakerjaanDto>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting BPJS Ketenagakerjaan list");
                throw;
            }
        }

        public async Task<BpjsKetenagakerjaanDto> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _repo.FindAsync(x => x.Id == id);
                return _mapper.Map<BpjsKetenagakerjaanDto>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting BPJS Ketenagakerjaan by ID {Id}", id);
                throw;
            }
        }

        public async Task<BpjsKetenagakerjaanDto> SaveAsync(BpjsKetenagakerjaanDto model)
        {
            try
            {
                if (model.Id <= 0)
                {
                    var entity = _mapper.Map<TblBpjsketenagakerjaanConfig>(model);
                    var added = await _repo.AddAsync(entity);
                    return _mapper.Map<BpjsKetenagakerjaanDto>(added);
                }
                else
                {
                    var existing = await _repo.FindAsync(x => x.Id == model.Id);
                    if (existing == null)
                        throw new KeyNotFoundException($"BPJS Ketenagakerjaan config {model.Id} not found");

                    _mapper.Map(model, existing);
                    var updated = await _repo.UpdateAsync(existing);
                    return _mapper.Map<BpjsKetenagakerjaanDto>(updated);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving BPJS Ketenagakerjaan config");
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
                _logger.LogError(ex, "Error deleting BPJS Ketenagakerjaan config {Id}", id);
                throw;
            }
        }
    }
}

