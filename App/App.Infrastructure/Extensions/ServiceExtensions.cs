using App.Application.Interfaces.Procedures;
using App.Application.Interfaces.Repositories;
using App.Domain.Models;
using App.Domain.Models.Attributes;
using App.Infrastructure.Data;
using App.Infrastructure.Procedures;
using App.Infrastructure.Repositories;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(
            this IServiceCollection services,
            IConfiguration config,
            IWebHostEnvironment env)
        {
            // services.Configure<ApplicationSetting>(config.GetSection("Application"));

            services.AddHttpContextAccessor();

            services.Scan(scan => scan
                .FromAssemblies(typeof(global::App.Infrastructure.AssemblyReference).Assembly)
                .AddClasses(classes => classes .InNamespaces(
                    "App.Infrastructure.Services",
                    "App.Infrastructure.Services.Payroll",
                    "App.Infrastructure.Services.Settings",
                    "App.Infrastructure.Services.Transactions")
                    .Where(type => type.Name.EndsWith("Service")))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            var mapsterConfig = TypeAdapterConfig.GlobalSettings;

            mapsterConfig.Default.IgnoreAttribute(typeof(IgnoreMappingAttribute));

            MappingConfig.RegisterMappings(mapsterConfig);

            services.AddSingleton(mapsterConfig);


            services.AddScoped<IMapper, ServiceMapper>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme =
                    CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/AccessDenied";
                options.SlidingExpiration = true;
            });


            services.AddDbContext<AppDBContext>(options =>
                options.UseSqlServer(
                    config.GetConnectionString("DefaultConnection"),
                    sqlOptions =>
                    {
                        sqlOptions.CommandTimeout(3600);
                    }));

            services.AddScoped(
                typeof(IGenericRepository<>),
                typeof(GenericRepository<>));

            services.AddScoped<IProcedureExecutor, ProcedureExecutor>();

            return services;
        }
    }
}