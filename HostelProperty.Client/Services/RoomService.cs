using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using HostelProperty.DataAccess.Entities;
using HostelProperty.Shared.Contracts;

namespace HostelProperty.Client.Services;

public static class RoomService
{
    public async static Task<List<Resident>> GetResidents(Guid roomId)
    {
        using (var client = new HttpClient())
        {
            var jwtToket = await SecureStorage.GetAsync("jwt");

            client.DefaultRequestHeaders.Add("Authorization", jwtToket);

            var response = await client.GetAsync($"https://localhost:7106/api/rooms/residents/{roomId}");

            if (response.IsSuccessStatusCode)
            {
                var residentsOfRoom = await response.Content.ReadFromJsonAsync<List<Resident>>();

                if (residentsOfRoom == null)
                {
                    throw new Exception("List residents is null");    
                }

                return residentsOfRoom; 
                
            } else
            {
                throw new Exception("Server error");
            }
        }
    }

    public async static Task<bool> DeleteResidentFromRoom(Guid residentId)
    {
        using (var client = new HttpClient())
        {
            var jwtToket = await SecureStorage.GetAsync("jwt");

            client.DefaultRequestHeaders.Add("Authorization", jwtToket);

            var response = await client.DeleteAsync($"https://localhost:7106/api/rooms/resident/delete/{residentId}");

            if (response.IsSuccessStatusCode)
            {
                return true;

            } else
            {
                return false;
            }
        }
    }

    public async static Task<List<RoomSubject>> GetRoomSubjects(Guid roomId)
    {
        using (var client = new HttpClient())
        {
            var jwtToket = await SecureStorage.GetAsync("jwt");

            client.DefaultRequestHeaders.Add("Authorization", jwtToket);

            var response = await client.GetAsync($"https://localhost:7106/api/rooms/roomsubjects/{roomId}");

            if (response.IsSuccessStatusCode)
            {
                var roomSubjectsOfRoom = await response.Content.ReadFromJsonAsync<List<RoomSubject>>();

                if (roomSubjectsOfRoom == null)
                {
                    throw new Exception("List residents is null");
                }

                return roomSubjectsOfRoom;

            }
            else
            {
                throw new Exception("Server error");
            }
        }
    }

    public async static Task<bool> DeleteRoomSubject(Guid subjectId)
    {
        using (var client = new HttpClient())
        {
            var jwtToket = await SecureStorage.GetAsync("jwt");

            client.DefaultRequestHeaders.Add("Authorization", jwtToket);

            var response = await client.DeleteAsync($"https://localhost:7106/api/rooms/roomsubject/delete/{subjectId}");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public async static Task<bool> ChangeRoom(Guid roomId, Room room)
    {
        using (var client = new HttpClient())
        {
            var jwtToket = await SecureStorage.GetAsync("jwt");

            client.DefaultRequestHeaders.Add("Authorization", jwtToket);

            var serializedRoom = JsonSerializer.Serialize(room);

            var stringContent = new StringContent(serializedRoom, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"https://localhost:7106/api/rooms/{roomId}", stringContent);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }
    }

    public async static Task<RoomDto?> GetById(Guid? roomId)
    {
        using (var client = new HttpClient())
        {
            var jwtToket = await SecureStorage.GetAsync("jwt");

            client.DefaultRequestHeaders.Add("Authorization", jwtToket);

            var response = await client.GetAsync($"https://localhost:7106/api/rooms/{roomId}");

            if (response.IsSuccessStatusCode)
            {
                var room = await response.Content.ReadFromJsonAsync<RoomDto>();

                if (room == null)
                {
                    throw new Exception("Room not found");
                }

                return room;
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }
    }

    public async static Task<List<RoomDto>?> GetByTitle(string title)
    {
        using var client = new HttpClient();

        var jwtToket = await SecureStorage.GetAsync("jwt");

        client.DefaultRequestHeaders.Add("Authorization", jwtToket);

        var response = await client.GetAsync($"https://localhost:7106/api/rooms/title/{title}");

        if (response.IsSuccessStatusCode)
        {
            var rooms = await response.Content.ReadFromJsonAsync<List<RoomDto>>();

            if (rooms == null)
            {
                throw new Exception("Rooms not found");
            }

            return rooms;
        }
        else
        {
            throw new Exception(await response.Content.ReadAsStringAsync());
        }
    }

    public async static Task<List<RoomDto>?> GetAll()
    {
        using var client = new HttpClient();

        var jwtToket = await SecureStorage.GetAsync("jwt");

        client.DefaultRequestHeaders.Add("Authorization", jwtToket);

        var response = await client.GetAsync($"https://localhost:7106/api/rooms/");

        if (response.IsSuccessStatusCode)
        {
            var rooms = await response.Content.ReadFromJsonAsync<List<RoomDto>>();

            if (rooms == null)
            {
                throw new Exception("Rooms not found");
            }

            return rooms;
        }
        else
        {
            throw new Exception(await response.Content.ReadAsStringAsync());
        }
    }
}
