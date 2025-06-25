using HostelProperty.DataAccess.Entities;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace HostelProperty.Client.Services;
public static class RoomSubjectService
{
    public static async Task<List<RoomSubject>?> GetAll()
    {
        using var client = new HttpClient();

        var jwtToket = await SecureStorage.GetAsync("jwt");

        client.DefaultRequestHeaders.Add("Authorization", jwtToket);

        var response = await client.GetAsync($"https://localhost:7106/api/roomsubjects");

        if (response.IsSuccessStatusCode)
        {
            var rooms = await response.Content.ReadFromJsonAsync<List<RoomSubject>>();

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

    public static async Task<List<RoomSubject>?> GetById(string id)
    {
        using var client = new HttpClient();

        var jwtToket = await SecureStorage.GetAsync("jwt");

        client.DefaultRequestHeaders.Add("Authorization", jwtToket);

        var response = await client.GetAsync($"https://localhost:7106/api/roomsubjects/id/{id}");

        if (response.IsSuccessStatusCode)
        {
            var rooms = await response.Content.ReadFromJsonAsync<List<RoomSubject>>();

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

    public static async Task<List<RoomSubject>?> GetByTitle(string title)
    {
        using var client = new HttpClient();

        var jwtToket = await SecureStorage.GetAsync("jwt");

        client.DefaultRequestHeaders.Add("Authorization", jwtToket);

        var response = await client.GetAsync($"https://localhost:7106/api/roomsubjects/title/{title}");

        if (response.IsSuccessStatusCode)
        {
            var rooms = await response.Content.ReadFromJsonAsync<List<RoomSubject>>();

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

    public static async Task EditResident(Guid id, RoomSubject editiedRoomSubject)
    {
        using (var client = new HttpClient())
        {
            var jwtToket = await SecureStorage.GetAsync("jwt");

            client.DefaultRequestHeaders.Add("Authorization", jwtToket);

            var serializedRoomSubject = JsonSerializer.Serialize(editiedRoomSubject);

            var stringContent = new StringContent(serializedRoomSubject, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"https://localhost:7106/api/roomsubjects/{id}", stringContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Server error");
            }
        }
    }

    public static async Task AddResident(RoomSubject newRoomSubject)
    {
        using (var client = new HttpClient())
        {
            var jwtToket = await SecureStorage.GetAsync("jwt");

            client.DefaultRequestHeaders.Add("Authorization", jwtToket);

            var serializedResident = JsonSerializer.Serialize(newRoomSubject);

            var stringContent = new StringContent(serializedResident, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"https://localhost:7106/api/roomsubjects/", stringContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Server error");
            }
        }
    }
}
