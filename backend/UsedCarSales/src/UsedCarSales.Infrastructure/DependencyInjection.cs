using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UsedCarSales.Application.Interfaces;
using UsedCarSales.Infrastructure.Persistence;
using UsedCarSales.Infrastructure.Repositories;
using UsedCarSales.Infrastructure.Storage;

namespace UsedCarSales.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, string connectionString, string webRootPath)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<ICarRepository, CarRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IBillRepository, BillRepository>();

        services.AddSingleton<IFileStorage>(_ => new LocalFileStorage(webRootPath));

        return services;
    }
}