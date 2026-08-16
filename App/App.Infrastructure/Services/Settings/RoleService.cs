using App.Application.Interfaces.Repositories;
using App.Application.Interfaces.Services;
using App.Application.Interfaces.Services.Masters;
using App.Application.Interfaces.Services.Settings;
using App.Domain.Entities;
using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Dto.Settings;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using App.Infrastructure.Extensions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Collections;
using static Dapper.SqlMapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace App.Infrastructure.Services.Settings
{
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<TblRole> _roleRepo;

        private readonly IGenericRepository<TblRolePermission> _rolePermissionRepo;
        private readonly IGenericRepository<VwRolePermission> _rolePermissionViewRepo;
        private readonly ILogger<RoleService> _logger;
        private readonly IContextService _context;
        public RoleService(
            ILogger<RoleService> logger,
            IGenericRepository<TblRole> RoleRepo,
            IGenericRepository<TblRolePermission> RolePermissionRepo,
            IGenericRepository<VwRolePermission> rolePermissionViewRepo,
            IContextService context,
            IMapper mapper)
        {
            _logger = logger;
            _roleRepo = RoleRepo;
            _rolePermissionViewRepo = rolePermissionViewRepo;
            _mapper = mapper;
            _context = context;
            _rolePermissionRepo = RolePermissionRepo;
        }

        public async Task<List<RoleDto>> GetListAsync()
        {
            var entities = await _roleRepo.GetListAsync(t => !t.IsDeleted);
            return _mapper.Map<List<RoleDto>>(entities);
        }

        //public async List<RolePermissionDto> SavePermissionAsync(List<RolePermissionDto> permissions)
        //{
        //    var keys = permissions.Select(t => new { t.RoleCode, t.MenuId }).ToList();
        //    var exititngItems = await _rolePermissionRepo.GetFirstOrDefaultAsync(x =>
        //           keys.Where(t => t.MenuId == x.MenuId && t.RoleCode == x.RoleCode).Any());

        //    // this update
        //    exititngItems

        //    if any new then insert

        //    return value
        //}
        public async Task<List<RolePermissionDto>> SavePermissionAsync( 
            List<RolePermissionDto> permissions)
        {
            if (permissions == null || !permissions.Any())
                return new List<RolePermissionDto>();

            try
            {
                var roleCode = permissions
                    .Select(x => x.RoleCode)
                    .First();

                var menuIds = permissions
                    .Select(x => x.MenuId)
                    .Distinct()
                    .ToList();

                // Get existing permissions
                var existingItems = await _rolePermissionRepo.GetListAsync(x =>
                    x.RoleCode == roleCode &&
                    menuIds.Contains(x.MenuId) &&
                    x.IsDeleted != true);

                var updateItems = new List<TblRolePermission>();
                var newItems = new List<TblRolePermission>();

                foreach (var permission in permissions)
                {
                    var existing = existingItems.FirstOrDefault(x =>
                        x.RoleCode == permission.RoleCode &&
                        x.MenuId == permission.MenuId);

                    if (existing != null)
                    {
                        // =========================
                        // UPDATE
                        // =========================

                        existing.CanView = permission.CanView;
                        existing.CanCreate = permission.CanCreate;
                        existing.CanEdit = permission.CanEdit;
                        existing.CanDelete = permission.CanDelete;
                        existing.CanExport = permission.CanExport;
                        existing.CanApprove = permission.CanApprove;

                        existing.IsDeleted = false;

                        updateItems.Add(existing);
                    }
                    else
                    {
                        // =========================
                        // INSERT
                        // =========================

                        var newItem = new TblRolePermission
                        {
                            RoleCode = permission.RoleCode,
                            MenuId = permission.MenuId,

                            CanView = permission.CanView,
                            CanCreate = permission.CanCreate,
                            CanEdit = permission.CanEdit,
                            CanDelete = permission.CanDelete,
                            CanExport = permission.CanExport,
                            CanApprove = permission.CanApprove,

                            IsDeleted = false
                        };

                        newItems.Add(newItem);
                    }
                }

                // =========================
                // DATABASE OPERATIONS
                // =========================

                if (updateItems.Any())
                {
                    await _rolePermissionRepo.UpdateRangeAsync(updateItems);
                }

                if (newItems.Any())
                {
                    await _rolePermissionRepo.AddRangeAsync(newItems);
                }

                // =========================
                // RETURN SAVED DATA
                // =========================

                var results = await _rolePermissionRepo.GetListAsync(x =>
                    x.RoleCode == roleCode &&
                    menuIds.Contains(x.MenuId) &&
                    x.IsDeleted != true);

                return results
                    .Select(x => _mapper.Map<RolePermissionDto>(x))
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error saving role permissions for role {RoleCode}",
                    permissions.FirstOrDefault()?.RoleCode);

                throw;
            }
        }
        public async Task<RoleDto?> GetByCodeAsync(string code)
        {
            var entity = await _roleRepo.GetFirstOrDefaultAsync(x =>
                    x.RoleCode == code);

            return entity == null
                ? null
                : _mapper.Map<RoleDto>(entity);
        }

        public async Task<List<VwRolePermissionDto>> GetPermissionsAsync(string code, string menudId)
        {
            var entity = await _rolePermissionViewRepo.GetListAsync(x =>
                    x.RoleCode == code && (x.ParentId == menudId || x.MenuId == menudId));

            return entity == null
                ? null
                : _mapper.Map<List<VwRolePermissionDto>>(entity);
        }


        public async Task<RoleDto?> DeleteAsync(string code)
        {
            var entity = await _roleRepo.FindAsync(x =>
                x.RoleCode == code);
            if (entity == null)
                return null;

            entity.IsDeleted = true;
            entity = await _roleRepo.UpdateAsync(entity);

            return _mapper.Map<RoleDto>(entity);
        }

        public async Task<PagedResponse<RoleDto>> GetPagedAsync(DataTableRequest model)
        {
            try
            {
                var entityResult = await _roleRepo.GetPagedAsync(model);
                return entityResult.MapPaged<TblRole, RoleDto>(_mapper, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged Roles");
                throw;
            }
        }



        public async Task<RoleDto> SaveAsync(RoleDto model)
        {
            try
            {
                var existingItem = await _roleRepo.FindAsync(t => t.RoleCode == model.RoleCode);
                if (existingItem == null)
                {
                    var item = _mapper.Map<TblRole>(model);
                    item.CreatedBy = _context.Username;
                    item.CreatedDate = DateTime.Now;
                    var addedItem = await _roleRepo.AddAsync(item);
                    return _mapper.Map<RoleDto>(addedItem);
                }
                else
                {
                    string roleCode = existingItem.RoleCode;
                    _mapper.Map(model, existingItem);
                    existingItem.RoleCode = roleCode;
                    existingItem.UpdatedBy = _context.Username;
                    existingItem.UpdatedDate = DateTime.Now;
                    var updatedItem = await _roleRepo.UpdateAsync(existingItem);
                    return _mapper.Map<RoleDto>(updatedItem);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Role");
                throw;
            }
        }
    }
}
