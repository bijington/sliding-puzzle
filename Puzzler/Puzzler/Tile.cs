namespace Puzzler;

/// <summary>
/// Represents a section of an image to slide.
/// </summary>
public class Tile
{
    private readonly Microsoft.Maui.Graphics.IImage image;
    private readonly SizeF imageSize;

    public Tile(Microsoft.Maui.Graphics.IImage image, PointF point, SizeF size, SizeF imageSize)
    {
        this.image = image;
        Size = size;
        this.imageSize = imageSize;
        CurrentPosition = DestinationPosition = point;
    }

    public RectF Bounds => new RectF(CurrentPosition, Size);

    public PointF CurrentPosition { get; set; }

    public PointF DestinationPosition { get; }

    public SizeF Size { get; }

    public float Offset { get; set; }

    public void Draw(ICanvas canvas)
    {
        if (image is null)
        {
            return;
        }

        var x = this.CurrentPosition.X;
        var y = this.CurrentPosition.Y;

        // Calculate any offset based on the TileSpacing defined on the SlidingTileGrid.
        float xOffset = x * Offset;
        float yOffset = y * Offset;

        // Clip the canvas so that we only render the part of the image that this tile represents.
        canvas.ClipRectangle(
            x * Size.Width + xOffset,
            y * Size.Height + yOffset,
            Size.Width,
            Size.Height);

        float imageOriginX = (x * Size.Width) - (this.DestinationPosition.X * Size.Width) + xOffset;
        float imageOriginY = (y * Size.Height) - (this.DestinationPosition.Y * Size.Height) + yOffset;

        canvas.DrawImage(
            image,
            imageOriginX,
            imageOriginY,
            imageSize.Width,
            imageSize.Height);
    }
}
