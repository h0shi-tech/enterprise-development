using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore;
using VetClinic.Api.ExceptionHandling;
using VetClinic.Application;
using VetClinic.Infrastructure.InMemory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddInMemoryClinic();
builder.Services.AddVetClinicApplication();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsEnvironment("Testing"))
{
    app.UseHttpsRedirection();
}

app.MapControllers();
app.Run();

public partial class Program;
