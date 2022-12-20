
using System.Diagnostics;
using System.Runtime.Serialization;

Console.WriteLine("Advent of Code 2022 - Day 16");
Console.WriteLine("\n-------------------");

string cluefile = "clue.txt";


Dictionary<string, Valve> valves = Setup();


State current = new()
{
    ElfAction = "Initial",
    ElfHistory = "Initial",
    ElfLocation = "AA",

    EleAction = "Initial",
    EleHistory = "Initial",
    EleLocation = "AA",

    PreasureRate = 0,
    TotalPreasure = 0,
    Minutes = 0,
    Value = 0,
    Open = new(),
};


using (Timer t = new())
{
    int depthSearch = 18;
    Console.WriteLine($"Searching with depth {depthSearch}");
    while (current.Minutes < 26)
    {
        current = Maximize(current, depthSearch);
        Console.WriteLine($"{current.Minutes}  {current.ElfAction} {current.EleAction}  Total Preasure:{current.TotalPreasure}     {t.CurrentLap()}");
    }
}




nint h = 5;


Stack<string> CopyStack(Stack<string> s)
{
    List<string> l = s.ToList();
    l.Reverse();
    return new(l);
}

//void Run(int minutes, int depthSearch, bool teachEle)
//{
//    using (Timer t = new())
//    {
//        Console.WriteLine($"Searching with depth {depthSearch}");
//        while (current.Minutes < minutes)
//        {
//            current = Maximize(current, depthSearch, minutes, teachEle);
//            Console.WriteLine($"{current.Minutes}  {current.ElfAction} {current.EleAction}  Total Preasure:{current.TotalPreasure}     {t.CurrentLap()}");
//        }
//    }
//}


State Maximize(State state, int depth)
{
    //Console.WriteLine($"Maximize {depth}  {state.Action}");

    // stop evaluating
    if (depth == 0 || state.Minutes == 26)
    {
        state.Value += (26 - state.Minutes) * state.PreasureRate + state.TotalPreasure;
        //Console.WriteLine($"Analyzed {state.History} with value of {state.Value}");
        return state;
    }

    if (depth < 5 && state.Open.Count == 0)
    {
        // did't open any yet, abondon
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

    Valve elfValve = valves[state.ElfLocation];
    Valve eleValve = valves[state.EleLocation];


    // ################################################
    #region both open valves 
    if (state.Open.Contains(state.ElfLocation) == false 
        && elfValve.Presure > 0
        && state.Open.Contains(state.EleLocation) == false
        && eleValve.Presure > 0
        && state.ElfLocation != state.EleLocation
        )
    {

        State nextState = new()
        {
            ElfAction = $"Elf Open {state.ElfLocation}",
            //History = state.History + " > " + $"Open {state.Location}",
            ElfLocation = state.ElfLocation,
            ElfLastLocation = "Open",

            EleAction = $"Ele Open {state.EleLocation}",
            //History = state.History + " > " + $"Open {state.Location}",
            EleLocation = state.EleLocation,
            EleLastLocation = "Open",

            Minutes = state.Minutes + 1,
            PreasureRate = state.PreasureRate + elfValve.Presure + eleValve.Presure,
            TotalPreasure = state.TotalPreasure + state.PreasureRate,
            Open = new(state.Open),
            Value = state.Value,

        };

        nextState.Open.Add(state.ElfLocation);
        nextState.Open.Add(state.EleLocation);
        possibilities.Add(nextState);
    }

    #endregion


    // ################################################
    #region Elf Open | Ele move 
    if (state.Open.Contains(state.ElfLocation) == false 
        && elfValve.Presure > 0
        )
    {
        foreach (string voisinEle in eleValve.Voisins)
        {

            if (state.EleLastLocation == voisinEle)
            {
                // useless roundtrip
                continue;
            }

            Valve nextEle = valves[voisinEle];

            State nextState = new()
            {
                ElfAction = $"Elf Open {state.ElfLocation}",
                //History = state.History + " > " + $"Move to {voisin}",
                ElfLocation = state.ElfLocation,
                ElfLastLocation = "Open",

                EleAction = $"Ele Move to {voisinEle}",
                //History = state.History + " > " + $"Move to {voisin}",
                EleLocation = voisinEle,
                EleLastLocation = state.EleLocation,

                Minutes = state.Minutes + 1,
                PreasureRate = state.PreasureRate + elfValve.Presure,
                TotalPreasure = state.TotalPreasure + state.PreasureRate,
                Open = new(state.Open),
                Value = state.Value,

            };

            nextState.Open.Add(state.ElfLocation);
            possibilities.Add(nextState);
        }
    }

    #endregion

    // ################################################
    #region Elf move | Ele open 
    if (state.Open.Contains(state.EleLocation) == false 
        && eleValve.Presure > 0)
    {
        foreach (string voisin in elfValve.Voisins)
        {

            if (state.ElfLastLocation == voisin)
            {
                // useless roundtrip
                continue;
            }

            Valve nextElf = valves[voisin];

            State nextState = new()
            {
                ElfAction = $"Elf Move to {voisin}",
                //History = state.History + " > " + $"Move to {voisin}",
                ElfLocation = voisin,
                ElfLastLocation = state.ElfLocation,

                EleAction = $"Ele Open {state.ElfLocation}",
                //History = state.History + " > " + $"Move to {voisin}",
                EleLocation = state.EleLocation,
                EleLastLocation = "Open",

                Minutes = state.Minutes + 1,
                PreasureRate = state.PreasureRate + eleValve.Presure,
                TotalPreasure = state.TotalPreasure + state.PreasureRate,
                Open = new(state.Open),
                Value = state.Value,

            };

            nextState.Open.Add(state.EleLocation);
            possibilities.Add(nextState);
        }
    }

    #endregion

    // ################################################



    #region both move to another valve

        foreach (string voisin in elfValve.Voisins)
        {
            if (state.ElfLastLocation == voisin)
            {
                // useless roundtrip
                continue;
            }

            foreach (string voisinEle in eleValve.Voisins)
            {

                if (state.EleLastLocation == voisinEle)
                {
                    // useless roundtrip
                    continue;
                }

                if (state.ElfLocation == voisinEle && state.EleLocation == voisin)
                {
                    //they switch place
                    continue;
                }

                Valve nextElf = valves[voisin];
                Valve nextEle = valves[voisinEle];

                State nextState = new()
                {
                    ElfAction = $"Elf Move to {voisin}",
                    //History = state.History + " > " + $"Move to {voisin}",
                    ElfLocation = voisin,
                    ElfLastLocation = state.ElfLocation,

                    EleAction = $"Ele Move to {voisinEle}",
                    //History = state.History + " > " + $"Move to {voisin}",
                    EleLocation = voisinEle,
                    EleLastLocation = state.EleLocation,

                    Minutes = state.Minutes + 1,
                    PreasureRate = state.PreasureRate,
                    TotalPreasure = state.TotalPreasure + state.PreasureRate,
                    Open = new(state.Open),
                    Value = state.Value,

                };
                possibilities.Add(nextState);
            }

        }


    #endregion
    // ################################################

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
    
    public int TotalPreasure;
    public int Value;
    public int PreasureRate;
    public int Minutes;
    public HashSet<string> Open = new();

    public string ElfLocation;
    public string ElfAction;
    public string ElfHistory;
    public string ElfLastLocation;

    public string EleLocation;
    public string EleAction;
    public string EleHistory;
    public string EleLastLocation;
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