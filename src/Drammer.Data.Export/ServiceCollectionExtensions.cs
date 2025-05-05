using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Drammer.Data.Export;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCsvExportServices(
        this IServiceCollection services,
        [ConstantExpected] string configurationSection = CsvExportOptions.SectionName)
    {
        services.AddOptions<CsvExportOptions>().Configure<IConfiguration>(
            (settings, configuration) => { configuration.GetSection(configurationSection).Bind(settings); });

        services.AddKeyedSingleton<IExportService, CsvExportService>("csv");
        return services;
    }
}