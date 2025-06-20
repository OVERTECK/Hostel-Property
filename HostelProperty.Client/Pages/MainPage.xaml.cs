using System.Net.Http.Json;
using System.Threading.Tasks;
using HostelProperty.Shared.Contracts;

namespace HostelProperty.Client.Pages;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    public async void LoadRooms()
    {
        try
        {
            collectionViewRooms.IsVisible = false;

            LoadIndicator.IsVisible = true;
            LoadIndicator.IsRunning = true;

            var httpClient = new HttpClient();

            var jwtToket = await SecureStorage.GetAsync("jwt");

            httpClient.DefaultRequestHeaders.Add("Authorization", jwtToket);

            var response = await httpClient.GetAsync("https://localhost:7106/api/rooms");

            if (response.IsSuccessStatusCode)
            {
                var rooms = await response.Content.ReadFromJsonAsync<List<RoomDto>>();

                collectionViewRooms.ItemsSource = rooms;

                collectionViewRooms.IsVisible = true;

                LoadIndicator.IsVisible = false;
                LoadIndicator.IsRunning = false;

            } else
            {
                throw new Exception("Сервер не отвечает");
            }
        }
        catch (Exception)
        {
            throw new Exception("Ошибка загрузки данных.");
        }
    }

    private void collectionViewRooms_Loaded(object sender, EventArgs e)
    {
        LoadRooms();
    }

    private async void EditButton_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;

        var context = (RoomDto)button.BindingContext;

        await Navigation.PushAsync(new EditRoomPage(context));
    }
}
