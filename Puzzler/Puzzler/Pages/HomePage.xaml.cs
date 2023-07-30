namespace Puzzler.Pages;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
    }

    async void OnPlayButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(LevelSelectionPage));
    }
}
