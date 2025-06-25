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
            response = await DisplayAlert("������������", $"�� ������ �������� \"{contextRoomSubject.Title}\" " +
                $"� ������� {currentRoom.Id}?", "��", "������");
        }
        else
        {
            response = await DisplayAlert("������������", $"������� \"{contextRoomSubject.Title}\" " +
                $"��� ���� � ������� {searchedRoom.Title}. �� ������ �������� ���?", "��", "������");
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
                await DisplayAlert("������!", ex.Message, "��");
            }
        }
    }
}