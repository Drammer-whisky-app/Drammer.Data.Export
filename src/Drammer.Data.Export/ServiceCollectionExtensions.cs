using System.Diagnostics.CodeAnalysis;
using Drammer.Data.Export.Csv;
using Drammer.Data.Export.Pdf;
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

        services.AddSingleton<ICsvExportService, CsvExportService>();
        return services;
    }

    public static IServiceCollection AddPdfExportServices(
        this IServiceCollection services,
        [ConstantExpected] string configurationSection = PdfExportOptions.SectionName)
    {
        services.AddOptions<PdfExportOptions>().Configure<IConfiguration>(
            (settings, configuration) => { configuration.GetSection(configurationSection).Bind(settings); });

        services.AddSingleton<IPdfExportService, PdfExportService>();
        return services;
    }
}