using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using SensitiveWordsAPI.Helpers;
using SensitiveWordsAPI.Repositories;
using SensitiveWordsAPI.Services;
using System.Data;
using System.Runtime.CompilerServices;

namespace SensitiveWordsAPI.Extensions
{
    public static class ServicesCollectionExtension
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration) =>
            services
                .AddSingleton<MemoryCacheHelper>()
                .AddScoped<ISensitiveWordsRepository, SensitiveWordsRepository>()
                .AddScoped<ISensitiveWordsService, SensitiveWordsService>()
                .AddScoped<IDbConnection>(sp => new SqlConnection(configuration.GetConnectionString("DefaultConnectionString")));    
    }
}
