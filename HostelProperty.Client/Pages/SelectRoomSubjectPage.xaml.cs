using System.Threading.Tasks;
using HostelProperty.Client.Services;
using HostelProperty.DataAccess.Entities;

namespace HostelProperty.Client.Pages;

public partial class SelectRoomSubjectPage : ContentPage
{
    public SelectRoomSubjectPage(Guid roomId)
    {
        InitializeComponent();

        LoadRoomSubjects();

        RoomId = roomId;
    }

    public Guid RoomId { get; }

    public async void LoadRoomSubjects()
    {
        RoomSubjectsCollection.ItemsSource = await RoomSubjectService.GetAll();
    }

    private async void searchRoomSubject_TextChanged(object sender, TextChangedEventArgs e)
    {
        var searchText = ((SearchBar)sender).Text.Trim().ToLower();

        if (string.IsNullOrEmpty(searchText))
        {
            return;
        }

        var searchedResident = await RoomSubjectService.GetById(searchText);

        RoomSubjectsCollection.ItemsSource = searchedResident;
    }

    private async void AddRoomSubjectBtn_Clicked(object sender, EventArgs e)
    {
        var contextBtn = (Button)sender;
        var contextRoomSubject = (RoomSubject)contextBtn.BindingContext;

        var searchedRoom = await RoomService.GetById(contextRoomSubject.RoomId);

        if (searchedRoom == null)
            throw new Exception("Room not found");

        bool response;

        var currentRoom = await RoomService.GetById(RoomId);

        if (contextRoomSubject.RoomId == null)
        {
            response = await DisplayAlert("Подтвеждение", $"Вы хотите добавить \"{contextRoomSubject.Title}\" " +
                $"в комнату {currentRoom.Id}?", "Да", "Отмена");
        }
        else
        {
            response = await DisplayAlert("Подтвеждение", $"Предмет \"{contextRoomSubject.Title}\" " +
                $"уже есть в комнате {searchedRoom.Title}. Вы хотите отвязать его?", "Да", "Отмена");
        }

        if (response)
        {
            contextRoomSubject.RoomId = currentRoom.Id;

            try
            {
                await RoomSubjectService.EditResident(contextRoomSubject.Id, contextRoomSubject);

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка!", ex.Message, "Ок");
            }
        }
    }
}