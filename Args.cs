
// Base class for all arguments
public abstract class Argument
{
    // Variables shared by all arguments
    public string Name { get; set; }
    public string Flag { get; set; }
    public string? AltFlag { get; set; }
    public abstract bool HasValue { get; }

    // Constructors
    public Argument(string n, string f)
    {
        Name = n;
        Flag = f;
    }

    public Argument(string n, string f, string af)
    {
        Name = n;
        Flag = f;
        AltFlag = af;
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
    public ArgumentValue(string n, string f, string af, T v) : base(n, f, af)
    {
        Name = n;
        Flag = f;
        AltFlag = af;
        Value = v;
    }
}

public class ArgumentInt : ArgumentValue<int>
{
    public ArgumentInt(string n, string f, string af, int v) : base(n, f, af, v)
    {
        Name = n;
        Flag = f;
        AltFlag = af;
        Value = v;
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

    // Outputs name and value
    override public void Debug()
    {
        Console.WriteLine(Name + " = " + Value);
    }
}

public class ArgumentFloat : ArgumentValue<float>
{
    public ArgumentFloat(string n, string f, string af, float v) : base(n, f, af, v)
    {
        Name = n;
        Flag = f;
        AltFlag = af;
        Value = v;
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

    // Outputs name and value
    override public void Debug()
    {
        Console.WriteLine(Name + " = " + Value);
    }
}
