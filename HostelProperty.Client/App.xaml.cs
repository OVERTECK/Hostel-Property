using HostelProperty.Client.Pages;

namespace HostelProperty.Client
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new AuthorizationPage());
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = base.CreateWindow(activationState);

            var currentDisplay = DeviceDisplay.Current.MainDisplayInfo;

            window.Width = currentDisplay.Width;
            window.Height = currentDisplay.Height;

            return window;
        }
    }
}