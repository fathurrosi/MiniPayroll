
using App.Domain.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;

namespace App.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
        {
            //services.Configure<ApplicationSetting>(config.GetSection("Application"));
            services.AddHttpContextAccessor();
            services.Scan(scan => scan
                .FromAssemblies(typeof(global::MANDe.Transmittal.Infrastructure.AssemblyReference).Assembly)
                .AddClasses(classes => classes.InNamespaces(
                    "App.Services",
                    "App.Services.Administration" )
                .Where(type => type.Name.EndsWith("Service")))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
            //services.AddScoped<CookieHandler>();

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/AccessDenied";
                options.SlidingExpiration = true;
            });

            //services.AddHttpClient("MANDeApi", (sp, client) =>
            //{
            //    var appSetting = sp.GetRequiredService<IOptions<ApplicationSetting>>().Value;
            //    client.BaseAddress = new Uri(appSetting.ApiSetting.BaseUrl);
            //    client.DefaultRequestHeaders.Add(appSetting.ApiSetting.ApiKey, appSetting.ApiSetting.ApiKeyValue);
            //});


            //Utils.Initialize(
            //    env,
            //    config.GetSection("Application").Get<ApplicationSetting>()
            //);

            return services;
        }
    }
}