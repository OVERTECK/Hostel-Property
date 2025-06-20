using System.Threading.Tasks;
using HostelProperty.Client.Services;
using HostelProperty.DataAccess.Entities;
using HostelProperty.Shared.Contracts;

namespace HostelProperty.Client.Pages;

public partial class EditRoomPage : ContentPage
{
    private RoomDto ContextRoom { get; }

    public EditRoomPage(
        RoomDto contextRoom)
    {
        InitializeComponent();

        this.ContextRoom = contextRoom;

        TitleRoom.Text = contextRoom.Title.ToString();
        CountResidents.Text = contextRoom.CountResidents.ToString();
        Floor.Text = contextRoom.Floor.ToString();

        ResidentsCollection.ItemsSource = contextRoom.Residents;

        RoomSubjectsCollection.ItemsSource = contextRoom.RoomSubjects;
    }

    private async void DeleteResident_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;

        var contextResident = (Resident)button.BindingContext;

        var result = await DisplayAlert("������������.", $"�� ������������� ������ ������� {contextResident.LastName} {contextResident.FirstName} {contextResident.MiddleName} �� ������� {this.ContextRoom.Title}?", "��", "������");

        if (result)
        {
            var response = await RoomService.DeleteResidentFromRoom(contextResident.Id);

            if (response)
            {
                ResidentsCollection.ItemsSource = await RoomService.GetResidents(this.ContextRoom.Id);

            }
            else
            {
                await DisplayAlert("������!", "������ ��������.", "��");
            }
        }
    }

    private async void DeleteRoomObject_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;

        var contextRoomSubject = (RoomSubject)button.BindingContext;

        var result = await DisplayAlert("������������.", $"�� ������������� ������ ������� \"{contextRoomSubject.Title}\" �� ������� {this.ContextRoom.Title}?", "��", "������");

        if (result)
        {
            var response = await RoomService.DeleteRoomSubject(contextRoomSubject.Id);

            if (response)
            {
                RoomSubjectsCollection.ItemsSource = await RoomService.GetRoomSubjects(this.ContextRoom.Id);
            }
            else
            {
                await DisplayAlert("������!", "������ ��������.", "��");
            }
        }
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        var title = TitleRoom.Text;

        if (string.IsNullOrWhiteSpace(title))
        {
            await DisplayAlert("������!", "���� \"�����\" �� ������ ���� ������!", "��");

            return;
        }

        var countResidents = CountResidents.Text;

        if (string.IsNullOrWhiteSpace(countResidents))
        {
            await DisplayAlert("������!", "���� \"���������� �����������\" �� ������ ���� ������!", "��");

            return;
        }

        var floor = Floor.Text;

        if (string.IsNullOrWhiteSpace(floor))
        {
            await DisplayAlert("������!", "���� \"����\" �� ������ ���� ������!", "��");

            return;
        }

        var room = new Room();

        room.Title = title;
        room.CountResidents = Byte.Parse(countResidents);
        room.Floor = Byte.Parse(floor);

        try
        {
            var response = await RoomService.ChangeRoom(this.ContextRoom.Id, room);
    
            await DisplayAlert("�������!", "������ ���������.", "��");
            
        }
        catch (Exception ex)
        {
            await DisplayAlert("������!", ex.Message, "��");
        }

    }
}