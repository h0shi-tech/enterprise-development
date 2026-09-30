using VetClinic.Domain.Entities;
using VetClinic.Domain.Enums;

namespace VetClinic.Domain.Data;

/// <summary>
/// Набор тестовых данных ветеринарной клиники, хранящийся в памяти
/// </summary>
public class VetClinicSeedData
{
    /// <summary>
    /// Создаёт связанный между собой набор данных клиники
    /// </summary>
    public VetClinicSeedData()
    {
        Breeds = CreateBreeds();
        Specializations = CreateSpecializations();
        Owners = CreateOwners();
        Pets = CreatePets(Owners, Breeds);
        Veterinarians = CreateVeterinarians(Specializations);
        Appointments = CreateAppointments(Pets, Veterinarians);

        foreach (var pet in Pets)
        {
            pet.Owner.Pets.Add(pet);
        }
    }

    /// <summary>
    /// Опорная дата набора данных: приёмы сентября 2026 года считаются приёмами текущего месяца
    /// </summary>
    public DateTime ReferenceDate { get; } = new DateTime(2026, 9, 15);

    /// <summary>
    /// Справочник пород
    /// </summary>
    public IReadOnlyList<Breed> Breeds { get; }

    /// <summary>
    /// Справочник специализаций врачей
    /// </summary>
    public IReadOnlyList<Specialization> Specializations { get; }

    /// <summary>
    /// Владельцы питомцев
    /// </summary>
    public IReadOnlyList<Owner> Owners { get; }

    /// <summary>
    /// Питомцы клиники
    /// </summary>
    public IReadOnlyList<Pet> Pets { get; }

    /// <summary>
    /// Ветеринарные врачи
    /// </summary>
    public IReadOnlyList<Veterinarian> Veterinarians { get; }

    /// <summary>
    /// Записи на приём
    /// </summary>
    public IReadOnlyList<Appointment> Appointments { get; }

    private static List<Breed> CreateBreeds() =>
    [
        new Breed { Id = 1, Name = "Мейн-кун", Species = AnimalSpecies.Cat },
        new Breed { Id = 2, Name = "Британская короткошёрстная", Species = AnimalSpecies.Cat },
        new Breed { Id = 3, Name = "Сиамская", Species = AnimalSpecies.Cat },
        new Breed { Id = 4, Name = "Немецкая овчарка", Species = AnimalSpecies.Dog },
        new Breed { Id = 5, Name = "Лабрадор-ретривер", Species = AnimalSpecies.Dog },
        new Breed { Id = 6, Name = "Вельш-корги", Species = AnimalSpecies.Dog },
        new Breed { Id = 7, Name = "Джунгарский хомяк", Species = AnimalSpecies.Rodent },
        new Breed { Id = 8, Name = "Морская свинка", Species = AnimalSpecies.Rodent },
        new Breed { Id = 9, Name = "Карликовый кролик", Species = AnimalSpecies.Rabbit },
        new Breed { Id = 10, Name = "Волнистый попугай", Species = AnimalSpecies.Bird },
        new Breed { Id = 11, Name = "Корелла", Species = AnimalSpecies.Bird },
        new Breed { Id = 12, Name = "Красноухая черепаха", Species = AnimalSpecies.Reptile },
        new Breed { Id = 13, Name = "Бородатая агама", Species = AnimalSpecies.Reptile },
        new Breed { Id = 14, Name = "Хорёк домашний", Species = AnimalSpecies.Ferret }
    ];

    private static List<Specialization> CreateSpecializations() =>
    [
        new Specialization { Id = 1, Name = "Терапевт", Species = AnimalSpecies.Cat },
        new Specialization { Id = 2, Name = "Хирург", Species = AnimalSpecies.Cat },
        new Specialization { Id = 3, Name = "Терапевт", Species = AnimalSpecies.Dog },
        new Specialization { Id = 4, Name = "Хирург", Species = AnimalSpecies.Dog },
        new Specialization { Id = 5, Name = "Стоматолог", Species = AnimalSpecies.Dog },
        new Specialization { Id = 6, Name = "Дерматолог", Species = AnimalSpecies.Cat },
        new Specialization { Id = 7, Name = "Родентолог", Species = AnimalSpecies.Rodent },
        new Specialization { Id = 8, Name = "Ортопед", Species = AnimalSpecies.Rabbit },
        new Specialization { Id = 9, Name = "Орнитолог", Species = AnimalSpecies.Bird },
        new Specialization { Id = 10, Name = "Герпетолог", Species = AnimalSpecies.Reptile },
        new Specialization { Id = 11, Name = "Кардиолог", Species = AnimalSpecies.Cat },
        new Specialization { Id = 12, Name = "Офтальмолог", Species = AnimalSpecies.Dog },
        new Specialization { Id = 13, Name = "Специалист по экзотическим животным", Species = AnimalSpecies.Ferret }
    ];

