// Base class for all arguments
public abstract class Argument
{
    // Variables shared by all arguments
    public string Name { get; set; }
    public string Flag { get; set; }
    public string? AltFlag { get; set; }
    public abstract bool HasValue { get; }
    public string HelpDescription { get; set; }

    // Constructors
    public Argument(string n, string f, string h)
    {
        Name = n;
        Flag = f;
        HelpDescription = h;
    }

    public Argument(string n, string f, string af, string h)
    {
        Name = n;
        Flag = f;
        AltFlag = af;
        HelpDescription = h;
    }

    // Check if inputted flag matches either flag
    public bool MatchesFlag(string arg)
    {
        if (arg == Flag || arg == AltFlag) return true;
        else return false;
    }

    // Abstract methods for child classes
    public abstract void Set(string s);
    public abstract void Debug();
}

// Abstract class for all arguments with an attached value
public abstract class ArgumentValue<T> : Argument
{
    public T Value { get; protected set; }
    public override bool HasValue { get; } = true;

    // Constructor for an argument with a default value
    public ArgumentValue(string n, string f, string af, T v, string h) : base(n, f, af, h)
    {
        Name = n;
        Flag = f;
        AltFlag = af;
        Value = v;
        HelpDescription = h;
    }

    // Outputs name and value
    override public void Debug()
    {
        Console.WriteLine(Name + " = " + Value);
    }
}

public class ArgumentNoValue : Argument
{
    public override bool HasValue { get; } = false;
    // Constructors
    public ArgumentNoValue(string n, string f, string h) : base(n, f, h)
    {
        Name = n;
        Flag = f;
        HelpDescription = h;
    }

    public ArgumentNoValue(string n, string f, string af, string h) : base(n, f, af, h)
    {
        Name = n;
        Flag = f;
        AltFlag = af;
        HelpDescription = h;
    }

    // Set is blank as no value to set
    override public void Set(string s) { }

    // Outputs name
    override public void Debug()
    {
        Console.WriteLine(Name);
    }
}

public class ArgumentInt : ArgumentValue<int>
{
    public ArgumentInt(string n, string f, string af, int v, string h) : base(n, f, af, v, h)
    {
        Name = n;
        Flag = f;
        AltFlag = af;
        Value = v;
        HelpDescription = h;
    }

    override public void Set(string s)
    {
        try
        {
            Value = Int32.Parse(s);
        }
        catch
        {
            Console.Error.WriteLine("Could not get valid value from " + s + " for argument " + Name);
            Environment.Exit(1);
        }
    }
}

public class ArgumentFloat : ArgumentValue<float>
{
    public ArgumentFloat(string n, string f, string af, float v, string h) : base(n, f, af, v, h)
    {
        Name = n;
        Flag = f;
        AltFlag = af;
        Value = v;
        HelpDescription = h;
    }

    override public void Set(string s)
    {
        try
        {
            Value = float.Parse(s);
        }
        catch
        {
            Console.Error.WriteLine("Could not get valid value from " + s + " for argument " + Name);
            Environment.Exit(1);
        }
    }


}

public class ArgumentBool : ArgumentValue<bool>
{
    public ArgumentBool(string n, string f, string af, bool v, string h) : base(n, f, af, v, h)
    {
        Name = n;
        Flag = f;
        AltFlag = af;
        Value = v;
        HelpDescription = h;
    }

    override public void Set(string s)
    {
        try
        {
            Value = bool.Parse(s);
        }
        catch
        {
            Console.Error.WriteLine("Could not get valid value from " + s + " for argument " + Name);
            Environment.Exit(1);
        }
    }

    // Outputs name and value
    override public void Debug()
    {
        Console.WriteLine(Name + " = " + Value);
    }
}

public class ArgumentString : ArgumentValue<string>
{
    public ArgumentString(string n, string f, string af, string v, string h) : base(n, f, af, v, h)
    {
        Name = n;
        Flag = f;
        AltFlag = af;
        Value = v;
        HelpDescription = h;
    }

    override public void Set(string s)
    {
        try
        {
            // If input string is a filepath, read from file instead
            if (File.Exists(s))
            {
                try
                {
                    using StreamReader reader = new(s);
                    Value = reader.ReadToEnd();
                }
                catch
                {
                    Console.Error.WriteLine("Could not get valid filepath from " + s + " for argument " + Name);
                    Environment.Exit(1);
                }
            }
            else Value = s;
        }
        catch
        {
            Console.Error.WriteLine("Could not get valid value from " + s + " for argument " + Name);
            Environment.Exit(1);
        }
    }

    // Outputs name and value
    override public void Debug()
    {
        Console.WriteLine(Name + " = " + Value);
    }
}

public class ArgumentFilepath : ArgumentString
{
    public ArgumentFilepath(string n, string f, string af, string v, string h) : base(n, f, af, v, h)
    {
        Name = n;
        Flag = f;
        AltFlag = af;
        Value = v;
        HelpDescription = h;
    }
    override public void Set(string s)
    {
        try
        {
            Path.GetFullPath(s);
            Value = s;
        }
        catch
        {
            Console.Error.WriteLine("Could not get valid filepath from " + s + " for argument " + Name);
            Environment.Exit(1);
        }
    }
}
