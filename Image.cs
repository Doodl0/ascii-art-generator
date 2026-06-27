using SkiaSharp;

class Image
{
    public SKBitmap Bitmap { get; set; }
    public bool UseAlpha { get; set; } = true;
    public bool UseEdgeDetect { get; set; } = true;

    // Brightness multiplier for whole image
    public float Brightness { get; set; } = 1.0f;

    public float Contrast { get; set; } = 1.0f;
    public float Saturation { get; set; } = 1.0f;

    // Default palette from dark to light
    public char[] Palette = ['@', '%', '#', '*', '+', '=', '-', ':', '.', ' '];

    public Image(string filepath, int? w, int? h, float b, float c, float s, bool a, bool e, string p)
    {
        // Catch error in case of file not being an image
        try
        {
            Bitmap = SKBitmap.Decode(filepath);
            // If w and h are set, use them for the image size, otherwise use original size
            int width = w ?? Bitmap.Width;
            int height = h ?? Bitmap.Height;
            // Resize image and set parameters
            Bitmap = Bitmap.Resize(new SKSizeI(width, height), new SKSamplingOptions(SKFilterMode.Nearest, SKMipmapMode.None));
        }
        catch
        {
            Console.Error.WriteLine("Could not decode valid image from " + filepath);
            Environment.Exit(1);
        }
        Brightness = b;
        Contrast = c;
        Saturation = s;
        UseAlpha = a;
        UseEdgeDetect = e;
        Palette = p.ToCharArray();
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
        // Sample pixel colour
        var pixelColour = Bitmap.GetPixel(x, y);

        // Apply sobel filter for edge detection
        if (UseEdgeDetect) pixelColour = Sobel(x, y);

        // Apply saturation and brightness modifier to pixel
        pixelColour = ApplyBrightnessSaturation(pixelColour);

        // Apply contrast modifier to pixel
        pixelColour = ApplyContrast(pixelColour);

        // Calculate pixel luminance
        float pixelBrightness = CalculatePixelLuminance(pixelColour);

        // Normalise luminance, then multiply by amount of characters in palette and round
        int index = (int)Math.Round((pixelBrightness / 255) * Palette.Length);

        // Catch any out of boundary indexes
        if (index > Palette.Length - 1) index = Palette.Length - 1;
        else if (index < 0) index = 0;
        return Palette[index];
    }

    public SKColor ApplyContrast(SKColor c)
    {
        // Subtract midpoint, mult by contrast, add midpoint and then clamp to between 0 and 255 to prevent overflow. Then convert to uint
        var r = (uint)Math.Clamp((((((c.Red) - 128) * Contrast) + 128)), 0, 255);
        var g = (uint)Math.Clamp((((((c.Green) - 128) * Contrast) + 128)), 0, 255);
        var b = (uint)Math.Clamp((((((c.Blue) - 128) * Contrast) + 128)), 0, 255);
        return new SKColor((byte)r, (byte)g, (byte)b, c.Alpha);
    }

    public SKColor ApplyBrightnessSaturation(SKColor c)
    {
        // Convert to HSV, mult S and V by saturation and brightness, convert back to RGB
        float h, s, v;
        c.ToHsv(out h, out s, out v);
        s = Math.Clamp(s * Saturation, 0f, 100f);
        v = Math.Clamp(v * Brightness, 0f, 100f);
        return SKColor.FromHsv(h, s, v, c.Alpha);
    }

    public SKColor Sobel(int x, int y)
    {
        // Sample 3x3 grid around pixel, from top left to bottom right
        // Clamp to prevent sampling out of bounds
        SKColor[] pixels = {
            Bitmap.GetPixel(Math.Clamp(x-1, 0, Bitmap.Width-1), Math.Clamp(y-1, 0, Bitmap.Height-1)), Bitmap.GetPixel(Math.Clamp(x, 0, Bitmap.Width-1), Math.Clamp(y-1, 0, Bitmap.Height-1)), Bitmap.GetPixel(Math.Clamp(x+1, 0, Bitmap.Width-1), Math.Clamp(y-1, 0, Bitmap.Height-1)),
            Bitmap.GetPixel(Math.Clamp(x-1, 0, Bitmap.Width-1), Math.Clamp(y, 0, Bitmap.Height-1)), Bitmap.GetPixel(Math.Clamp(x, 0, Bitmap.Width-1), Math.Clamp(y, 0, Bitmap.Height-1)), Bitmap.GetPixel(Math.Clamp(x+1, 0, Bitmap.Width-1), Math.Clamp(y, 0, Bitmap.Height-1)),
            Bitmap.GetPixel(Math.Clamp(x-1, 0, Bitmap.Width-1), Math.Clamp(y+1, 0, Bitmap.Height-1)), Bitmap.GetPixel(Math.Clamp(x, 0, Bitmap.Width-1), Math.Clamp(y+1, 0, Bitmap.Height-1)), Bitmap.GetPixel(Math.Clamp(x+1, 0, Bitmap.Width-1), Math.Clamp(y+1, 0, Bitmap.Height-1)),
        };

        float[] xKernel = {
            -1,0,1,
            -2,0,2,
            -1,0,1
        };

        float[] yKernel = {
            -1,-2,-1,
            0,0,0,
            1,2,1
        };

        float gx = 0;
        float gy = 0;

        for (int i = 0; i < 9; i++)
        {
            gx += (pixels[i].Red + pixels[i].Green + pixels[i].Blue) / 3 * xKernel[i];
        }
        for (int i = 0; i < 9; i++)
        {
            gy += (pixels[i].Red + pixels[i].Green + pixels[i].Blue) / 3 * yKernel[i];
        }
        var g = Math.Sqrt(Math.Pow(gx, 2) + Math.Pow(gy, 2));

        var col = (uint)(Math.Clamp(255 - g, 0, 255));
        return new SKColor((byte)col, (byte)col, (byte)col);
    }
}
