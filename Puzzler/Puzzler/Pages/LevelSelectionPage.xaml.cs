namespace Puzzler.Pages;

public partial class LevelSelectionPage : ContentPage
{
    public LevelSelectionPage(Context context)
    {
        InitializeComponent();

        LevelsCollectionView.ItemsSource = context.Levels;
    }

    private async void OnLevelsCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var level = e.CurrentSelection.FirstOrDefault();

        if (level is null)
        {
            return;
        }

        await Shell.Current.GoToAsync(nameof(LevelPage), new Dictionary<string, object>
        {
            [nameof(LevelPage.Level)] = level
        });

        // Clear the selection for when we navigate back nothing is selected.
        LevelsCollectionView.SelectedItem = null;
    }
}
