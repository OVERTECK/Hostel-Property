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
            var residents = await residentRepository.GetAll();

            if (residents == null)
                return Results.NotFound();

            return Results.Ok(residents);
        });

        group.MapGet("/fio/{searchText}", async (string searchText, ResidentRepository residentRepository) =>
        {
            var searchedResidents = await residentRepository.GetByFIO(searchText);

            if (searchedResidents == null)
                return Results.NotFound();

            return Results.Ok(searchedResidents);
        });

        group.MapGet("/{id}", async (Guid id, ResidentRepository residentRepository) =>
        {
            var searchedResident = await residentRepository.GetById(id);

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
                resident.RoomId);

            return Results.Created();
        });

        group.MapPut("/{id}", async (Guid id, Resident resident, ResidentRepository residentRepository) =>
        {
            try
            {
                await residentRepository.Update(id, resident);

                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }

        });

        group.MapDelete("/{id}", async (Guid id, ResidentRepository residentRepository) =>
        {
            await residentRepository.Delete(id);

            return Results.Ok();
        });
    }
}
