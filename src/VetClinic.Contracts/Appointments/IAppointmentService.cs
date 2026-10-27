namespace VetClinic.Contracts.Appointments;

public interface IAppointmentService
{
    IReadOnlyList<AppointmentDto> GetAll();

    AppointmentDto GetById(int id);

    IReadOnlyList<AppointmentDto> GetByVeterinarian(int veterinarianId);

    IReadOnlyList<AppointmentDto> GetByPet(int petId);

    IReadOnlyList<AppointmentDto> GetCurrentMonth(string roomNumber);

    FollowUpCountDto CountFollowUps(int breedId);

    AppointmentDto Create(AppointmentEditDto dto);

    AppointmentDto Update(int id, AppointmentEditDto dto);

    void Delete(int id);
}
