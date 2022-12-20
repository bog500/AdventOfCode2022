using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using Advent2022_Day16v2;

string cluefile = "clue.txt";

Dictionary<string, Valve> allValves = Setup();

Stopwatch sw = new();


int NbNodes = 0;

CalculateMoveCosts();

Prune();

Activity currentBest = new()
{
    MaxMinutes = 30
};

Valve best = allValves.OrderBy(o => o.Value.Presure).Last().Value;

List<Valve> toVisit = allValves.Values.Where(o => o.Code != "AA").OrderBy(o => Guid.NewGuid()).ToList();

Part1();


Part2();

void Prune()
{
    List<string> toDelete = allValves.Where(o => o.Value.Presure == 0 && o.Key != "AA").Select(o => o.Key).ToList();

    foreach (string code in toDelete)
    {
        allValves.Remove(code);
        foreach(var v in allValves.Values)
        {
            v.MoveCosts.Remove(code);
        }
    }
}

void CalculateMoveCosts()
{
    // initial
    foreach (var v in allValves.Values)
    {
        foreach(var v2 in v.Voisins)
        {
            v.MoveCosts.Add(v2, 1);
        }
        v.MoveCosts.Add(v.Code, 0); // self (no move)
    }

    while (allValves.Any(o => o.Value.MoveCosts.Count != allValves.Count))
    {
        foreach (var valve in allValves.Values)
        {
            foreach (var voisin in valve.Voisins)
            {
                Valve valVoisin = allValves[voisin];
                foreach (var vTarget in valVoisin.MoveCosts)
                {
                    if (valve.MoveCosts.ContainsKey(vTarget.Key))
                    {
                        if (valve.MoveCosts[vTarget.Key] > vTarget.Value + 1)
                        {
                            valve.MoveCosts[vTarget.Key] = vTarget.Value + 1;
                        }
                    }
                    else
                    {
                        valve.MoveCosts.Add(vTarget.Key, vTarget.Value + 1);
                    }
                }
            }
        }
    }

}

void CheckBest(HashSet<Valve> elf, HashSet<Valve> ele)
{
    Activity activity = new()
    {
        MaxMinutes = currentBest.MaxMinutes
    };

    Valve previous = allValves["AA"];
    int totalMove = 0;
    foreach(var v in elf)
    {
        int move = 1 + allValves[previous.Code].MoveCosts[v.Code];
        activity.elf.Add(totalMove + move, v.Presure);
        totalMove += move;
        previous = v;
    }

    previous = allValves["AA"];
    totalMove = 0;
    foreach (var v in ele)
    {
        int move = 1 + allValves[previous.Code].MoveCosts[v.Code];
        activity.ele.Add(totalMove + move, v.Presure);
        totalMove += move;
        previous = v;
    }


    int totalPresure = Util.TotalPresure(activity.MaxMinutes, activity);
    activity.Value = totalPresure;

    if (totalPresure > currentBest.Value)
    {
        currentBest = activity;
        Console.WriteLine("=========");
        Console.WriteLine("New Best: " + currentBest.Value + "    " + sw.Elapsed.TotalSeconds);
        foreach(var v in elf)
        {
            Console.WriteLine("Elf: " + v.Code + " : " + v.Presure);
        }
        foreach (var v in ele)
        {
            Console.WriteLine("Ele: " + v.Code + " : " + v.Presure);
        }
    }
        

}

void Part1()
{
    sw.Reset();
    sw.Start();
    currentBest = new()
    {
        MaxMinutes = 30
    };

    HashSet<Valve> elfValves = new();
    HashSet<Valve> eleValves = new();

    Loop1(elfValves, eleValves, allValves["AA"], 16, 0, 30);
    Console.WriteLine("Part 1 completed - click to continue to part 2 (very long)");
    Console.ReadKey();
}

void Part2()
{
    sw.Reset();
    sw.Start();
    currentBest = new()
    {
        MaxMinutes = 26
    };

    HashSet<Valve> elfValves = new();
    HashSet<Valve> eleValves = new();


    Loop2(elfValves, eleValves, allValves["AA"], currentDepth:8, maxDepth: 8,totalTime: 0, maxTime: 26);
    Console.WriteLine("Part 2 completed");
}

void Loop2(HashSet<Valve> elfValves, HashSet<Valve> eleValves, Valve previousValve, int currentDepth, int maxDepth, int totalTime, int maxTime)
{
    int nbCheck = 0;
    foreach (var nextValve in toVisit)
    {
        nbCheck++;

        if (elfValves.Contains(nextValve) || eleValves.Contains(nextValve))
            continue;

        eleValves.Add(nextValve);
        totalTime += previousValve.MoveCosts[nextValve.Code] + 1;

        if(eleValves.Count >= 4)
            Loop1(elfValves, eleValves, allValves["AA"], maxDepth, totalTime: 0, maxTime);

        if (currentDepth > 0 && totalTime < maxTime)
            Loop2(elfValves, eleValves, nextValve, currentDepth - 1, maxDepth, totalTime, maxTime);

        //eleValves.Pop();
        eleValves.Remove(nextValve);
        totalTime -= (previousValve.MoveCosts[nextValve.Code] + 1);
    }
}


void Loop1(HashSet<Valve> elfValves, HashSet<Valve> eleValves, Valve previousValve, int depth, int totalTime, int maxTime)
{
    int nbCheck = 0;
    foreach (var nextValve in toVisit)
    {
        nbCheck++;

        if (elfValves.Contains(nextValve) || eleValves.Contains(nextValve))
            continue;

        elfValves.Add(nextValve);
        totalTime += previousValve.MoveCosts[nextValve.Code] + 1;

        if(elfValves.Count + elfValves.Count + 5 > toVisit.Count)
            CheckBest(elfValves, eleValves);

        if (depth > 0 && totalTime < maxTime)
            Loop1(elfValves, eleValves, nextValve, depth-1, totalTime, maxTime);

        //elfValves.Pop();
        elfValves.Remove(nextValve);
        totalTime -= (previousValve.MoveCosts[nextValve.Code] + 1);
    }
}



Dictionary<string, Valve> Setup()
{
    Dictionary<string, Valve> list = new();

    var lines = File.ReadAllLines(cluefile);

    foreach (var line in lines)
    {
        string code = line.Substring(6, 2);

        int pFrom = line.IndexOf("=") + 1;
        int pTo = line.IndexOf(";");

        int presure = int.Parse(line.Substring(pFrom, pTo - pFrom));

        Valve v = new(code, presure);

        int pValve = line.Replace("valves", "valve").IndexOf("to valve ") + "to valve ".Length;

        string s = line.Substring(pValve, line.Length - pValve);

        foreach (var voisin in s.Replace(" ", "").Split(','))
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

    public Dictionary<string, int> MoveCosts = new();

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

class Activity
{
    public int MaxMinutes;

    public Dictionary<int, int> elf = new();
    public Dictionary<int, int> ele = new();

    public int Value;


}

static class Util
{
    public static int TotalPresure(int minutes, Activity activity)
    {

        int elfRelease = 0;
        int eleRelease = 0;

        int rate = 0;
        int total = 0;

        for (int m = 1; m <= minutes; m++)
        {
            total += rate;

            if (activity.elf.ContainsKey(m))
                elfRelease = activity.elf[m];
            else
                elfRelease = 0;

            if (activity.ele.ContainsKey(m))
                eleRelease = activity.ele[m];
            else
                eleRelease = 0;

            rate += elfRelease + eleRelease;
        }
        return total;
    }

}