using HostelProperty.DataAccess;
using HostelProperty.DataAccess.Entities;
using HostelProperty.DataAccess.Repositories;

namespace HostelProperty.API.Endpoints;

public static class ResidentEndpoints
{
    public static void MapResidentEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("api/resident").WithTags(nameof(Resident));

        group.MapGet("/", async (ResidentRepository residentRepository) =>
        {
            return await residentRepository.GetAll();
        });

        group.MapGet("/{id}", async (Guid idResident, ResidentRepository residentRepository) =>
        {
            var searchedResident = await residentRepository.GetById(idResident);

            if (searchedResident != null)
                return Results.Ok(searchedResident);
            else
                return Results.NotFound();
        });

        group.MapPost("/", async (Resident resident, ResidentRepository residentRepository) =>
        {
            await residentRepository.Add(
                resident.FirstName,
                resident.MiddleName,
                resident.LastName,
                resident.Age,
                resident.NumberCourse,
                resident.RoomNumber);

            return Results.Created();
        });

        group.MapPut("/{id}", async (Guid idResident, Resident resident, ResidentRepository residentRepository) =>
        {
            var searchedResident = residentRepository.GetById(idResident);

            if (searchedResident == null)
                return Results.NotFound();
            else
            {
                await residentRepository.Update(idResident, resident);

                return Results.Ok();
            }
        });

        group.MapDelete("/{id}", async (Guid idResident, ResidentRepository residentRepository) =>
        {
            await residentRepository.Delete(idResident);

            return Results.Ok();
        });
    }
}
