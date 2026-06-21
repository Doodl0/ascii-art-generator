using System;
using SkiaSharp;

class ASCIIArtGen
{
    // Argument defintions
    ArgumentInt height = new("Height", "-h", "--height", 64);
    ArgumentInt width = new("Width", "-w", "--width", 64);
    ArgumentFloat brightness = new("Brightness", "-b", "--brightness", 1.0f);
    ArgumentFloat contrast = new("Contrast", "-c", "--contrast", 1.0f);

    static void Main(string[] args)
    {
        var program = new ASCIIArtGen();
        program.ParseArgs(args);

        Image img = new(args[0], program.width.Value, program.height.Value, 0.5f);

        for (int y = 0; y < img.Bitmap.Height; y++)
        {
            for (int x = 0; x < img.Bitmap.Width; x++)
            {
                Console.Write(img.CalculatePixelCharacter(x, y));

            }
            Console.WriteLine();
        }

    }

    public void ParseArgs(string[] inputArgs)
    {
        // Arg 0 is always filepath
        // Check validity of filepath, close if invalid
        if (File.Exists(inputArgs[0])) Console.WriteLine("Filepath valid");
        else
        {
            Console.Error.WriteLine("Filepath invalid");
            Environment.Exit(1);
        }

        List<Argument> argsList = new();
        argsList.Add(height);
        argsList.Add(width);
        argsList.Add(brightness);
        argsList.Add(contrast);

        // Loop through inputted args, skipping first
        for (int input = 1; input < inputArgs.Length; input++)
        {
            // Loop through valid args
            foreach (Argument arg in argsList)
            {
                // Check if input is in args list
                if (arg.MatchesFlag(inputArgs[input]))
                {
                    // Check if arg requires value
                    if (arg.HasValue)
                    {
                        // Try catch to catch any out of boundary errors if a value has not been included
                        try
                        {
                            arg.Set(inputArgs[input + 1]);
                            arg.Debug();
                        }
                        catch
                        {
                            Console.Error.WriteLine("Could not find required value for " + arg.Name);
                            Environment.Exit(1);
                        }
                    }
                }
            }
        }
    }
}
