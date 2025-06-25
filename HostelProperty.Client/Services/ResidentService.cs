using HostelProperty.DataAccess.Entities;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace HostelProperty.Client.Services;

public static class ResidentService
{
    public async static Task<List<Resident>> GetResidents()
    {
        using (var client = new HttpClient())
        {
            var jwtToket = await SecureStorage.GetAsync("jwt");

            client.DefaultRequestHeaders.Add("Authorization", jwtToket);

            var response = await client.GetAsync($"https://localhost:7106/api/resident");

            if (response.IsSuccessStatusCode)
            {
                var residentsOfRoom = await response.Content.ReadFromJsonAsync<List<Resident>>();

                if (residentsOfRoom == null)
                {
                    throw new Exception("List residents is null");
                }

                return residentsOfRoom;

            }
            else
            {
                throw new Exception("Server error");
            }
        }
    }

    public async static Task<List<Resident>> GetResidentsByFIO(string searchText)
    {
        using var client = new HttpClient();

        var jwtToket = await SecureStorage.GetAsync("jwt");

        client.DefaultRequestHeaders.Add("Authorization", jwtToket);

        var response = await client.GetAsync($"https://localhost:7106/api/resident/fio/{searchText}");

        if (response.IsSuccessStatusCode)
        {
            var resident = await response.Content.ReadFromJsonAsync<List<Resident>>();

            if (resident == null)
            {
                throw new Exception("Resident not found");
            }

            return resident;

        }
        else
        {
            throw new Exception("Server error");
        }
    }

    public async static Task EditResident(Guid id, Resident editiedResident)
    {
        using (var client = new HttpClient())
        {
            var jwtToket = await SecureStorage.GetAsync("jwt");

            client.DefaultRequestHeaders.Add("Authorization", jwtToket);

            var serializedResident = JsonSerializer.Serialize(editiedResident);

            var stringContent = new StringContent(serializedResident, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"https://localhost:7106/api/resident/{id}", stringContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Server error");              
            }
        }
    }

    public static async Task AddResident(Resident newResident)
    {
        using (var client = new HttpClient())
        {
            var jwtToket = await SecureStorage.GetAsync("jwt");

            client.DefaultRequestHeaders.Add("Authorization", jwtToket);

            var serializedResident = JsonSerializer.Serialize(newResident);

            var stringContent = new StringContent(serializedResident, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"https://localhost:7106/api/resident/", stringContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Server error");
            }
        }
    }
}
