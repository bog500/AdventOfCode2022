
using System.Diagnostics;
using System.Runtime.Serialization;

Console.WriteLine("Advent of Code 2022 - Day 16");
Console.WriteLine("\n-------------------");

string cluefile = "clue.txt";


Dictionary<string, Valve> valves = Setup();


State initial = new()
{
    Action = "Initial",
    History = "Initial",
    Location = "AA",
    PreasureRate = 0,
    TotalPreasure = 0,
    Minutes = 0,
    Value = 0,
    Open = new()
};

State current = initial;

using (Timer t = new())
{
    int deptSearch = 21;
    Console.WriteLine($"Searching with dept {deptSearch}");
    while (current.Minutes < 30)
    {
        current = Maximize(current, deptSearch);
        Console.WriteLine($"{current.Minutes}  {current.Action}  Total Preasure:{current.TotalPreasure}     {t.CurrentLap()}");
    }
}




int h = 5;

State Maximize(State state, int depth)
{
    //Console.WriteLine($"Maximize {depth}  {state.Action}");

    // stop evaluating
    if (depth == 0 || state.Minutes == 30)
    {
        state.Value += (30 - state.Minutes) * state.PreasureRate + state.TotalPreasure;
        //Console.WriteLine($"Analyzed {state.History} with value of {state.Value}");
        return state;
    }
        

    int maxValue = -1;
    State nextState = null;

    List<State> possibilities = GetPossibilities(state);
    foreach(var p in possibilities)
    {
        State nextStateCandidate = Maximize(p, depth - 1);
        if(nextStateCandidate != null && nextStateCandidate.Value > maxValue)
        {
            maxValue = nextStateCandidate.Value;
            nextState = p;
            nextState.Value = maxValue;
        }
    }
    return nextState;
}




List<State> GetPossibilities(State state)
{
    List<State> possibilities = new();

    Valve currentValve = valves[state.Location];

    if (state.Open.Contains(state.Location) == false && currentValve.Presure > 0)
    {
        // open current valve
        State nextState = new()
        {
            Action = $"Open {state.Location}",
            //History = state.History + " > " + $"Open {state.Location}",
            Location = state.Location,
            Minutes = state.Minutes + 1,
            PreasureRate = state.PreasureRate + currentValve.Presure,
            TotalPreasure = state.TotalPreasure + state.PreasureRate,
            Open = new(state.Open),
            Value = state.Value,
            LastLocation = "Open"
        };
        nextState.Open.Add(state.Location);
        possibilities.Add(nextState);
    }

    // move to another valve
    foreach (string voisin in currentValve.Voisins)
    {
        if(state.LastLocation == voisin)
        {

        }
        else
        {
            Valve next = valves[voisin];
            State nextState = new()
            {
                Action = $"Move to {voisin}",
                //History = state.History + " > " + $"Move to {voisin}",
                Location = voisin,
                Minutes = state.Minutes + 1,
                PreasureRate = state.PreasureRate,
                TotalPreasure = state.TotalPreasure + state.PreasureRate,
                Open = new(state.Open),
                Value = state.Value,
                LastLocation = state.Location
            };
            possibilities.Add(nextState);
        }

  

    }
    return possibilities;
}


Dictionary<string, Valve> Setup()
{
    Dictionary<string, Valve> list = new();

    var lines = File.ReadAllLines(cluefile);

    foreach(var line in lines)
    {
        string code = line.Substring(6, 2);

        int pFrom = line.IndexOf("=")+1;
        int pTo = line.IndexOf(";");

        int presure = int.Parse(line.Substring(pFrom, pTo - pFrom));

        Valve v = new(code, presure);

        int pValve = line.Replace("valves", "valve").IndexOf("to valve ") + "to valve ".Length;

        string s = line.Substring(pValve, line.Length - pValve);

        foreach(var voisin in s.Replace(" ","").Split(','))
        {
            v.Voisins.Add(voisin);
        }

        list.Add(code, v);
    }

    return list;
}

class Valve
{
    public int Presure { get; private set; }

    public string Code { get; private set; }

    public List<string> Voisins = new();

    public Valve(string code, int presure)
    {
        this.Code = code;
        this.Presure = presure;
    }

    private int _hashcode;
    public override int GetHashCode()
    {
        if (_hashcode == 0)
            _hashcode = this.Code.GetHashCode();
        return _hashcode;
    }

}

class State
{
    public string Location;
    public int TotalPreasure;
    public int Value;
    public int PreasureRate;
    public int Minutes;
    public HashSet<string> Open = new();
    public string Action;
    public string History;
    public string LastLocation;
}

class Timer : IDisposable
{
    Stopwatch sw = new();
    double totalSeconds = 0;

    public Timer()
    {
        sw.Start();
    }

    public string CurrentTotal()
    {
        return "SubTotal " + Formatter(sw.Elapsed);
    }

    public string CurrentLap()
    {
        string result = Formatter(sw.Elapsed - TimeSpan.FromSeconds(totalSeconds));
        totalSeconds = sw.Elapsed.TotalSeconds;
        return result;
    }

    private string Formatter(TimeSpan ts)
    {
        return ts.TotalSeconds switch
        {
            < 1 => $"Time taken: {ts.TotalMilliseconds} ms",
            < 120 => $"Time taken: {ts.TotalSeconds} sec",
            < 7200 => $"Time taken: {ts.TotalMinutes} min",
            _ => $"Time taken: {ts.TotalHours} h"
        };
    }

    public void Dispose()
    {
        sw.Stop();
        Console.WriteLine($"Total {Formatter(sw.Elapsed)}");
    }
}