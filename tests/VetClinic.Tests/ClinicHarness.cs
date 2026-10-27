using Microsoft.Extensions.Logging.Abstractions;
using VetClinic.Application.Services;
using VetClinic.Infrastructure.InMemory;
using VetClinic.Infrastructure.InMemory.Repositories;

namespace VetClinic.Tests;

internal sealed class ClinicHarness
{
    public ClinicHarness(TimeProvider? time = null)
    {
        var clock = time ?? TimeProvider.System;
        var store = new InMemoryClinicStore();
        Breeds = new BreedService(new BreedRepository(store), NullLogger<BreedService>.Instance);
        Specializations = new SpecializationService(
            new SpecializationRepository(store),
            NullLogger<SpecializationService>.Instance);
        Owners = new OwnerService(new OwnerRepository(store), NullLogger<OwnerService>.Instance);
        Pets = new PetService(new PetRepository(store), clock, NullLogger<PetService>.Instance);
        Veterinarians = new VeterinarianService(
            new VeterinarianRepository(store),
            clock,
            NullLogger<VeterinarianService>.Instance);
        Appointments = new AppointmentService(
            new AppointmentRepository(store),
            new BreedRepository(store),
            clock,
            NullLogger<AppointmentService>.Instance);
    }

    public BreedService Breeds { get; }

    public SpecializationService Specializations { get; }

    public OwnerService Owners { get; }

    public PetService Pets { get; }

    public VeterinarianService Veterinarians { get; }

    public AppointmentService Appointments { get; }
}
