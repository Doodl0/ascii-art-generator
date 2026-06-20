using SkiaSharp;

class Image
{
    public SKBitmap Bitmap { get; set; }
    public float Brightness { get; set; } = 1.0f;

    // Default palette from dark to light
    public char[] Palette = ['%', '#', '*', '+', '=', '-', ':', '.'];

    public Image(string filepath, int? w, int? h, float b)
    {
        Bitmap = SKBitmap.Decode(filepath);
        int width = w ?? Bitmap.Width;
        int height = h ?? Bitmap.Height;
        Bitmap = Bitmap.Resize(new SKSizeI(width, height), SKSamplingOptions.Default);
        Brightness = b;
    }

    public float CalculatePixelBrightness(SKColor colour)
    {
        return 0.2126f * colour.Red + 0.7152f * colour.Green + 0.0722f * colour.Blue * this.Brightness;
    }

    public char CalculatePixelCharacter(int x, int y)
    {
        var pixelColour = this.Bitmap.GetPixel(x, y);
        float pixelBrightness = this.CalculatePixelBrightness(pixelColour);
        int index = (int)Math.Round((pixelBrightness / 255) * Palette.Length);
        return this.Palette[index];
    }
}
