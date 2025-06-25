using HostelProperty.Client.Services;
using HostelProperty.DataAccess.Entities;

namespace HostelProperty.Client.Pages;

public partial class CreateRoomSubjectPage : ContentPage
{
	public CreateRoomSubjectPage(
		Guid RoomId)
	{
		InitializeComponent();
        this.RoomId = RoomId;
    }

    public Guid RoomId { get; }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TitleObject.Text))
        {
            await DisplayAlert("Ошибка", "Все обязательные поля должны быть заполнены", "OK");

            return;
        }

        var newRoomSubject = new RoomSubject
        {
            Id = Guid.NewGuid(),
            Title = TitleObject.Text,
            DateReseption = DateTime.Now,
            RoomId = RoomId
        };

        try
        {
            await RoomSubjectService.AddResident(newRoomSubject);

            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Не удалось сохранить жильца: {ex.Message}", "Oк");
        }
    }
}