using NuGet.Configuration;
using Microsoft.AspNetCore.Authorization;
using HostelProperty.DataAccess.Entities;
using HostelProperty.DataAccess.Repositories;

namespace HostelProperty.API.Endpoints;

public static class RoomSubjectEndpoints
{
    public static void MapRoomSubjectEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("api/roomsubjects").WithTags("Room subjects");

        group.MapGet("/", [Authorize] async (RoomSubjectRepository roomSubjectRepository) =>
        {
            var roomSubjects = await roomSubjectRepository.GetAll();

            return Results.Ok(roomSubjects);
        });

        group.MapGet("/id/{id}", [Authorize] async (string id, RoomSubjectRepository roomSubjectRepository) =>
        {
            var roomSubjects = await roomSubjectRepository.GetById(id);

            return Results.Ok(roomSubjects);
        });

        group.MapGet("/title/{title}", [Authorize] async (string title, RoomSubjectRepository roomSubjectRepository) =>
        {
            var roomSubjects = await roomSubjectRepository.GetByTitle(title);

            return Results.Ok(roomSubjects);
        });

        group.MapPut("/{id}", [Authorize] async (Guid id, RoomSubject roomSubject, RoomSubjectRepository roomSubjectRepository) =>
        {
            await roomSubjectRepository.Update(id, roomSubject);

            return Results.Ok();
        });

        group.MapPost("/", async (RoomSubject roomSubject, RoomSubjectRepository roomSubjectRepository) =>
        {
            await roomSubjectRepository.Add(roomSubject);

            return Results.Created();
        });
    }
}
