using HostelProperty.Client.Services;
using HostelProperty.DataAccess.Entities;

namespace HostelProperty.Client.Pages;

public partial class CreateResidentPage : ContentPage
{
	public CreateResidentPage(
        Guid roomId)
	{
		InitializeComponent();

        RoomId = roomId;
    }

    public Guid RoomId { get; }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(LastNameEntry.Text) ||
            string.IsNullOrWhiteSpace(FirstNameEntry.Text) ||
            string.IsNullOrWhiteSpace(AgeEntry.Text) ||
            string.IsNullOrWhiteSpace(CourseNumberEntry.Text))
        {
            await DisplayAlert("������", "��� ������������ ���� ������ ���� ���������", "OK");

            return;
        }

        // �������� �������� �����
        if (!int.TryParse(AgeEntry.Text, out int age) || age <= 0)
        {
            await DisplayAlert("������", "������� ������ ���� ������������� ������", "OK");

            return;
        }

        if (!int.TryParse(CourseNumberEntry.Text, out int courseNumber) || courseNumber <= 0)
        {
            await DisplayAlert("������", "����� ����� ������ ���� ������������� ������", "OK");

            return;
        }

        // �������� ������ ������
        var newResident = new Resident
        {
            LastName = LastNameEntry.Text,
            FirstName = FirstNameEntry.Text,
            MiddleName = MiddleNameEntry.Text,
            Age = Byte.Parse(AgeEntry.Text),
            NumberCourse = Byte.Parse(CourseNumberEntry.Text),
            RoomId = RoomId
        };

        try
        {
            await ResidentService.AddResident(newResident);

            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("������", $"�� ������� ��������� ������: {ex.Message}", "O�");
        }
    }
}