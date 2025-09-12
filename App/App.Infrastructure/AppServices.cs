using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using App.Application.Interfaces.Repositories;
using App.Application.Interfaces.Services;
using App.Infrastructure.Repositories;
using App.Infrastructure.Services;
namespace App.Infrastructure
{
    public static class AppServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ILogRepository, LogRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IMenuRepository, MenuRepository>();
            services.AddTransient<IPrevillageRepository, PrevillageRepository>();
            services.AddTransient<IJwtService, JwtService>();
            return services;
        }
    }
}
