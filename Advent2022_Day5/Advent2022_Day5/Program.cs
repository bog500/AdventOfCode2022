using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Text;

Console.WriteLine("Advent of Code 2022 - Day 5");
Console.WriteLine("\n-------------------");


var lines = File.ReadAllLines("clue.txt");


bool isMoveOperation = false;

List<List<char>> stacks = null;


foreach(var line in lines)
{
    if (string.IsNullOrWhiteSpace(line))
        continue;

    if(isMoveOperation)
    {
        string clean = line.Replace("move", "").Replace("from", "").Replace("to", "");

        int qty = int.Parse(clean.Split(' ')[1]);
        int from = int.Parse(clean.Split(' ')[3])-1;
        int to = int.Parse(clean.Split(' ')[5])-1;

        Move(qty, from, to);
        //Move2(qty, from, to);
    }
    else
    {
        if (line.Contains("1"))
        {
            // setup completed
            isMoveOperation = true;
            Print();
            continue;
        }

        // is still in setup
        if (stacks == null)
        {
            // initialization
            stacks = new List<List<char>>();
            for (int i = 0; i < (line.Length + 1) / 4; i++)
            {
                stacks.Add(new List<char>());
            }
        }
        int charNum = 0;
        foreach (var c in line)
        {
            if (c != ' ' && c != '[' && c != ']')
            {
                int stackNum = charNum / 4;
                Add(stackNum, c);
            }
            charNum++;
        }
    }
}

// print answer
Print();
string answer = "";
foreach(var s in stacks)
{
    if (s.Any())
        answer += s.Last();
    else
        answer += " ";
}
Console.WriteLine(answer);
Console.ReadKey();


// ##########################################################

void Add(int to, char v)
{
    stacks[to].Insert(0, v);
}

// Part 1
void Move(int qty, int from, int to)
{
    var source = stacks[from];
    var target = stacks[to];

    for (int i = 0; i < qty; i++)
    {
        char c = source.Last();
        source.RemoveAt(source.Count - 1);

        target.Add(c);
    }
}

// Part 2
void Move2(int qty, int from, int to)
{
    var source = stacks[from];
    var target = stacks[to];

    int positionSource = source.Count - qty;

    for (int i = 0; i < qty; i++)
    {
        char c = source.ElementAt(positionSource);
        source.RemoveAt(positionSource);

        target.Add(c);
    }
}

void Print()
{
    int max = stacks.Max(o => o.Count);
    for(int i = max-1 ; i >= 0; i--)
    {
        foreach(var s in stacks)
        {
            if(s.Count <= i)
            {
                Console.Write("    ");
            }
            else
            {
                Console.Write($"[{s[i]}] ");
            }
        }
        Console.Write("\n");
    }
    for(int i = 1; i <= stacks.Count; i++)
    {
        Console.Write($" {i}  ");
    }
    Console.WriteLine("\n-------------------");
}