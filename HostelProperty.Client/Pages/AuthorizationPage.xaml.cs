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
            await DisplayAlert("Ошибка", "Введите почту", "OK");

            return;
        }

        if (string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            await DisplayAlert("Ошибка", "Введите пароль", "OK");

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
                var token = await response.Content.ReadAsStringAsync();

                token = token.Trim('"');

                await SecureStorage.SetAsync("jwt", "Bearer " + token);

                await Navigation.PushModalAsync(new NavigationPage(new MainPage()));

                LoadingIndicator.IsVisible = false;
                LoadingIndicator.IsRunning = false;
                LoginButton.IsEnabled = true;
            }
            else
            {
                await DisplayAlert("Ошибка", "Неверная почта или пароль." , "OK");

                LoadingIndicator.IsVisible = false;
                LoadingIndicator.IsRunning = false;
                LoginButton.IsEnabled = true;
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