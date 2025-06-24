using HostelProperty.DataAccess.Entities;
using HostelProperty.DataAccess.Repositories;
using HostelProperty.Shared.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace HostelProperty.API.Endpoints;

public static class RoomEndpoints
{
    public static void MapRoomEndponts(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("api/rooms").WithTags("Rooms");

        group.MapGet("/", [Authorize] async (RoomRepository roomRepository) =>
        {
            var rooms = await roomRepository.GetAll();

            return Results.Ok(rooms);
        });

        group.MapGet("/{id}", [Authorize] async (Guid id, RoomRepository roomRepository) =>
        {
            var room = await roomRepository.GetById(id);

            return Results.Ok(room);
        });

        group.MapGet("/title/{title}", [Authorize] async (string title, RoomRepository roomRepository) =>
        {
            var rooms = await roomRepository.GetByTitle(title);

            return Results.Ok(rooms);
        });

        group.MapDelete("/resident/delete/{id}", [Authorize] async (Guid id, RoomRepository roomRepository) =>
        {
            try
            {
                await roomRepository.DeleteResidentFromRoom(id);

                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });

        group.MapGet("/residents/{roomId}", [Authorize] async (Guid roomId, RoomRepository roomRepository) =>
        {
            var searchedRoom = await roomRepository.GetById(roomId);

            if (searchedRoom == null)
            {
                return Results.BadRequest("Room not found");
            }

            var residents = searchedRoom.Residents;

            return Results.Ok(residents);
        });

        group.MapGet("/roomsubjects/{roomId}", [Authorize] async (Guid roomId, RoomRepository roomRepository) =>
        {
            var searchedRoom = await roomRepository.GetById(roomId);

            if (searchedRoom == null)
            {
                return Results.BadRequest("Room not found");
            }

            var roomSubjects = searchedRoom.RoomSubjects;

            return Results.Ok(roomSubjects);
        });

        group.MapDelete("/roomsubject/delete/{id}", [Authorize] async (Guid id, RoomRepository roomRepository) =>
        {
            await roomRepository.DeleteRoomSubjectFromRoom(id);

            return Results.Ok();
        });

        group.MapPut("/{roomId}", [Authorize] async (Guid roomId, Room room, RoomRepository roomRepository) =>
        {
            try
            {
                await roomRepository.Update(roomId, room);

                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }

        });
    }
}
