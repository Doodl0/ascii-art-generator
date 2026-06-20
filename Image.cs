using SkiaSharp;

class Image
{
    public SKBitmap Bitmap { get; set; }
    public float Brightness { get; set; } = 1.0f;

    public Image(string filepath, int w, int h, float b)
    {
        Bitmap = SKBitmap.Decode(filepath);
        Bitmap.Resize(new SKSizeI(w, h), SKSamplingOptions.Default);
        Brightness = b;
    }

    public int PixelBrightness(SKColor colour)
    {
        return (int)Math.Round((0.2126f * colour.Red + 0.7152f * colour.Green + 0.0722f * colour.Blue));
    }
}
