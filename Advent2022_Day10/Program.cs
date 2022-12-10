using System.Security.Cryptography.X509Certificates;
using System.Text;

Console.WriteLine("Advent of Code 2022 - Day 10");
Console.WriteLine("\n-------------------");


var lines = File.ReadAllLines("clue.txt");
string output = "";

const int rowLength = 40;

List<int> values = new List<int>();

int cycle = 0;
int currentX = 1;
foreach (string line in lines)
{
    string command = line.Split(' ').First();
    int value = 0;
    if(command == "noop")
    {
        DoCycle();
    }
    else if(command == "addx")
    {
        DoCycle();
        DoCycle();

        value = int.Parse(line.Split(' ').Last());
        currentX += value;
    }
}

int total = GetStrength(20) + GetStrength(60) + GetStrength(100) + GetStrength(140) + GetStrength(180) + GetStrength(220);

Console.WriteLine("Part1: " + total);
Console.WriteLine("Part2: " + output);

int GetStrength(int cycle)
{
    return values[cycle - 1] * cycle;
}

void DoCycle()
{
    AddPixel(cycle, currentX);
    values.Add(currentX);
    cycle++;
}

void AddPixel(int cycle, int x)
{
    if (cycle % rowLength == 0)
        output += "\n";

    int row = cycle / rowLength;
    int val = cycle - rowLength * row;

    if (val+1 >= x && val-1 <= x)
        output += "#";
    else
        output += ".";
}
