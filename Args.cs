public class Argument
{
    public string Name { get; set; }
    public string Flag { get; set; }
    public string? AltFlag { get; set; }
    public bool UsesValue { get; set; }
    public int? Value { get; set; }

    public Argument(string n, string f)
    {
        Name = n;
        Flag = f;
        UsesValue = false;
    }

    public Argument(string n, string f, string af)
    {
        Name = n;
        Flag = f;
        AltFlag = af;
        UsesValue = false;
    }

    public Argument(string n, string f, string af, bool v)
    {
        Name = n;
        Flag = f;
        AltFlag = af;
        UsesValue = v;
    }

    public Argument(string n, string f, string af, bool uv, int v)
    {
        Name = n;
        Flag = f;
        AltFlag = af;
        UsesValue = v;
        Value = v;
    }

    public bool MatchesFlag(string arg)
    {
        if (arg == Flag || arg == AltFlag) return true;
        else return false;
    }

    // Outputs name and value
    public void Debug()
    {
        if (this.UsesValue)
        {
            Console.WriteLine(this.Name + " = " + this.Value);
        }
        else Console.WriteLine(this.Name);
    }
}
