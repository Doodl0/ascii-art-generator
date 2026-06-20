using System;
using SkiaSharp;

class ASCIIArtGen
{
    // Argument defintions
    Argument height = new Argument("Height", "-h", "--height", true, 64);
    Argument width = new Argument("Width", "-w", "--width", true, 64);

    static void Main(string[] args)
    {
        var program = new ASCIIArtGen();
        program.ParseArgs(args);

        var img = new Image(args[0], program.width.Value, program.height.Value, 1.0f);

        for (int y = 0; y < img.Bitmap.Height; y++)
        {
            for (int x = 0; x < img.Bitmap.Width; x++)
            {
                Console.Write(img.CalculatePixelCharacter(x, y));

            }
            Console.WriteLine();
        }

    }

    public void ParseArgs(string[] args)
    {
        // Arg 0 is always filepath
        // Check validity of filepath
        if (File.Exists(args[0])) Console.WriteLine("Filepath valid");
        else Console.WriteLine("Filepath invalid");

        Argument[] argsList = [height, width];

        // Loop through inputted args
        for (int i = 1; i < args.Length; i++)
        {
            // Loop through valid args
            for (int j = 0; j < argsList.Length; j++)
            {
                // Check if input is in args list
                if (argsList[j].MatchesFlag(args[i]))
                {
                    // If arg has an input value needed, check next input and set value from it
                    if (argsList[j].UsesValue) argsList[j].Value = Int32.Parse(args[i + 1]); i++;
                    // Output debug info
                    argsList[j].Debug();
                }
            }
        }
    }
}
