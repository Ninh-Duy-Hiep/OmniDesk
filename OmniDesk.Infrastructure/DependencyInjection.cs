using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OmniDesk.Application.Interfaces;
using OmniDesk.Infrastructure.Persistence;
using OmniDesk.Infrastructure.Persistence.Repositories;

namespace OmniDesk.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Không tìm thấy chuỗi kết nối 'DefaultConnection'.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}