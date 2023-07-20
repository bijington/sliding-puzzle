namespace Puzzler.Pages;

public partial class LevelSelectionPage : ContentPage
{
    public LevelSelectionPage(Context context)
    {
        InitializeComponent();

        LevelsCollectionView.ItemsSource = context.Levels;
    }
}
