using VetClinic.Domain.Enums;

namespace VetClinic.Contracts.Veterinarians;

public interface IVeterinarianService
{
    IReadOnlyList<VeterinarianDto> GetAll();

    VeterinarianDto GetById(int id);

    IReadOnlyList<VeterinarianDto> GetBySpecies(AnimalSpecies species);

    IReadOnlyList<VeterinarianDto> GetBySpecialization(int specializationId);

    VeterinarianDto Create(VeterinarianEditDto dto);

    VeterinarianDto Update(int id, VeterinarianEditDto dto);

    void Delete(int id);
}
