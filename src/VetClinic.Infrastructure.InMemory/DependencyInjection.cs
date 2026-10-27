using Microsoft.Extensions.DependencyInjection;
using VetClinic.Domain.Interfaces;
using VetClinic.Infrastructure.InMemory.Repositories;

namespace VetClinic.Infrastructure.InMemory;

public static class DependencyInjection
{
    public static IServiceCollection AddInMemoryClinic(this IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);
        services.AddSingleton<InMemoryClinicStore>();
        services.AddSingleton<IBreedRepository, BreedRepository>();
        services.AddSingleton<ISpecializationRepository, SpecializationRepository>();
        services.AddSingleton<IOwnerRepository, OwnerRepository>();
        services.AddSingleton<IPetRepository, PetRepository>();
        services.AddSingleton<IVeterinarianRepository, VeterinarianRepository>();
        services.AddSingleton<IAppointmentRepository, AppointmentRepository>();
        return services;
    }
}
