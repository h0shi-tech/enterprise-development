using VetClinic.Domain.Entities;
using VetClinic.Domain.Exceptions;

namespace VetClinic.Infrastructure.InMemory.Repositories;

internal static class SpeciesMatch
{
    public static void Ensure(Pet pet, Veterinarian veterinarian)
    {
        if (pet.Species != veterinarian.Specialization.Species)
        {
            throw new DomainRuleException("Вид питомца не совпадает со специализацией врача.");
        }
    }
}
