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

        var result = await DisplayAlert("Подтвеждение.", $"Вы действительно хотите удалить {contextResident.LastName} {contextResident.FirstName} {contextResident.MiddleName} из комнаты {this.ContextRoom.Title}?", "Да", "Отмена");

        if (result)
        {
            var response = await RoomService.DeleteResidentFromRoom(contextResident.Id);

            if (response)
            {
                ResidentsCollection.ItemsSource = await RoomService.GetResidents(this.ContextRoom.Id);

            }
            else
            {
                await DisplayAlert("Ошибка!", "Ошибка удаления.", "Ок");
            }
        }
    }

    private async void DeleteRoomObject_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;

        var contextRoomSubject = (RoomSubject)button.BindingContext;

        var result = await DisplayAlert("Подтвеждение.", $"Вы действительно хотите удалить \"{contextRoomSubject.Title}\" из комнаты {this.ContextRoom.Title}?", "Да", "Отмена");

        if (result)
        {
            var response = await RoomService.DeleteRoomSubject(contextRoomSubject.Id);

            if (response)
            {
                RoomSubjectsCollection.ItemsSource = await RoomService.GetRoomSubjects(this.ContextRoom.Id);
            }
            else
            {
                await DisplayAlert("Ошибка!", "Ошибка удаления.", "Ок");
            }
        }
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        var title = TitleRoom.Text;

        if (string.IsNullOrWhiteSpace(title))
        {
            await DisplayAlert("Ошибка!", "Поле \"Номер\" не должно быть пустым!", "Ок");

            return;
        }

        var countResidents = CountResidents.Text;

        if (string.IsNullOrWhiteSpace(countResidents))
        {
            await DisplayAlert("Ошибка!", "Поле \"Количество проживающих\" не должно быть пустым!", "Ок");

            return;
        }

        var floor = Floor.Text;

        if (string.IsNullOrWhiteSpace(floor))
        {
            await DisplayAlert("Ошибка!", "Поле \"Этаж\" не должно быть пустым!", "Ок");

            return;
        }

        var room = new Room();

        room.Title = title;
        room.CountResidents = Byte.Parse(countResidents);
        room.Floor = Byte.Parse(floor);

        try
        {
            var response = await RoomService.ChangeRoom(this.ContextRoom.Id, room);
    
            await DisplayAlert("Успешно!", "Данные обновлены.", "Ок");
            
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка!", ex.Message, "Ок");
        }

    }
}