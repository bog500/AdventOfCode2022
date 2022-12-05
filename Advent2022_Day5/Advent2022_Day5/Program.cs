using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Text;

Console.WriteLine("Advent of Code - Day 5");
Console.WriteLine("\n-------------------");


var lines = File.ReadAllLines("clue.txt");


bool isMoveOperation = false;

List<List<char>> stacks = null;

int nbMoves = 0;

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
        nbMoves++;
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
                Console.Write("[" + s[i] + "] ");
            }
        }
        Console.Write("\n");
    }
    for(int i = 1; i <= stacks.Count; i++)
    {
        Console.Write(" " + i + "  ");
    }
    Console.WriteLine("\n-------------------");
}


/* ################################################## */


    public class Day05
    {
        public List<int[]> moves = new List<int[]>();

        public class State
        {
            public List<char[]> stacks = new List<char[]>();
            public int[] sizes;
            public State(int nstacks, int maxn)
            {
                sizes = new int[nstacks];
                for (int k = 0; k < nstacks; k++)
                {
                    stacks.Add(new char[nstacks * maxn]);
                    sizes[k] = 0;
                }
            }
            public State(State other)
            {
                sizes = (int[])other.sizes.Clone();
                foreach (var stack in other.stacks) stacks.Add((char[])stack.Clone());
            }

            public void move1(int x, int a, int b)
            {
                int l = sizes[a];
                for (int i = 0; i < x; i++) stacks[b][sizes[b]++] = stacks[a][l - i - 1];
                sizes[a] -= x;
            }

            public void move2(int x, int a, int b)
            {
                int l = sizes[a];
                Array.Copy(stacks[a], sizes[a] - x, stacks[b], sizes[b], x);
                sizes[a] -= x; sizes[b] += x;
            }

            public String top()
            {
                string ret = "";
                for (int i = 0; i < stacks.Count(); i++) ret += stacks[i][sizes[i] - 1];
                return ret;
            }
        }

        public State? initial;

        public void parse(List<string> input)
        {
            int i;
            for (i = 0; i < input.Count(); i++) if (input[i].Length < 2) break;
            int nstacks = (input[i - 1].Length + 1) / 4;
            initial = new State(nstacks, i - 1);
            for (int j = i - 2; j >= 0; j--)
            {
                for (int k = 0; k < nstacks; k++)
                {
                    if (input[j][k * 4 + 1] != ' ') initial.stacks[k][initial.sizes[k]++] = input[j][k * 4 + 1];
                }
            }
            for (int j = i + 1; j < input.Count(); j++)
            {
                var parts = input[j].Split(' ');
                moves.Add(new int[] { int.Parse(parts[1]), int.Parse(parts[3]) - 1, int.Parse(parts[5]) - 1 });
            }
        }


        public string part1()
        {
            State tmp = new State(initial!);
            foreach (var move in moves) tmp.move1(move[0], move[1], move[2]);
            return tmp.top();
        }

        public string part2()
        {
            State tmp = new State(initial!);
            foreach (var move in moves) tmp.move2(move[0], move[1], move[2]);
            return tmp.top();
        }
    }
