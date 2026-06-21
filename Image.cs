using SkiaSharp;

class Image
{
    public SKBitmap Bitmap { get; set; }
    public bool UseAlpha { get; set; } = true;

    // Brightness multiplier for whole image
    public float Brightness { get; set; } = 1.0f;

    public float Contrast { get; set; } = 1.0f;
    public float Saturation { get; set; } = 1.0f;

    // Default palette from dark to light
    public char[] Palette = ['@', '%', '#', '*', '+', '=', '-', ':', '.', ' '];

    public Image(string filepath, int? w, int? h, float b, float c, float s, bool a)
    {
        Bitmap = SKBitmap.Decode(filepath);
        int width = w ?? Bitmap.Width;
        int height = h ?? Bitmap.Height;
        Bitmap = Bitmap.Resize(new SKSizeI(width, height), SKSamplingOptions.Default);
        Brightness = b;
        Contrast = c;
        Saturation = s;
        UseAlpha = a;
    }

    public float CalculatePixelLuminance(SKColor colour)
    {
        // Calculate pixel luminance then multiply by brightness and include alpha by subtracting.
        float luminance = (0.2126f * (float)colour.Red + 0.7152f * (float)colour.Green + 0.0722f * (float)colour.Blue);
        if (UseAlpha) return Math.Clamp(luminance + (255 - colour.Alpha), 0, 255);
        else return Math.Clamp(luminance, 0, 255);
    }

    public char CalculatePixelCharacter(int x, int y)
    {
        var pixelColour = Bitmap.GetPixel(x, y);
        pixelColour = ApplyBrightnessSaturation(pixelColour);
        pixelColour = ApplyContrast(pixelColour);
        float pixelBrightness = CalculatePixelLuminance(pixelColour);
        int index = ((int)Math.Round((pixelBrightness / 255) * Palette.Length));
        if (index > Palette.Length - 1) index = Palette.Length - 1;
        else if (index < 0) index = 0;
        return Palette[index];
    }

    public SKColor ApplyContrast(SKColor c)
    {
        // subtract midpoint, mult by contrast, add midpoint and then clamp to between 0 and 255 to prevent overflow. Then convert to uint
        var r = (uint)Math.Clamp((((((c.Red) - 128) * Contrast) + 128)), 0, 255);
        var g = (uint)Math.Clamp((((((c.Green) - 128) * Contrast) + 128)), 0, 255);
        var b = (uint)Math.Clamp((((((c.Blue) - 128) * Contrast) + 128)), 0, 255);
        return new SKColor((byte)r, (byte)g, (byte)b, c.Alpha);
    }

    public SKColor ApplyBrightnessSaturation(SKColor c)
    {
        float h, s, v;
        c.ToHsv(out h, out s, out v);
        s = Math.Clamp(s * Saturation, 0f, 100f);
        v = Math.Clamp(v * Brightness, 0f, 100f);
        return SKColor.FromHsv(h, s, v, c.Alpha);
    }
}
