namespace Puzzler.Pages;

public partial class LevelSelectionPage : ContentPage
{
    private int childCount;

    public LevelSelectionPage(Context context)
    {
        InitializeComponent();

        LevelsCollectionView.ItemsSource = context.Levels;
    }

    private async void OnLevelsCollectionViewChildAdded(object sender, ElementEventArgs e)
    {
        if (e.Element is VisualElement visualElement)
        {
            // Move the item off screen.
            visualElement.TranslationY = DeviceDisplay.Current.MainDisplayInfo.Height;

            await Task.Delay(250 + (childCount++ * 100));

            await visualElement.TranslateTo(0, -20, length: 650, Easing.SinInOut);
            await visualElement.TranslateTo(0, 10, length: 250, Easing.SinInOut);
            await visualElement.TranslateTo(0, 0, length: 150, Easing.SinInOut);
        }
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
