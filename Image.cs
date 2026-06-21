using SkiaSharp;

class Image
{
    public SKBitmap Bitmap { get; set; }
    public bool UseAlpha { get; set; } = true;

    // Brightness multiplier for whole image
    public float Brightness { get; set; } = 1.0f;

    // Default palette from dark to light
    public char[] Palette = ['@', '%', '#', '*', '+', '=', '-', ':', '.', ' '];

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
        // Calculate pixel luminance then multiply by brightness and include alpha
        float luminance = (0.2126f * (float)colour.Red + 0.7152f * (float)colour.Green + 0.0722f * (float)colour.Blue);
        if (UseAlpha) return (luminance * Brightness) + (255 - colour.Alpha);
        else return (luminance * Brightness);
    }

    public char CalculatePixelCharacter(int x, int y)
    {
        var pixelColour = Bitmap.GetPixel(x, y);
        float pixelBrightness = CalculatePixelBrightness(pixelColour);
        int index = ((int)Math.Round((pixelBrightness / 255) * Palette.Length));
        if (index > Palette.Length - 1) index = Palette.Length - 1;
        else if (index < 0) index = 0;
        return Palette[index];
    }
}
