using Microsoft.Extensions.DependencyInjection;
using VetClinic.Application.Services;
using VetClinic.Contracts.Appointments;
using VetClinic.Contracts.Breeds;
using VetClinic.Contracts.Owners;
using VetClinic.Contracts.Pets;
using VetClinic.Contracts.Specializations;
using VetClinic.Contracts.Veterinarians;

namespace VetClinic.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddVetClinicApplication(this IServiceCollection services)
    {
        services.AddSingleton<IBreedService, BreedService>();
        services.AddSingleton<ISpecializationService, SpecializationService>();
        services.AddSingleton<IOwnerService, OwnerService>();
        services.AddSingleton<IPetService, PetService>();
        services.AddSingleton<IVeterinarianService, VeterinarianService>();
        services.AddSingleton<IAppointmentService, AppointmentService>();
        return services;
    }
}
