namespace VetClinic.Contracts.Owners;

public interface IOwnerService
{
    IReadOnlyList<OwnerDto> GetAll();

    OwnerDto GetById(int id);

    IReadOnlyList<OwnerDto> GetWithSeveralPets();

    OwnerDto Create(OwnerEditDto dto);

    OwnerDto Update(int id, OwnerEditDto dto);

    void Delete(int id);
}