    private static List<Owner> CreateOwners() =>
    [
        new Owner { Id = 1, FullName = "Никитина Анна Сергеевна", PhoneNumber = "+7 900 111-22-01", Address = "ул. Ленина, 12, кв. 4" },
        new Owner { Id = 2, FullName = "Абрамов Игорь Петрович", PhoneNumber = "+7 900 111-22-02", Address = "ул. Садовая, 5, кв. 18" },
        new Owner { Id = 3, FullName = "Волков Денис Олегович", PhoneNumber = "+7 900 111-22-03", Address = "пр. Мира, 44, кв. 71" },
        new Owner { Id = 4, FullName = "Гурьева Марина Львовна", PhoneNumber = "+7 900 111-22-04", Address = "ул. Полевая, 8, кв. 2" },
        new Owner { Id = 5, FullName = "Дементьев Павел Игоревич", PhoneNumber = "+7 900 111-22-05", Address = "ул. Речная, 23, кв. 56" },
        new Owner { Id = 6, FullName = "Ермакова Ольга Дмитриевна", PhoneNumber = "+7 900 111-22-06", Address = "ул. Зелёная, 3, кв. 9" },
        new Owner { Id = 7, FullName = "Жукова Татьяна Романовна", PhoneNumber = "+7 900 111-22-07", Address = "пр. Победы, 17, кв. 30" },
        new Owner { Id = 8, FullName = "Зайцев Кирилл Андреевич", PhoneNumber = "+7 900 111-22-08" },
        new Owner { Id = 9, FullName = "Иванцов Роман Сергеевич", PhoneNumber = "+7 900 111-22-09", Address = "ул. Лесная, 61, кв. 12" },
        new Owner { Id = 10, FullName = "Ковалёв Артём Юрьевич", PhoneNumber = "+7 900 111-22-10", Address = "ул. Северная, 2, кв. 88" },
        new Owner { Id = 11, FullName = "Лапшина Елена Викторовна", PhoneNumber = "+7 900 111-22-11", Address = "ул. Южная, 19, кв. 43" },
        new Owner { Id = 12, FullName = "Мельникова Дарья Олеговна", PhoneNumber = "+7 900 111-22-12" }
    ];

    private static List<Pet> CreatePets(IReadOnlyList<Owner> owners, IReadOnlyList<Breed> breeds)
    {
        var ownerById = owners.ToDictionary(owner => owner.Id);
        var breedById = breeds.ToDictionary(breed => breed.Id);

        return
        [
            new Pet
            {
                Id = 1,
                Name = "Мурзик",
                Breed = breedById[1],
                Owner = ownerById[1],
                BirthDate = new DateOnly(2019, 4, 12),
                Weight = 7.4
            },
            new Pet
            {
                Id = 2,
                Name = "Ася",
                Breed = breedById[3],
                Owner = ownerById[1],
                BirthDate = new DateOnly(2021, 6, 30),
                Weight = 3.8
            },
            new Pet
            {
                Id = 3,
                Name = "Граф",
                Breed = breedById[4],
                Owner = ownerById[2],
                BirthDate = new DateOnly(2018, 9, 5),
                Weight = 34.5
            },
            new Pet
            {
                Id = 4,
                Name = "Белла",
                Breed = breedById[5],
                Owner = ownerById[2],
                BirthDate = new DateOnly(2020, 2, 17),
                Weight = 28.9
            },
            new Pet
            {
                Id = 5,
                Name = "Тимон",
                Breed = breedById[6],
                Owner = ownerById[3],
                BirthDate = new DateOnly(2022, 11, 23),
                Weight = 12.1
            },
            new Pet
            {
                Id = 6,
                Name = "Пушок",
                Breed = breedById[2],
                Owner = ownerById[4],
                BirthDate = new DateOnly(2017, 1, 19),
                Weight = 5.6
            },
            new Pet
            {
                Id = 7,
                Name = "Соня",
                Breed = breedById[7],
                Owner = ownerById[5],
                BirthDate = new DateOnly(2024, 3, 8),
                Weight = 0.05
            },
            new Pet
            {
                Id = 8,
                Name = "Кроха",
                Breed = breedById[8],
                Owner = ownerById[5],
                BirthDate = new DateOnly(2023, 7, 14),
                Weight = 0.9
            },
            new Pet
            {
                Id = 9,
                Name = "Ушастик",
                Breed = breedById[9],
                Owner = ownerById[6],
                BirthDate = new DateOnly(2022, 5, 2),
                Weight = 1.7
            },
            new Pet
            {
                Id = 10,
                Name = "Кеша",
                Breed = breedById[10],
                Owner = ownerById[7],
                BirthDate = new DateOnly(2023, 10, 11),
                Weight = 0.04
            },
            new Pet
            {
                Id = 11,
                Name = "Лора",
                Breed = breedById[11],
                Owner = ownerById[7],
                BirthDate = new DateOnly(2021, 12, 1),
                Weight = 0.09
            },
            new Pet
            {
                Id = 12,
                Name = "Тортилла",
                Breed = breedById[12],
                Owner = ownerById[8],
                BirthDate = new DateOnly(2015, 8, 21),
                Weight = 1.2
            },
            new Pet
            {
                Id = 13,
                Name = "Фред",
                Breed = breedById[14],
                Owner = ownerById[9],
                BirthDate = new DateOnly(2022, 2, 28),
                Weight = 1.1
            },
            new Pet
            {
                Id = 14,
                Name = "Зевс",
                Breed = breedById[13],
                Owner = ownerById[10],
                BirthDate = new DateOnly(2020, 6, 15),
                Weight = 0.45
            },
            new Pet
            {
                Id = 15,
                Name = "Найда",
                Breed = breedById[4],
                Owner = ownerById[11],
                BirthDate = new DateOnly(2019, 3, 30),
                Weight = 31.2
            },
            new Pet
            {
                Id = 16,
                Name = "Симба",
                Breed = breedById[1],
                Owner = ownerById[12],
                BirthDate = new DateOnly(2023, 1, 9),
                Weight = 6.3
            }
        ];
    }

