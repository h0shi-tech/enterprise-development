using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using VetClinic.Contracts.Appointments;
using VetClinic.Contracts.Breeds;
using VetClinic.Contracts.Owners;
using VetClinic.Contracts.Pets;
using VetClinic.Domain.Enums;

namespace VetClinic.Tests;

public class VetClinicApiTests
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private static CancellationToken Cancel => TestContext.Current.CancellationToken;

    [Fact]
    public async Task GetBreeds_ReturnsSeed()
    {
        await using var factory = new VetClinicApiFactory();
        var breeds = await factory.CreateClient().GetFromJsonAsync<List<BreedDto>>("/api/breeds", JsonOptions, Cancel);

        Assert.NotNull(breeds);
        Assert.Equal(14, breeds.Count);
    }

    [Fact]
    public async Task PostBreed_WithoutSpecies_ReturnsBadRequest()
    {
        await using var factory = new VetClinicApiFactory();
        var response = await factory.CreateClient().PostAsJsonAsync("/api/breeds", new { name = "Беспородная" }, Cancel);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostBreed_ReturnsCreatedBreed()
    {
        await using var factory = new VetClinicApiFactory();
        var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/api/breeds",
            new BreedEditDto { Name = "Мопс", Species = AnimalSpecies.Dog },
            JsonOptions,
            Cancel);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<BreedDto>(JsonOptions, Cancel);
        Assert.NotNull(created);
        Assert.Equal("Мопс", created.Name);
        Assert.Equal(AnimalSpecies.Dog, created.Species);
    }

    [Fact]
    public async Task Analytics_ReturnsCatVeterinariansAndOwners()
    {
        await using var factory = new VetClinicApiFactory();
        var client = factory.CreateClient();

        var veterinarians = await client.GetFromJsonAsync<List<VeterinarianJson>>(
            "/api/analytics/veterinarians?species=Cat",
            JsonOptions,
            Cancel);
        var owners = await client.GetFromJsonAsync<List<OwnerDto>>(
            "/api/analytics/owners/with-several-pets",
            JsonOptions,
            Cancel);
        var pets = await client.GetFromJsonAsync<List<PetDto>>("/api/veterinarians/3/pets", JsonOptions, Cancel);

        Assert.NotNull(veterinarians);
        Assert.Equal([1, 2, 6, 11], veterinarians.Select(veterinarian => veterinarian.Id).ToArray());
        Assert.NotNull(owners);
        Assert.Equal([2, 5, 7, 1], owners.Select(owner => owner.Id).ToArray());
        Assert.NotNull(pets);
        Assert.Equal([4, 3, 15], pets.Select(pet => pet.Id).ToArray());
    }

    [Fact]
    public async Task DeleteUsedBreed_ReturnsConflict()
    {
        await using var factory = new VetClinicApiFactory();
        var response = await factory.CreateClient().DeleteAsync("/api/breeds/1", Cancel);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task PostAppointment_WithMismatchedSpecies_ReturnsBadRequest()
    {
        await using var factory = new VetClinicApiFactory();
        var response = await factory.CreateClient().PostAsJsonAsync(
            "/api/appointments",
            new AppointmentEditDto
            {
                PetId = 1,
                VeterinarianId = 3,
                VisitDateTime = new DateTime(2026, 10, 20, 10, 0, 0),
                RoomNumber = "101"
            },
            JsonOptions,
            Cancel);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetOwnerPets_ReturnsPetsOfOwner()
    {
        await using var factory = new VetClinicApiFactory();
        var pets = await factory.CreateClient().GetFromJsonAsync<List<PetDto>>("/api/owners/1/pets", JsonOptions, Cancel);

        Assert.NotNull(pets);
        Assert.Equal([2, 1], pets.Select(pet => pet.Id).ToArray());
    }

    private sealed class VeterinarianJson
    {
        public int Id { get; set; }
    }
}

public sealed class VetClinicApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
    }
}
