namespace Puzzler.Pages;

[QueryProperty(nameof(Level), nameof(Level))]
public partial class LevelPage : ContentPage
{
    public Level Level { get; set; }

    public LevelPage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        TitleLabel.Text = Level.Name;
        SubtitleLabel.Text = Level.Difficulty.ToString();

        SlidingTiles.Level = Level;
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        if (width > 0)
        {
            var smallest = Math.Min(width, height);

            SlidingTiles.WidthRequest = smallest;
            SlidingTiles.HeightRequest = smallest;
        }
    }

    private async void OnSlidingTilesCompleted(object sender, EventArgs e)
    {
        await this.DisplayAlert("Congratulations", "Party time", "Whoop whoop");

        await Shell.Current.GoToAsync("..");
    }
}