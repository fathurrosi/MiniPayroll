using App.Application.Interfaces.Repositories;
using App.Application.Interfaces.Services;
using App.Application.Interfaces.Services.Settings;
using App.Domain.Entities;
using App.Domain.Models.Dto;
using App.Domain.Models.Dto.Settings;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using App.Infrastructure.Extensions;
using MapsterMapper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace App.Infrastructure.Services.Settings
{
    public class UserService : IUserService
    {
        const string CacheKeyPrefix = "CurrentUser";
        readonly IHasherService _hasherService;
        readonly IContextService _context;
        readonly IGenericRepository<TblUser> _userRepo;
        readonly IGenericRepository<VwUser> _vwUserRepo;
        readonly IGenericRepository<TblMenu> _menuRepo;
        readonly IGenericRepository<TblUserEmployee> _userEmployeeRepo;  // ★ NEW
        readonly IGenericRepository<TblEmployee> _employeeRepo;           // ★ NEW
        readonly IMapper _mapper;
        readonly ILogger<UserService> _logger;
        readonly IDistributedCache _cache;
        public UserService(IMapper mapper
            , ILogger<UserService> logger
            , IContextService context
            , IGenericRepository<TblUser> userRepo
            , IHasherService hasherService
            , IDistributedCache cache
            , IGenericRepository<TblMenu> menuRepo
            , IGenericRepository<VwUser> vwUserRepo
            , IGenericRepository<TblUserEmployee> userEmployeeRepo   // ★ NEW
            , IGenericRepository<TblEmployee> employeeRepo            // ★ NEW
            )
        {
            _mapper = mapper;
            _userRepo = userRepo;
            _logger = logger;
            _vwUserRepo = vwUserRepo;
            _context = context;
            _hasherService = hasherService;
            _cache = cache;
            _menuRepo = menuRepo;
            _userEmployeeRepo = userEmployeeRepo;  // ★ NEW
            _employeeRepo = employeeRepo;          // ★ NEW
        }

        public async Task<string> GetHashPassword(string password)
        {
            return _hasherService.Hash(password);
        }

        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            var user = await _userRepo.GetFirstOrDefaultAsync(t => t.Username == username);

            string temp = _hasherService.Hash(password);

            if (user == null || !_hasherService.Verify(password, user.PasswordHash))
            {
                return false;
            }

            return true;
        }


        #region CRUD
        public async Task<UserDto?> GetByKey(string name)
        {
            try
            {
                var entityItem = await _userRepo.GetFirstOrDefaultAsync(t => t.Username == name);
                return entityItem == null ? null : _mapper.Map<UserDto>(entityItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting User with name {name}", name);
                throw;
            }
        }
        public async Task<PagedResponse<UserDto>> GetPagedAsync(DataTableRequest model)
        {
            try
            {
                var entityResult = await _userRepo.GetPagedAsync(model, true);
                return entityResult.MapPaged<TblUser, UserDto>(_mapper, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged Users");
                throw;
            }
        }
        public async Task<int> Delete(string name)
        {
            try
            {
                var entityItem = await _userRepo.GetFirstOrDefaultAsync(t => t.Username.Equals(name));
                if (entityItem == null)
                {
                    throw new KeyNotFoundException($"User with name {name} not found");
                }
                return await _userRepo.Remove(entityItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting User with name {name}", name);
                throw;
            }
        }
        public async Task<UserDto> Save(UserDto model)
        {
            try
            {
                var entityItem = await _userRepo.GetFirstOrDefaultAsync(t => t.Username.Equals(model.Username));
                TblUser savedUser;

                if (entityItem == null)
                {
                    // ─── CREATE ───────────────────────────────────────────
                    TblUser item = _mapper.Map<TblUser>(model);
                    item.PasswordHash = _hasherService.Hash(model.Password);
                    item.CreatedBy = _context.Username;
                    item.CreatedDate = DateTime.UtcNow;
                    savedUser = await _userRepo.AddAsync(item);
                }
                else
                {
                    // ─── UPDATE ───────────────────────────────────────────
                    // Hanya update password jika diisi baru
                    if (!string.IsNullOrWhiteSpace(model.Password))
                    {
                        entityItem.PasswordHash = _hasherService.Hash(model.Password);
                    }
                    entityItem.Email = model.Email;
                    entityItem.FullName = model.FullName;
                    entityItem.IsActive = model.IsActive;
                    entityItem.UpdatedBy = _context.Username;
                    entityItem.UpdatedDate = DateTime.UtcNow;
                    savedUser = await _userRepo.UpdateAsync(entityItem);
                }

                // ★ Simpan mapping employee di tbl_UserEmployees (jika EmployeeCode dipilih)
                await SaveUserEmployeeMappingAsync(savedUser.Id, model.EmployeeCode);

                return _mapper.Map<UserDto>(savedUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving User");
                throw;
            }
        }

        /// <summary>
        /// Simpan / update mapping user ↔ employee di tbl_UserEmployees.
        /// Jika EmployeeCode kosong/null → hapus mapping yang ada.
        /// </summary>
        private async Task SaveUserEmployeeMappingAsync(long userId, string? employeeCode)
        {
            try
            {
                // Hapus mapping lama (jika ada)
                var existing = await _userEmployeeRepo.GetFirstOrDefaultAsync(x => x.UserId == userId);

                if (string.IsNullOrWhiteSpace(employeeCode))
                {
                    // Tidak ada employee dipilih → hapus mapping jika ada
                    if (existing != null)
                        await _userEmployeeRepo.Remove(existing);
                    return;
                }

                // Cari EmployeeId berdasarkan EmployeeCode
                var employee = await _employeeRepo.GetFirstOrDefaultAsync(e => e.EmployeeCode == employeeCode);
                if (employee == null) return; // EmployeeCode tidak valid → abaikan

                if (existing == null)
                {
                    // Insert mapping baru
                    await _userEmployeeRepo.AddAsync(new TblUserEmployee
                    {
                        UserId = userId,
                        EmployeeId = employee.EmployeeId,
                        CreatedDate = DateTime.UtcNow
                    });
                }
                else if (existing.EmployeeId != employee.EmployeeId)
                {
                    // Update mapping ke employee yang berbeda
                    await _userEmployeeRepo.Remove(existing);
                    await _userEmployeeRepo.AddAsync(new TblUserEmployee
                    {
                        UserId = userId,
                        EmployeeId = employee.EmployeeId,
                        CreatedDate = DateTime.UtcNow
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving User");
                throw;
            }
        }

        public async Task<List<UserDto>> GetUsersAsync()
        {
            try
            {
                var entityResult = await _userRepo.GetListAsync();
                return entityResult.Select(t => _mapper.Map<UserDto>(t)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting User");
                throw;
            }
        }
        #endregion 
        public async Task<UserDto?> GetById(int userId)
        {
            try
            {
                var entity = await _userRepo.GetFirstOrDefaultAsync(u => u.Username == userId.ToString());
                return entity == null ? null : _mapper.Map<UserDto>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting User with UserId {userId}", userId);
                throw;
            }
        }

        public async Task<List<UserDto>> GetUsers(List<string> Usernames)
        {
            if (Usernames == null || !Usernames.Any())
                return new List<UserDto>();

            var UsernameSet = Usernames
               .Where(x => !string.IsNullOrEmpty(x))
               .Distinct()
               .ToList();

            var entityResult = await _userRepo.GetListAsync(
                t => UsernameSet.Contains(t.Username)
            );

            return entityResult
                .Select(t => _mapper.Map<UserDto>(t))
                .ToList();
        }

        public async Task<List<UserDto>> GetUserByDisplayName(List<string> displayNames)
        {
            var entityResult = await _userRepo.GetListAsync(t => displayNames.Any(t1 => t1 == t.FullName));
            return entityResult.Select(t => _mapper.Map<UserDto>(t)).ToList();
        }

        public async Task<List<UserDto>> GetUsersAsync(string groupName)
        {
            try
            {
                if (string.IsNullOrEmpty(groupName))
                    return await GetUsersAsync();
                else
                {
                    var entityResult = await _userRepo.GetListAsync();
                    return entityResult.Select(t => _mapper.Map<UserDto>(t)).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting User");
                throw;
            }
        }

        public async Task<VwUserDto?> GetUserAsync()
        {
            VwUserDto? userInfo = null;
            string userDataJson = "";
            var username = _context.Username;
            if (string.IsNullOrEmpty(username)) return null;

            var cacheKey = $"{CacheKeyPrefix}:{username}";
            var cachedBytes = await _cache.GetAsync(cacheKey);
            if (cachedBytes != null)
            {
                var json = Encoding.UTF8.GetString(cachedBytes);
                return JsonConvert.DeserializeObject<VwUserDto>(json);
            }

            userInfo = await GetUserAsync(username);
            if (userInfo != null)
            {
                userDataJson = JsonConvert.SerializeObject(userInfo);
                var cacheEntryOptions = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1));
                _cache.SetString(cacheKey, userDataJson, cacheEntryOptions);
            }
            return userInfo;
        }

        // ★ NEW: Ambil VwUserDto langsung dari view (ada EmployeeCode) berdasarkan username
        public async Task<VwUserDto?> GetVwUserByUsernameAsync(string username)
        {
            if (string.IsNullOrEmpty(username)) return null;
            var entity = await _vwUserRepo.GetFirstOrDefaultAsync(u => u.Username == username);
            return entity == null ? null : _mapper.Map<VwUserDto>(entity);
        }

        private async Task<VwUserDto?> GetUserAsync(string username)
        {
            VwUserDto? user = null;
            if (string.IsNullOrEmpty(username)) return null;
            var entity = await _vwUserRepo.GetFirstOrDefaultAsync(u => u.Username == username);
            if (entity != null)
            {
                user = _mapper.Map<VwUserDto>(entity);
                var menuEntities = await _menuRepo.GetListAsync(t => t.Deleted != 1);

                List<MenuDto> result = new List<MenuDto>();
                var parentList = menuEntities.Where(t => string.IsNullOrEmpty(t.ParentId)).OrderBy(t => t.Sort).ToList();
                foreach (var parent in parentList)
                {
                    var parentManu = _mapper.Map<MenuDto>(parent);
                    parentManu.ChildList = menuEntities.Where(t => t.ParentId == parent.Id)
                        .Select(t => _mapper.Map<MenuDto>(t)).OrderBy(t => t.Sort).ToList();

                    result.Add(parentManu);
                }

                user.MenuList = result;

                user.ConfigMenuList = menuEntities.Where(t => t.IsCard == true)
                    .Select(t => _mapper.Map<MenuDto>(t)).OrderBy(t => t.Sort).ToList();
            }
            return user;
        }

        public async Task<bool> ClearAsync()
        {
            return await ClearAsync(_context.Username);
        }

        public async Task<bool> ClearAsync(string username)
        {
            try
            {
                if (!string.IsNullOrEmpty(username))
                {
                    string cacheKey = $"{CacheKeyPrefix}:{username}";
                    await _cache.RemoveAsync(cacheKey);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public Task<List<UserDto>> GetUsersAsyncJoin()
        {
            throw new NotImplementedException();
        }

        public Task<List<UserDto>> GetUnMappedusers()
        {
            throw new NotImplementedException();
        }

        public Task DeleteBulkAsync(List<int> userIds)
        {
            throw new NotImplementedException();
        }

    }

}
