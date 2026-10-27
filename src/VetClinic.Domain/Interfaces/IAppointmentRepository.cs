using VetClinic.Domain.Entities;

namespace VetClinic.Domain.Interfaces;

/// <summary>
/// Хранилище записей на приём
/// </summary>
public interface IAppointmentRepository
{
    IReadOnlyList<Appointment> GetAll();

    Appointment? FindById(int id);

    IReadOnlyList<Appointment> GetByVeterinarian(int veterinarianId);

    IReadOnlyList<Appointment> GetByPet(int petId);

    IReadOnlyList<Appointment> GetByRoom(string roomNumber, DateTime from, DateTime to);

    int CountFollowUps(int breedId);

    Appointment Add(
        int petId,
        int veterinarianId,
        DateTime visitDateTime,
        string roomNumber,
        bool isFollowUp);

    Appointment Update(
        int id,
        int petId,
        int veterinarianId,
        DateTime visitDateTime,
        string roomNumber,
        bool isFollowUp);

    void Remove(int id);
}
