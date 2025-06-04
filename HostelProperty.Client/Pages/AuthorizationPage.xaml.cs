using System.Text;
using System.Text.Json;
using HostelProperty.Shared;
using HostelProperty.Shared.Contracts;

namespace HostelProperty.Client.Pages;

public partial class AuthorizationPage : ContentPage
{
	public AuthorizationPage()
	{
		InitializeComponent();
	}

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(UsernameEntry.Text))
        {
            await DisplayAlert("ќшибка", "¬ведите почту", "OK");

            return;
        }

        if (string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            await DisplayAlert("ќшибка", "¬ведите пароль", "OK");

            return;
        }

        LoadingIndicator.IsVisible = true;
        LoadingIndicator.IsRunning = true;
        LoginButton.IsEnabled = false;

        try
        {
            string email = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            string passwordHashed = Hash.CreateHash(password);  

            var authorizationModel = new AuthorizationDto(email, passwordHashed);

            var serializedModel = JsonSerializer.Serialize(authorizationModel);

            var httpClient = new HttpClient();

            var content = new StringContent(serializedModel, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://localhost:7106/authorization", content);

            if (response.IsSuccessStatusCode)
            {
                await Navigation.PushAsync(new MainPage());
            }
            else
            {
                //await DisplayAlert("ќшибка", , "OK");
            }    

        }
        catch (Exception)
        {

        }

    }

    private void OnShowPasswordChanged(object sender, CheckedChangedEventArgs e)
    {
        PasswordEntry.IsPassword = !e.Value;
    }
}