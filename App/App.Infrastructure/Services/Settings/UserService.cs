using App.Application.Interfaces.Repositories;
using App.Application.Interfaces.Services;
using App.Application.Interfaces.Services.Settings;
using App.Domain.Entities;
using App.Domain.Models.Dto;
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
        readonly IGenericRepository<TblMenu> _menuRepo;
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
            )
        {
            _mapper = mapper;
            _userRepo = userRepo;
            _logger = logger;
            _context = context;
            _hasherService = hasherService;
            _cache = cache;
            _menuRepo = menuRepo;
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

        //public async Task<UserViewDto?> GetViewByKey(string name)
        //{
        //    try
        //    {
        //        var entityItem = await _userViewRepo.GetFirstOrDefaultAsync(t => t.Username == name);
        //        return entityItem == null ? null : _mapper.Map<UserViewDto>(entityItem);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error getting User View with name {name}", name);
        //        throw;
        //    }
        //}

        //public async Task<List<UserViewDto>> GetUsersViewByGroup(string groupName)
        //{
        //    try
        //    {
        //        var entityItem = await _userViewRepo.GetListAsync(t => t.GroupName == groupName);
        //        return entityItem.Select(t => _mapper.Map<UserViewDto>(t)).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error getting User View with name {groupName}", groupName);
        //        throw;
        //    }
        //}
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

        //public async Task<PagedResult<UserViewDto>> GetPagedByGroupAsync(DataTableRequest model, string groupName)
        //{
        //    try
        //    {
        //        var entityResult = await _userViewRepo.GetPagedAsync(t => t.GroupName == groupName, model);
        //        return entityResult.MapPaged<VwTblUser, UserViewDto>(_mapper, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error getting paged Users");
        //        throw;
        //    }
        //}
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
                if (entityItem == null)
                {
                    TblUser item = _mapper.Map<TblUser>(model);
                    item.CreatedBy = _context.Username;
                    item.CreatedDate = DateTime.UtcNow;
                    var addedEntity = await _userRepo.AddAsync(item);
                    return _mapper.Map<UserDto>(addedEntity);
                }
                else
                {
                    _mapper.Map(model, entityItem);
                    entityItem.UpdatedBy = _context.Username;
                    entityItem.UpdatedDate = DateTime.UtcNow;
                    var updatedEntity = await _userRepo.UpdateAsync(entityItem);
                    return _mapper.Map<UserDto>(updatedEntity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving User");
                throw;
            }
        }
        public async Task<UserDto> SaveUser(UserDto model)
        {
            try
            {
                return new UserDto();
                //// CEK USERNAME DUPLICATE
                //var existingUsername = await _userRepo
                //    .GetFirstOrDefaultAsync(x =>
                //        x.Username == model.Username &&
                //        x.UserId != model.UserId);

                //if (existingUsername != null)
                //{
                //    throw new Exception("Username already exists");
                //}

                //var entityItem = await _userRepo
                //    .GetFirstOrDefaultAsync(t => t.UserId == model.UserId);

                //if (entityItem == null)
                //{
                //    TblUser item =
                //        _mapper.Map<TblUser>(model);

                //    item.PasswordHash =
                //        _passwordHasher.Hash(model.Password);

                //    item.CreatedBy = _context.Username;
                //    item.CreatedDate = DateTime.UtcNow;

                //    var addedEntity =
                //        await _userRepo.AddAsync(item);

                //    return _mapper.Map<UserDto>(addedEntity);
                //}
                //else
                //{
                //    var oldPasswordHash = entityItem.PasswordHash;

                //    _mapper.Map(model, entityItem);

                //    entityItem.PasswordHash = oldPasswordHash;

                //    if (!string.IsNullOrWhiteSpace(model.Password))
                //    {
                //        entityItem.PasswordHash =
                //            _passwordHasher.Hash(model.Password);
                //    }

                //    entityItem.UpdatedBy = _context.Username;
                //    entityItem.UpdatedDate = DateTime.UtcNow;

                //    var updatedEntity =
                //        await _userRepo.UpdateAsync(entityItem);

                //    return _mapper.Map<UserDto>(updatedEntity);
                //}
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

        public async Task<UserDto?> GetUserAsync()
        {
            UserDto? userInfo = null;
            string userDataJson = "";
            var username = _context.Username;
            if (string.IsNullOrEmpty(username)) return null;

            var cacheKey = $"{CacheKeyPrefix}:{username}";
            var cachedBytes = await _cache.GetAsync(cacheKey);
            if (cachedBytes != null)
            {
                var json = Encoding.UTF8.GetString(cachedBytes);
                return JsonConvert.DeserializeObject<UserDto>(json);
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

        private async Task<UserDto?> GetUserAsync(string username)
        {
            UserDto? user = null;
            if (string.IsNullOrEmpty(username)) return null;
            var entity = await _userRepo.GetFirstOrDefaultAsync(u => u.Username == username);
            if (entity != null)
            {
                user = _mapper.Map<UserDto>(entity);
                var menuEntities = await _menuRepo.GetListAsync(t=> t.Deleted != 1);

                List<MenuDto> result = new List<MenuDto>();
                var parentList = menuEntities.Where(t => string.IsNullOrEmpty(t.ParentId)).ToList();
                foreach (var parent in parentList)
                {
                    var parentManu = _mapper.Map<MenuDto>(parent);
                    parentManu.ChildList = menuEntities.Where(t => t.ParentId == parent.Id).Select(t => _mapper.Map<MenuDto>(t)).OrderBy(t => t.Sort).ToList();

                    result.Add(parentManu);
                }

                user.MenuList = result;
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

        Task<UserDto?> IUserService.GetUserAsync()
        {
            return GetUserAsync();
        }
    }

}
