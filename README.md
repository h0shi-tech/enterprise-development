# Ветеринарная клиника

Проект по дисциплине «Разработка корпоративных приложений» ([задание курса](https://github.com/itsecd/enterprise-development)).

| Параметр | Значение |
| --- | --- |
| Вариант | 51 |
| Предметная область | Ветеринарная клиника |
| База данных | PostgreSQL (лабораторная 3) |
| Обмен с генератором данных | Kafka (лабораторная 4) |

## Предметная область

Клиника ведёт учёт записей питомцев на приём к ветеринарным врачам.

Питомец описывается кличкой, породой, датой рождения и весом. Порода — справочник, привязанный
к биологическому виду животного, поэтому вид питомца определяется его породой. Владелец описывается
ФИО, телефоном и адресом; у одного владельца может быть несколько питомцев. Врач описывается номером
паспорта, ФИО, годом рождения, стажем работы и специализацией — справочником врачебных профилей
клиники с указанием вида животных, который принимает врач.

Запись на приём связывает питомца и врача и фиксирует дату и время приёма, номер кабинета
и признак повторного приёма. Именно запись на приём используется в дальнейшем в качестве контракта.

## Реализовано в лабораторной 1

- Объектная модель предметной области.
- Набор тестовых данных в памяти: 14 пород, 13 специализаций, 12 владельцев, 16 питомцев,
  13 врачей и 31 запись на приём.
- Пять аналитических запросов на LINQ, оформленных в виде unit-тестов, и проверки самого
  набора данных.
- Запуск сборки и тестов в GitHub Actions.

## Реализовано в лабораторной 2

Сервер на ASP.NET Core отдаёт CRUD по сущностям первой лабораторной и те же аналитические
запросы. Данные по-прежнему лежат в памяти, стартовый набор берётся из `VetClinicSeedData`.

Биологический вид у породы и специализации хранится как nullable и обязателен при создании.
Так пропущенное значение не превращается в первый элемент перечисления.

Новый идентификатор берётся из счётчика, который увеличивается при создании. Удалённый номер
повторно не выдаётся.

Запуск:

```
dotnet run --project src/VetClinic.Api
```

Swagger: `http://localhost:5081/swagger`.

| Метод | Маршрут |
| --- | --- |
| CRUD | `/api/breeds`, `/api/specializations`, `/api/owners`, `/api/pets`, `/api/veterinarians`, `/api/appointments` |
| Связанные коллекции | `/api/owners/{id}/pets`, `/api/pets/{id}/appointments`, `/api/veterinarians/{id}/appointments`, `/api/veterinarians/{id}/pets`, `/api/specializations/{id}/veterinarians`, `/api/species/{species}/breeds`, `/api/species/{species}/specializations`, `/api/species/{species}/veterinarians` |
| Врачи выбранного вида | `GET /api/analytics/veterinarians?species=Cat` |
| Питомцы выбранного врача, по кличке | `GET /api/analytics/veterinarians/{id}/pets` |
| Число повторных приёмов породы | `GET /api/analytics/breeds/{id}/follow-ups` |
| Владельцы с несколькими питомцами | `GET /api/analytics/owners/with-several-pets` |
| Приёмы текущего месяца в кабинете | `GET /api/analytics/appointments?roomNumber=101` |

Текущий месяц считается по локальному времени процесса. В наборе данных приёмы сентября 2026
года подобраны под опорную дату `VetClinicSeedData.ReferenceDate`, поэтому unit-тест этого
запроса фиксирует время на 15 сентября 2026.

## Структура решения

```
VetClinic.slnx                    решение
global.json                       включение MTP-режима dotnet test
src/VetClinic.Domain/             доменная библиотека
    Entities/                     сущности предметной области
    Enums/                        перечисления
    Data/                         набор тестовых данных
    Interfaces/                   интерфейсы репозиториев
src/VetClinic.Contracts/          DTO и интерфейсы служб
src/VetClinic.Application/        службы приложения
src/VetClinic.Infrastructure.InMemory/  репозитории в памяти
src/VetClinic.Api/                ASP.NET Core хост
tests/VetClinic.Tests/            unit-тесты на xUnit v3
```

### Доменная модель

| Сущность | Назначение | Связи |
| --- | --- | --- |
| `Breed` | справочник пород | принадлежит виду `AnimalSpecies` |
| `Specialization` | справочник врачебных профилей | относится к виду `AnimalSpecies` |
| `Owner` | владелец питомцев | содержит список `Pet` |
| `Pet` | питомец клиники | ссылается на `Breed` и `Owner` |
| `Veterinarian` | ветеринарный врач | ссылается на `Specialization` |
| `Appointment` | запись на приём, контракт | ссылается на `Pet` и `Veterinarian` |

## Аналитические запросы

Запросы из задания реализованы в `tests/VetClinic.Tests/VetClinicQueryTests.cs`. Ожидаемые
результаты заданы явными идентификаторами, поэтому тесты проверяют соответствие реальным данным,
а не работу самих методов LINQ.

| Запрос | Тест |
| --- | --- |
| Врачи, специализирующиеся на выбранном виде животных | `VeterinariansOfSelectedSpecies` |
| Питомцы, записанные к указанному врачу, по кличке | `PetsOfSelectedVeterinarianOrderedByName` |
| Количество повторных приёмов выбранной породы | `FollowUpCountForSelectedBreed` |
| Владельцы с более чем одним питомцем, по ФИО | `OwnersWithSeveralPetsOrderedByFullName` |
| Приёмы текущего месяца в выбранном кабинете | `CurrentMonthAppointmentsInSelectedRoom` |

Запрос о приёмах текущего месяца опирается не на `DateTime.Now`, а на опорную дату набора данных
`VetClinicSeedData.ReferenceDate`, поэтому результат теста воспроизводим при любом запуске.

## Сборка и запуск тестов

Требуется .NET SDK 10.

```
dotnet build VetClinic.slnx --configuration Release
dotnet test --solution VetClinic.slnx --configuration Release --no-build
dotnet run --project src/VetClinic.Api
```

Тесты используют Microsoft Testing Platform, поэтому решение и передаётся ключом `--solution`.
