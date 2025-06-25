using System.Net.Http.Json;
using System.Threading.Tasks;
using HostelProperty.Client.Services;
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

    private async void searchRoom_TextChanged(object sender, TextChangedEventArgs e)
    {
        var textSearch = searchRoom.Text;

        if (string.IsNullOrEmpty(textSearch))
        {
            var defaultRooms = await RoomService.GetAll();

            collectionViewRooms.ItemsSource = defaultRooms;

            return;
        }

        var searchedRooms = await RoomService.GetByTitle(textSearch);

        collectionViewRooms.ItemsSource = searchedRooms;
    }

    private void SortPicker_Loaded(object sender, EventArgs e)
    {


        var floors = new List<FloorDto>
        {
            new FloorDto(0, "Все этажи"),
            new FloorDto(1, "1 этаж"),
            new FloorDto(2, "2 этаж"),
            new FloorDto(3, "3 этаж"),
            new FloorDto(4, "4 этаж"),
        };

        SortPicker.ItemsSource = floors;

        SortPicker.SelectedItem = floors[0];
    }

    private async void SortPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedFloor = SortPicker.SelectedItem as FloorDto;
    
        if (selectedFloor.Id == 0)
        {
            collectionViewRooms.ItemsSource = await RoomService.GetAll();

            return;
        }
        else
        {
            collectionViewRooms.ItemsSource = await RoomService.GetByFloor((byte)selectedFloor.Id);

            return;
        }
    }

    private async void DeleteRoomBtn_Clicked(object sender, EventArgs e)
    {
        var contextBtn = (Button)sender;

        var contextRoom = (RoomDto)contextBtn.BindingContext;

        var response = await DisplayAlert("Подтвеждение", $"Вы уверены, что хотите удалить комнату {contextRoom.Title}?", "Да", "Отмена");

        if (response)
            await RoomService.DeleteById(contextRoom.Id);

        collectionViewRooms.ItemsSource = await RoomService.GetAll();
    }
}
