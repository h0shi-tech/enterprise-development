# Ветеринарная клиника

Лабораторные по курсу «Разработка корпоративных приложений», вариант 51.
Репозиторий курса: https://github.com/itsecd/enterprise-development

Предметная область — ветеринарная клиника. База в третьей лабораторной будет PostgreSQL, обмен с генератором данных в четвёртой — Kafka.

## О чём проект

Клиника записывает питомцев на приём к врачам.

У питомца есть кличка, порода, дата рождения и вес. Порода — справочник и привязана к виду животного, поэтому вид питомца берётся из породы. У владельца ФИО, телефон и адрес, животных может быть несколько. У врача номер паспорта, ФИО, год рождения, стаж и специализация. Специализация тоже справочник и тоже с видом животных.

Запись на приём связывает питомца и врача: когда, в каком кабинете и повторный это приём или нет. В следующих лабораторных эта запись будет контрактом.

## Лабораторная 1

Объектная модель и тестовые данные в памяти: 14 пород, 13 специализаций, 12 владельцев, 16 питомцев, 13 врачей и 31 запись. Пять запросов из задания — в `tests/VetClinic.Tests/VetClinicQueryTests.cs`.

«Текущий месяц» там не берётся из `DateTime.Now`. В данных есть опорная дата `VetClinicSeedData.ReferenceDate`, 15 сентября 2026. Иначе набор приёмов разъехался бы с днём, когда запускаешь тесты.

## Лабораторная 2

Сервер с CRUD по тем же сущностям и с теми же аналитическими запросами. Хранение пока в памяти, стартовые данные те же, из `VetClinicSeedData`.

Вид у породы и специализации nullable и обязателен при создании. Если его не передать, у перечисления подставится первый элемент, и это уже не «поле пустое».

Номера выдаёт счётчик. Удалённый идентификатор заново не используется.

Запуск:

```
dotnet run --project src/VetClinic.Api
```

Swagger: http://localhost:5081/swagger

Маршруты:

- CRUD: `/api/breeds`, `/api/specializations`, `/api/owners`, `/api/pets`, `/api/veterinarians`, `/api/appointments`
- питомцы владельца: `/api/owners/{id}/pets`
- приёмы питомца: `/api/pets/{id}/appointments`
- приёмы и питомцы врача: `/api/veterinarians/{id}/appointments`, `/api/veterinarians/{id}/pets`
- врачи специализации: `/api/specializations/{id}/veterinarians`
- породы, специализации и врачи вида: `/api/species/{species}/breeds`, `/api/species/{species}/specializations`, `/api/species/{species}/veterinarians`
- аналитика: `/api/analytics/veterinarians?species=Cat`, `/api/analytics/veterinarians/{id}/pets`, `/api/analytics/breeds/{id}/follow-ups`, `/api/analytics/owners/with-several-pets`, `/api/analytics/appointments?roomNumber=101`

Запрос приёмов за текущий месяц на сервере смотрит локальное время процесса. В тесте этого запроса время зафиксировано на 15 сентября 2026, чтобы кабинет 101 сверялся с сентябрьскими записями.

## Сборка и тесты

Нужен .NET SDK 10.

```
dotnet build VetClinic.slnx --configuration Release
dotnet test --solution VetClinic.slnx --configuration Release --no-build
```

Ключ `--solution` нужен, потому что тесты запускаются через Microsoft Testing Platform.

Проекты: `VetClinic.Domain` — сущности и данные, `VetClinic.Contracts` — DTO, `VetClinic.Application` — службы, `VetClinic.Infrastructure.InMemory` — репозитории в памяти, `VetClinic.Api` — сервер, `VetClinic.Tests` — тесты.
