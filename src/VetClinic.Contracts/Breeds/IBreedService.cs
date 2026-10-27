using VetClinic.Domain.Enums;

namespace VetClinic.Contracts.Breeds;

public interface IBreedService
{
    IReadOnlyList<BreedDto> GetAll();

    BreedDto GetById(int id);

    IReadOnlyList<BreedDto> GetBySpecies(AnimalSpecies species);

    BreedDto Create(BreedEditDto dto);

    BreedDto Update(int id, BreedEditDto dto);

    void Delete(int id);
}