    private static List<Veterinarian> CreateVeterinarians(IReadOnlyList<Specialization> specializations)
    {
        var specializationById = specializations.ToDictionary(specialization => specialization.Id);

        return
        [
            new Veterinarian
            {
                Id = 1,
                PassportNumber = "4512 345678",
                FullName = "Смирнов Алексей Иванович",
                Specialization = specializationById[1],
                BirthYear = 1985,
                ExperienceYears = 15
            },
            new Veterinarian
            {
                Id = 2,
                PassportNumber = "4513 221100",
                FullName = "Егорова Мария Павловна",
                Specialization = specializationById[2],
                BirthYear = 1990,
                ExperienceYears = 10
            },
            new Veterinarian
            {
                Id = 3,
                PassportNumber = "4514 778899",
                FullName = "Панин Сергей Викторович",
                Specialization = specializationById[4],
                BirthYear = 1978,
                ExperienceYears = 22
            },
            new Veterinarian
            {
                Id = 4,
                PassportNumber = "4515 553311",
                FullName = "Ларина Ольга Николаевна",
                Specialization = specializationById[3],
                BirthYear = 1992,
                ExperienceYears = 8
            },
            new Veterinarian
            {
                Id = 5,
                PassportNumber = "4516 909090",
                FullName = "Кузнецов Дмитрий Артёмович",
                Specialization = specializationById[5],
                BirthYear = 1995,
                ExperienceYears = 5
            },
            new Veterinarian
            {
                Id = 6,
                PassportNumber = "4517 161616",
                FullName = "Белкина Ирина Вадимовна",
                Specialization = specializationById[6],
                BirthYear = 1988,
                ExperienceYears = 12
            },
            new Veterinarian
            {
                Id = 7,
                PassportNumber = "4518 242424",
                FullName = "Родин Виктор Семёнович",
                Specialization = specializationById[7],
                BirthYear = 1975,
                ExperienceYears = 25
            },
            new Veterinarian
            {
                Id = 8,
                PassportNumber = "4519 313131",
                FullName = "Сафина Алина Рустамовна",
                Specialization = specializationById[8],
                BirthYear = 1993,
                ExperienceYears = 7
            },
            new Veterinarian
            {
                Id = 9,
                PassportNumber = "4520 424242",
                FullName = "Тихонов Егор Максимович",
                Specialization = specializationById[9],
                BirthYear = 1987,
                ExperienceYears = 13
            },
            new Veterinarian
            {
                Id = 10,
                PassportNumber = "4521 505050",
                FullName = "Успенская Вера Львовна",
                Specialization = specializationById[10],
                BirthYear = 1982,
                ExperienceYears = 17
            },
            new Veterinarian
            {
                Id = 11,
                PassportNumber = "4522 616161",
                FullName = "Фомин Илья Олегович",
                Specialization = specializationById[11],
                BirthYear = 1991,
                ExperienceYears = 9
            },
            new Veterinarian
            {
                Id = 12,
                PassportNumber = "4523 727272",
                FullName = "Хабиров Ренат Маратович",
                Specialization = specializationById[12],
                BirthYear = 1996,
                ExperienceYears = 4
            },
            new Veterinarian
            {
                Id = 13,
                PassportNumber = "4524 838383",
                FullName = "Царёв Никита Борисович",
                Specialization = specializationById[13],
                BirthYear = 1989,
                ExperienceYears = 11
            }
        ];
    }

