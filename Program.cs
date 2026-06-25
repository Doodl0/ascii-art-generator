using System;
using System.Linq.Expressions;
using SkiaSharp;

class ASCIIArtGen
{
    // Argument defintions
    readonly ArgumentNoValue help = new("Help", "--help", "List all options for the program. --help must be ran before a filepath in order to work");
    readonly ArgumentInt height = new("Height", "-h", "--height", 64, "Set the height of the output text [Integer]");
    readonly ArgumentInt width = new("Width", "-w", "--width", 64, "Set the width of the output text [Integer]");
    readonly ArgumentFloat brightness = new("Brightness", "-b", "--brightness", 1.0f, "Multiply the brightness of the input image [Decimal]");
    readonly ArgumentFloat contrast = new("Contrast", "-c", "--contrast", 1.0f, "Multiply the contrast of the input image [Decimal]");
    readonly ArgumentFloat saturation = new("Saturation", "-s", "--saturation", 1.0f, "Multiply the saturation of the input image [Decimal]");
    readonly ArgumentBool useAlpha = new("Use Alpha", "-a", "--use-alpha", true, "Use the alpha channel of the input image ['true' or 'false']");
    readonly ArgumentFilepath fileOutput = new("File Output", "-f", "--file", "", "Output to a text file [Filepath]");

    static void Main(string[] args)
    {
        var program = new ASCIIArtGen();
        program.ParseArgs(args);

        Image img = new(
            args[0], // filepath
            program.width.Value,
            program.height.Value,
            program.brightness.Value,
            program.contrast.Value,
            program.saturation.Value,
            program.useAlpha.Value
        );

        // Loop over all pixels and output a respective character
        // If user inputs a filepath, write to a file
        // Else, write to console
        if (program.fileOutput.Value != "")
        {
            try
            {
                using (StreamWriter outputFile = new(program.fileOutput.Value))
                {
                    for (int y = 0; y < img.Bitmap.Height; y++)
                    {
                        for (int x = 0; x < img.Bitmap.Width; x++)
                        {
                            outputFile.Write(img.CalculatePixelCharacter(x, y));
                        }
                        outputFile.WriteLine();
                    }
                }
                Console.WriteLine("Outputted to " + program.fileOutput.Value);
            }
            catch
            {
                Console.Error.WriteLine("Could not write to file " + program.fileOutput.Value);
                Environment.Exit(1);
            }
        }

        else
        {
            for (int y = 0; y < img.Bitmap.Height; y++)
            {
                for (int x = 0; x < img.Bitmap.Width; x++)
                {
                    Console.Write(img.CalculatePixelCharacter(x, y));
                }
                Console.WriteLine();
            }
        }

    }

    public void ParseArgs(string[] inputArgs)
    {
        Argument[] argsList = [
            help,
            height,
            width,
            brightness,
            contrast,
            saturation,
            useAlpha,
            fileOutput
        ];

        if (inputArgs.Length <= 0)
        {
            Console.WriteLine("No input arguments found. Run 'ascii-art-gen --help' for a list of all options");
            Environment.Exit(0);
        }

        // Arg 0 is always filepath or help
        // Check if arg 0 is help
        if (help.MatchesFlag(inputArgs[0]))
        {
            Help(argsList);
            Environment.Exit(0);
        }

        // Check validity of filepath, close if invalid
        if (File.Exists(inputArgs[0])) Console.WriteLine("Filepath valid");
        else
        {
            Console.Error.WriteLine("Filepath invalid");
            Environment.Exit(1);
        }


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

    public void Help(Argument[] argsList)
    {
        Console.WriteLine("Usage: ascii-art-gen [path-to-image-file] [arguments]\n");
        Console.WriteLine("path-to-image-file:\n    The path to the image file to be used to generate the ASCII text\n");

        Console.WriteLine("Arguments:");
        foreach (Argument arg in argsList)
        {
            if (arg.HasValue && arg.AltFlag != null)
            {
                Console.WriteLine($"{arg.Flag}|{arg.AltFlag} [Value]        {arg.HelpDescription}");
            }
            else if (arg.HasValue)
            {
                Console.WriteLine($"{arg.Flag} [Value]        {arg.HelpDescription}");
            }
            else
            {
                Console.WriteLine($"{arg.Flag}                   {arg.HelpDescription}");
            }
        }
    }
}
