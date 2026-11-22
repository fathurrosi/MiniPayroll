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
    public class TaxService : ITaxService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<TblPph21Bracket> _bracketRepo;
        private readonly IGenericRepository<TblBiayaJabatanConfig> _biayaRepo;
        private readonly ILogger<TaxService> _logger;
        private readonly IContextService _context;

        public TaxService(
            IGenericRepository<TblPph21Bracket> bracketRepo,
            IGenericRepository<TblBiayaJabatanConfig> biayaRepo,
            IMapper mapper,
            ILogger<TaxService> logger,
            IContextService context)
        {
            _bracketRepo = bracketRepo;
            _biayaRepo = biayaRepo;
            _mapper = mapper;
            _logger = logger;
            _context = context;
        }

        #region PPh21 Bracket
        public async Task<PagedResponse<Pph21BracketDto>> GetPph21PagedAsync(DataTableRequest model)
        {
            try
            {
                var result = await _bracketRepo.GetPagedAsync(model);
                return result.MapPaged<TblPph21Bracket, Pph21BracketDto>(_mapper, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged PPh21 brackets");
                throw;
            }
        }

        public async Task<List<Pph21BracketDto>> GetPph21ListAsync()
        {
            try
            {
                var list = await _bracketRepo.GetListAsync();
                return _mapper.Map<List<Pph21BracketDto>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting PPh21 bracket list");
                throw;
            }
        }

        public async Task<Pph21BracketDto> GetPph21ByIdAsync(int id)
        {
            try
            {
                var entity = await _bracketRepo.FindAsync(x => x.Id == id);
                return _mapper.Map<Pph21BracketDto>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting PPh21 bracket by ID {Id}", id);
                throw;
            }
        }

        public async Task<Pph21BracketDto> SavePph21Async(Pph21BracketDto model)
        {
            try
            {
                if (model.Id <= 0)
                {
                    var entity = _mapper.Map<TblPph21Bracket>(model);
                    var added = await _bracketRepo.AddAsync(entity);
                    return _mapper.Map<Pph21BracketDto>(added);
                }
                else
                {
                    var existing = await _bracketRepo.FindAsync(x => x.Id == model.Id);
                    if (existing == null)
                        throw new KeyNotFoundException($"PPh21 bracket {model.Id} not found");

                    _mapper.Map(model, existing);
                    var updated = await _bracketRepo.UpdateAsync(existing);
                    return _mapper.Map<Pph21BracketDto>(updated);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving PPh21 bracket");
                throw;
            }
        }

        public async Task<int> DeletePph21Async(int id)
        {
            try
            {
                var entity = await _bracketRepo.FindAsync(x => x.Id == id);
                if (entity == null)
                    return 0;

                return await _bracketRepo.Remove(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting PPh21 bracket {Id}", id);
                throw;
            }
        }
        #endregion

        #region Biaya Jabatan
        public async Task<PagedResponse<BiayaJabatanDto>> GetBiayaJabatanPagedAsync(DataTableRequest model)
        {
            try
            {
                var result = await _biayaRepo.GetPagedAsync(model);
                return result.MapPaged<TblBiayaJabatanConfig, BiayaJabatanDto>(_mapper, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged Biaya Jabatan configs");
                throw;
            }
        }

        public async Task<List<BiayaJabatanDto>> GetBiayaJabatanListAsync()
        {
            try
            {
                var list = await _biayaRepo.GetListAsync();
                return _mapper.Map<List<BiayaJabatanDto>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Biaya Jabatan list");
                throw;
            }
        }

        public async Task<BiayaJabatanDto> GetBiayaJabatanByIdAsync(int id)
        {
            try
            {
                var entity = await _biayaRepo.FindAsync(x => x.Id == id);
                return _mapper.Map<BiayaJabatanDto>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Biaya Jabatan by ID {Id}", id);
                throw;
            }
        }

        public async Task<BiayaJabatanDto> SaveBiayaJabatanAsync(BiayaJabatanDto model)
        {
            try
            {
                if (model.Id <= 0)
                {
                    var entity = _mapper.Map<TblBiayaJabatanConfig>(model);
                    var added = await _biayaRepo.AddAsync(entity);
                    return _mapper.Map<BiayaJabatanDto>(added);
                }
                else
                {
                    var existing = await _biayaRepo.FindAsync(x => x.Id == model.Id);
                    if (existing == null)
                        throw new KeyNotFoundException($"Biaya Jabatan config {model.Id} not found");

                    _mapper.Map(model, existing);
                    var updated = await _biayaRepo.UpdateAsync(existing);
                    return _mapper.Map<BiayaJabatanDto>(updated);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Biaya Jabatan config");
                throw;
            }
        }

        public async Task<int> DeleteBiayaJabatanAsync(int id)
        {
            try
            {
                var entity = await _biayaRepo.FindAsync(x => x.Id == id);
                if (entity == null)
                    return 0;

                return await _biayaRepo.Remove(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Biaya Jabatan config {Id}", id);
                throw;
            }
        }
        #endregion
    }
}