    private static List<Appointment> CreateAppointments(IReadOnlyList<Pet> pets, IReadOnlyList<Veterinarian> veterinarians)
    {
        var petById = pets.ToDictionary(pet => pet.Id);
        var veterinarianById = veterinarians.ToDictionary(veterinarian => veterinarian.Id);

        return
        [
            new Appointment
            {
                Id = 1,
                Pet = petById[1],
                Veterinarian = veterinarianById[1],
                VisitDateTime = new DateTime(2026, 7, 6, 9, 30, 0),
                RoomNumber = "101",
                IsFollowUp = false
            },
            new Appointment
            {
                Id = 2,
                Pet = petById[3],
                Veterinarian = veterinarianById[4],
                VisitDateTime = new DateTime(2026, 7, 6, 11, 0, 0),
                RoomNumber = "102",
                IsFollowUp = false
            },
            new Appointment
            {
                Id = 3,
                Pet = petById[7],
                Veterinarian = veterinarianById[7],
                VisitDateTime = new DateTime(2026, 7, 10, 14, 0, 0),
                RoomNumber = "201",
                IsFollowUp = false
            },
            new Appointment
            {
                Id = 4,
                Pet = petById[1],
                Veterinarian = veterinarianById[1],
                VisitDateTime = new DateTime(2026, 7, 20, 10, 0, 0),
                RoomNumber = "101",
                IsFollowUp = true
            },
            new Appointment
            {
                Id = 5,
                Pet = petById[12],
                Veterinarian = veterinarianById[10],
                VisitDateTime = new DateTime(2026, 7, 22, 16, 30, 0),
                RoomNumber = "202",
                IsFollowUp = false
            },
            new Appointment
            {
                Id = 6,
                Pet = petById[2],
                Veterinarian = veterinarianById[6],
                VisitDateTime = new DateTime(2026, 8, 3, 12, 0, 0),
                RoomNumber = "103",
                IsFollowUp = false
            },
            new Appointment
            {
                Id = 7,
                Pet = petById[4],
                Veterinarian = veterinarianById[3],
                VisitDateTime = new DateTime(2026, 8, 5, 9, 0, 0),
                RoomNumber = "201",
                IsFollowUp = false
            },
            new Appointment
            {
                Id = 8,
                Pet = petById[4],
                Veterinarian = veterinarianById[3],
                VisitDateTime = new DateTime(2026, 8, 19, 9, 0, 0),
                RoomNumber = "201",
                IsFollowUp = true
            },
            new Appointment
            {
                Id = 9,
                Pet = petById[9],
                Veterinarian = veterinarianById[8],
                VisitDateTime = new DateTime(2026, 8, 11, 15, 0, 0),
                RoomNumber = "202",
                IsFollowUp = false
            },
            new Appointment
            {
                Id = 10,
                Pet = petById[10],
                Veterinarian = veterinarianById[9],
                VisitDateTime = new DateTime(2026, 8, 14, 10, 30, 0),
                RoomNumber = "103",
                IsFollowUp = false
            },
            new Appointment
            {
                Id = 11,
                Pet = petById[6],
                Veterinarian = veterinarianById[11],
                VisitDateTime = new DateTime(2026, 8, 25, 13, 0, 0),
                RoomNumber = "101",
                IsFollowUp = false
            },
            new Appointment
            {
                Id = 12,
                Pet = petById[15],
                Veterinarian = veterinarianById[12],
                VisitDateTime = new DateTime(2026, 8, 27, 11, 30, 0),
                RoomNumber = "102",
                IsFollowUp = false
            },
            new Appointment
            {
                Id = 13,
                Pet = petById[1],
                Veterinarian = veterinarianById[2],
                VisitDateTime = new DateTime(2026, 9, 2, 9, 0, 0),
                RoomNumber = "101",
                IsFollowUp = false
            },
            new Appointment
            {
                Id = 14,
                Pet = petById[5],
                Veterinarian = veterinarianById[5],
                VisitDateTime = new DateTime(2026, 9, 3, 10, 15, 0),
                RoomNumber = "102",
                IsFollowUp = false
            },
            new Appointment
            {
                Id = 15,
                Pet = petById[16],
                Veterinarian = veterinarianById[1],
                VisitDateTime = new DateTime(2026, 9, 4, 11, 45, 0),
                RoomNumber = "101",
                IsFollowUp = false
            },
            new Appointment
            {
                Id = 16,
                Pet = petById[8],
                Veterinarian = veterinarianById[7],
                VisitDateTime = new DateTime(2026, 9, 7, 14, 30, 0),
                RoomNumber = "201",
                IsFollowUp = false
            },
            new Appointment
            {
                Id = 17,
                Pet = petById[13],
                Veterinarian = veterinarianById[13],
                VisitDateTime = new DateTime(2026, 9, 8, 16, 0, 0),
                RoomNumber = "202",
                IsFollowUp = false
            },
            new Appointment
            {
                Id = 18,
                Pet = petById[2],
                Veterinarian = veterinarianById[6],
                VisitDateTime = new DateTime(2026, 9, 9, 12, 30, 0),
                RoomNumber = "103",
                IsFollowUp = true
            },
            new Appointment
            {
                Id = 19,
                Pet = petById[3],
                Veterinarian = veterinarianById[3],
                VisitDateTime = new DateTime(2026, 9, 10, 9, 30, 0),
                RoomNumber = "201",
                IsFollowUp = false
            },
            new Appointment
            {
                Id = 20,
                Pet = petById[15],
                Veterinarian = veterinarianById[3],
                VisitDateTime = new DateTime(2026, 9, 11, 10, 0, 0),
                RoomNumber = "201",
                IsFollowUp = true
            },
            new Appointment
            {
                Id = 21,
                Pet = petById[14],
                Veterinarian = veterinarianById[10],
                VisitDateTime = new DateTime(2026, 9, 14, 15, 30, 0),
                RoomNumber = "202",
                IsFollowUp = false
            },
            new Appointment
            {
                Id = 22,
                Pet = petById[11],
                Veterinarian = veterinarianById[9],
                VisitDateTime = new DateTime(2026, 9, 15, 11, 0, 0),
                RoomNumber = "103",
                IsFollowUp = false
            },
            new Appointment
            {
                Id = 23,
                Pet = petById[16],
                Veterinarian = veterinarianById[1],
                VisitDateTime = new DateTime(2026, 9, 17, 9, 45, 0),
                RoomNumber = "101",
                IsFollowUp = true
            },
            new Appointment
            {
                Id = 24,
                Pet = petById[6],
                Veterinarian = veterinarianById[11],
                VisitDateTime = new DateTime(2026, 9, 18, 13, 15, 0),
                RoomNumber = "101",
                IsFollowUp = true
            },
            new Appointment
            {
                Id = 25,
                Pet = petById[9],
                Veterinarian = veterinarianById[8],
                VisitDateTime = new DateTime(2026, 9, 21, 15, 0, 0),
                RoomNumber = "202",
                IsFollowUp = true
            },
            new Appointment
            {
                Id = 26,
                Pet = petById[5],
                Veterinarian = veterinarianById[5],
                VisitDateTime = new DateTime(2026, 9, 24, 10, 30, 0),
                RoomNumber = "102",
                IsFollowUp = true
            },
            new Appointment
            {
                Id = 27,
                Pet = petById[12],
                Veterinarian = veterinarianById[10],
                VisitDateTime = new DateTime(2026, 9, 28, 16, 45, 0),
                RoomNumber = "202",
                IsFollowUp = false
            },
            new Appointment
            {
                Id = 28,
                Pet = petById[1],
                Veterinarian = veterinarianById[1],
                VisitDateTime = new DateTime(2026, 9, 29, 9, 15, 0),
                RoomNumber = "101",
                IsFollowUp = true
            },
            new Appointment
            {
                Id = 29,
                Pet = petById[4],
                Veterinarian = veterinarianById[3],
                VisitDateTime = new DateTime(2026, 10, 1, 9, 0, 0),
                RoomNumber = "201",
                IsFollowUp = true
            },
            new Appointment
            {
                Id = 30,
                Pet = petById[10],
                Veterinarian = veterinarianById[9],
                VisitDateTime = new DateTime(2026, 10, 2, 10, 0, 0),
                RoomNumber = "103",
                IsFollowUp = false
            },
            new Appointment
            {
                Id = 31,
                Pet = petById[7],
                Veterinarian = veterinarianById[7],
                VisitDateTime = new DateTime(2026, 10, 5, 14, 0, 0),
                RoomNumber = "101",
                IsFollowUp = false
            }
        ];
    }
}
