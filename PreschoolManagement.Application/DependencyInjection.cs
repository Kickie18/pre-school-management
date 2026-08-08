using Microsoft.Extensions.DependencyInjection;
using PreschoolManagement.Application.Mappings;

namespace PreschoolManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());
        return services;
    }
}
