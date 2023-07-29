using Microsoft.Maui.Graphics.Platform;
using System.Reflection;

namespace Puzzler;

public class SlidingTileGrid : GraphicsView, IDrawable
{
    private Microsoft.Maui.Graphics.IImage image;
    private float imageHeight;
    private float imageWidth;
    private int horizontalTileCount;
    private int verticalTileCount;
    private float tileHeight;
    private float tileWidth;

    private readonly IList<Tile> tiles = new List<Tile>();
    private EmptyTile emptyTile;

    public event EventHandler<EventArgs> Completed;

    public float TileSpacing { get; set; } = 10;

    public static readonly BindableProperty LevelProperty =
        BindableProperty.Create(nameof(Level), typeof(Level), typeof(SlidingTileGrid), propertyChanged: OnLevelPropertyChanged);

    public Level Level
    {
        get => (Level)GetValue(LevelProperty);
        set => SetValue(LevelProperty, value);
    }

    static void OnLevelPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var slidingTileGrid = (SlidingTileGrid)bindable;

        if (slidingTileGrid.Height != -1 &&
            newValue is Level level)
        {
            slidingTileGrid.Load(level.ImageName, GetLevelGridSize(level.Difficulty));
        }
    }

    public SlidingTileGrid()
    {
        Drawable = this;

        this.EndInteraction += SlidingTileGrid_EndInteraction;
    }

    private void SlidingTileGrid_EndInteraction(object sender, TouchEventArgs e)
    {
        if (this.IsEnabled == false)
        {
            return;
        }

        // Detect if the tile can be moved
        var touchUp = e.Touches.First();

        var position = new PointF(MathF.Floor(touchUp.X / tileWidth), MathF.Floor(touchUp.Y / tileHeight));

        var matchingTile = this.tiles.FirstOrDefault(t => t.CurrentPosition.X == position.X && t.CurrentPosition.Y == position.Y);

        if (matchingTile is null)
        {
            return;
        }

        // Check if there is any empty tile location nextdoor.
        MoveTile(matchingTile, true);
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        if (this.Level is not null)
        {
            this.Load(this.Level.ImageName, GetLevelGridSize(this.Level.Difficulty));
        }
    }

    public static Size GetLevelGridSize(LevelDifficulty levelDifficulty) => levelDifficulty switch
    {
        LevelDifficulty.Easy => new Size(2, 2),
        LevelDifficulty.Medium => new Size(3, 3),
        LevelDifficulty.Hard => new Size(4, 4),
        _ => throw new NotImplementedException()
    };

    public void Load(string imageName, Size gridSize)
    {
        this.tiles.Clear();

        image = LoadImage(imageName);

        // Divide the dimensions up evenly taking into account the number of gaps between each tile
        imageWidth = (float)this.WidthRequest - (float)((gridSize.Width - 1) * TileSpacing);
        imageHeight = (float)this.HeightRequest - (float)((gridSize.Height - 1) * TileSpacing);

        horizontalTileCount = (int)gridSize.Width;
        verticalTileCount = (int)gridSize.Height;

        tileHeight = imageHeight / verticalTileCount;
        tileWidth = imageWidth / horizontalTileCount;

        for (int x = 0; x < horizontalTileCount; x++)
        {
            for (int y = 0; y < verticalTileCount; y++)
            {
                // If the control is enabled then we allow for interaction.
                if (IsEnabled && x == horizontalTileCount - 1 && y == verticalTileCount - 1)
                {
                    this.emptyTile = new EmptyTile(new PointF(x, y), new SizeF(tileWidth, tileHeight), new SizeF(imageWidth, imageHeight));
                    this.tiles.Add(this.emptyTile);
                    break;
                }

                this.tiles.Add(new Tile(image, new PointF(x, y), new SizeF(tileWidth, tileHeight), new SizeF(imageWidth, imageHeight)));
            }
        }

        foreach (var tile in this.tiles)
        {
            tile.Offset = TileSpacing;
        }

        if (this.IsEnabled)
        {
            this.ShuffleTiles(200);
        }
    }

    private void ShuffleTiles(int numberOfMovements)
    {
        var random = new Random();

        for (int i = 0; i < numberOfMovements; i++)
        {
            var possibleMovements = this.tiles.Where(t => this.emptyTile.CurrentPosition.Distance(t.CurrentPosition) == 1).ToList();

            var randomIndex = random.Next(possibleMovements.Count);

            MoveTile(possibleMovements[randomIndex], false);
        }

        this.Invalidate();
    }

    private void MoveTile(Tile matchingTile, bool checkForCompletion)
    {
        var distance = this.emptyTile.CurrentPosition.Distance(matchingTile.CurrentPosition);

        if (distance == 1)
        {
            var oldPosition = matchingTile.CurrentPosition;
            var newPosition = this.emptyTile.CurrentPosition;
            this.emptyTile.CurrentPosition = oldPosition;

            matchingTile.CurrentPosition = newPosition;

            this.Invalidate();

            // Check to see if all tiles are in their correct positions.
            if (checkForCompletion &&
                this.tiles.All(t => t.CurrentPosition == t.DestinationPosition))
            {
                this.Completed?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        foreach (var tile in this.tiles)
        {
            // Save and then reset the state of the canvas ahead of allowing a tile to draw.
            canvas.SaveState();
            canvas.ResetState();

            tile.Draw(canvas);

            // Restore back to the original saved state to avoid any left over state from the last tiles drawing.
            canvas.RestoreState();
        }
    }

    protected Microsoft.Maui.Graphics.IImage LoadImage(string imageName)
    {
        var assembly = GetType().GetTypeInfo().Assembly;

        using (var stream = assembly.GetManifestResourceStream(imageName))
        {
#if WINDOWS
            return new W2DImageLoadingService().FromStream(stream);
#else
            return PlatformImage.FromStream(stream);
#endif
        }
    }
}
