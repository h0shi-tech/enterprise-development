namespace VetClinic.Contracts.Pets;

public interface IPetService
{
    IReadOnlyList<PetDto> GetAll();

    PetDto GetById(int id);

    IReadOnlyList<PetDto> GetByOwner(int ownerId);

    IReadOnlyList<PetDto> GetByVeterinarian(int veterinarianId);

    PetDto Create(PetEditDto dto);

    PetDto Update(int id, PetEditDto dto);

    void Delete(int id);
}
