using VetClinic.Domain.Entities;
using VetClinic.Domain.Exceptions;
using VetClinic.Domain.Interfaces;

namespace VetClinic.Infrastructure.InMemory.Repositories;

public sealed class AppointmentRepository(InMemoryClinicStore store) : IAppointmentRepository
{
    public IReadOnlyList<Appointment> GetAll()
    {
        lock (store.Sync)
        {
            return store.Appointments.OrderBy(appointment => appointment.VisitDateTime).ToArray();
        }
    }

    public Appointment? FindById(int id)
    {
        lock (store.Sync)
        {
            return store.Appointments.FirstOrDefault(appointment => appointment.Id == id);
        }
    }

    public IReadOnlyList<Appointment> GetByVeterinarian(int veterinarianId)
    {
        lock (store.Sync)
        {
            _ = store.Veterinarians.FirstOrDefault(veterinarian => veterinarian.Id == veterinarianId)
                ?? throw new EntityNotFoundException("ветеринар", veterinarianId);

            return store.Appointments
                .Where(appointment => appointment.Veterinarian.Id == veterinarianId)
                .OrderBy(appointment => appointment.VisitDateTime)
                .ToArray();
        }
    }

    public IReadOnlyList<Appointment> GetByPet(int petId)
    {
        lock (store.Sync)
        {
            _ = store.Pets.FirstOrDefault(pet => pet.Id == petId)
                ?? throw new EntityNotFoundException("питомец", petId);

            return store.Appointments
                .Where(appointment => appointment.Pet.Id == petId)
                .OrderBy(appointment => appointment.VisitDateTime)
                .ToArray();
        }
    }

    public IReadOnlyList<Appointment> GetByRoom(string roomNumber, DateTime from, DateTime to)
    {
        lock (store.Sync)
        {
            return store.Appointments
                .Where(appointment => appointment.RoomNumber == roomNumber
                    && appointment.VisitDateTime >= from
                    && appointment.VisitDateTime < to)
                .OrderBy(appointment => appointment.VisitDateTime)
                .ToArray();
        }
    }

    public int CountFollowUps(int breedId)
    {
        lock (store.Sync)
        {
            _ = store.Breeds.FirstOrDefault(breed => breed.Id == breedId)
                ?? throw new EntityNotFoundException("порода", breedId);

            return store.Appointments.Count(appointment =>
                appointment.IsFollowUp && appointment.Pet.Breed.Id == breedId);
        }
    }

    public Appointment Add(
        int petId,
        int veterinarianId,
        DateTime visitDateTime,
        string roomNumber,
        bool isFollowUp)
    {
        lock (store.Sync)
        {
            var pet = FindPet(petId);
            var veterinarian = FindVeterinarian(veterinarianId);
            SpeciesMatch.Ensure(pet, veterinarian);

            var appointment = new Appointment
            {
                Id = store.TakeAppointmentId(),
                Pet = pet,
                Veterinarian = veterinarian,
                VisitDateTime = visitDateTime,
                RoomNumber = roomNumber,
                IsFollowUp = isFollowUp
            };
            store.Appointments.Add(appointment);
            return appointment;
        }
    }

    public Appointment Update(
        int id,
        int petId,
        int veterinarianId,
        DateTime visitDateTime,
        string roomNumber,
        bool isFollowUp)
    {
        lock (store.Sync)
        {
            var appointment = store.Appointments.FirstOrDefault(item => item.Id == id)
                ?? throw new EntityNotFoundException("запись на приём", id);
            var pet = FindPet(petId);
            var veterinarian = FindVeterinarian(veterinarianId);
            SpeciesMatch.Ensure(pet, veterinarian);

            appointment.Pet = pet;
            appointment.Veterinarian = veterinarian;
            appointment.VisitDateTime = visitDateTime;
            appointment.RoomNumber = roomNumber;
            appointment.IsFollowUp = isFollowUp;
            return appointment;
        }
    }

    public void Remove(int id)
    {
        lock (store.Sync)
        {
            var appointment = store.Appointments.FirstOrDefault(item => item.Id == id)
                ?? throw new EntityNotFoundException("запись на приём", id);
            store.Appointments.Remove(appointment);
        }
    }

    private Pet FindPet(int petId) =>
        store.Pets.FirstOrDefault(pet => pet.Id == petId)
        ?? throw new EntityNotFoundException("питомец", petId);

    private Veterinarian FindVeterinarian(int veterinarianId) =>
        store.Veterinarians.FirstOrDefault(veterinarian => veterinarian.Id == veterinarianId)
        ?? throw new EntityNotFoundException("ветеринар", veterinarianId);
}
