using System.Threading.Tasks;
using HostelProperty.Client.Services;
using HostelProperty.DataAccess.Entities;

namespace HostelProperty.Client.Pages;

public partial class SelectResidentPage : ContentPage
{
	public SelectResidentPage(Guid roomId)
	{
		InitializeComponent();
	
		LoadResidents();

        RoomId = roomId;
    }

    public Guid RoomId { get; }

    public async void LoadResidents()
	{
        ResidentsCollection.ItemsSource = await ResidentService.GetResidents();
    }

    private async void searchResident_TextChanged(object sender, TextChangedEventArgs e)
    {
		var searchText = ((SearchBar)sender).Text.Trim().ToLower();

		if (string.IsNullOrEmpty(searchText))
		{
            var residentsAll = await ResidentService.GetResidents();

            ResidentsCollection.ItemsSource = residentsAll;

            return;
		}

		var searchedResident = await ResidentService.GetResidentsByFIO(searchText);

		ResidentsCollection.ItemsSource = searchedResident;
    }

    private async void SelectResident_Clicked(object sender, EventArgs e)
    {
		var contextBtn = (Button)sender;
		var contextResident = (Resident)contextBtn.BindingContext;

		var searchedRoom = await RoomService.GetById(contextResident.RoomId);

		if (searchedRoom == null)
			throw new Exception("Room not found");

		bool response;

		var currentRoom = await RoomService.GetById(RoomId);

		if (contextResident.RoomId == null)
		{
            response = await DisplayAlert("������������", $"�� ������ �������� \"{contextResident.FirstName} {contextResident.LastName} {contextResident.MiddleName}\" " +
                $"� ������� {currentRoom.Id}?", "��", "������");
        } 
		else
		{
			response = await DisplayAlert("������������", $"������ \"{contextResident.FirstName} {contextResident.LastName} {contextResident.MiddleName}\" " +
				$"��� ����� � ������� {searchedRoom.Title}. �� ������ �������� ���?", "��", "������");
		}

		if (response)
		{
			contextResident.RoomId = currentRoom.Id;

			try
			{
				await ResidentService.EditResident(contextResident.Id, contextResident);

				await Navigation.PopAsync();
			}
			catch (Exception ex)
			{
				await DisplayAlert("������!", ex.Message, "��");
			}
		}
    }
}