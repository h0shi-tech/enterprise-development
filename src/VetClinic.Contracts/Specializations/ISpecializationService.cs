using VetClinic.Domain.Enums;

namespace VetClinic.Contracts.Specializations;

public interface ISpecializationService
{
    IReadOnlyList<SpecializationDto> GetAll();

    SpecializationDto GetById(int id);

    IReadOnlyList<SpecializationDto> GetBySpecies(AnimalSpecies species);

    SpecializationDto Create(SpecializationEditDto dto);

    SpecializationDto Update(int id, SpecializationEditDto dto);

    void Delete(int id);
}
