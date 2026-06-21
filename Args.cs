public class Argument
{
    public string Name { get; set; }
    public string Flag { get; set; }
    public string? AltFlag { get; set; }
    public bool UsesValue { get; set; }
    public int? Value { get; set; }
    public Func<int> Run { get; set; }

    public Argument(string n, string f)
    {
        Name = n;
        Flag = f;
        UsesValue = false;
        Run = () => { Debug(); return 1; };
    }

    public Argument(string n, string f, string af)
    {
        Name = n;
        Flag = f;
        AltFlag = af;
        UsesValue = false;
        Run = () => { Debug(); return 1; };
    }

    public Argument(string n, string f, string af, bool uv)
    {
        Name = n;
        Flag = f;
        AltFlag = af;
        UsesValue = uv;
        Run = () => { Debug(); return 1; };
    }

    public Argument(string n, string f, string af, bool uv, int v)
    {
        Name = n;
        Flag = f;
        AltFlag = af;
        UsesValue = uv;
        Value = v;
        Run = () => { Debug(); return 1; };
    }

    public bool MatchesFlag(string arg)
    {
        if (arg == Flag || arg == AltFlag) return true;
        else return false;
    }

    // Outputs name and value
    public void Debug()
    {
        if (UsesValue)
        {
            Console.WriteLine(Name + " = " + Value);
        }
        else Console.WriteLine(Name);
    }
}
