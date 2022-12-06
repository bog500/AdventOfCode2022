using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Text;

Console.WriteLine("Advent of Code 2022 - Day 6");
Console.WriteLine("\n-------------------");

// part1
int charCount = 4;

// part2
//int charCount = 14;



var txt = File.ReadAllText("clue.txt");

List<char> buffer = new List<char>();

int index = 0;
foreach(char c in txt)
{
    Add(c);
    if(buffer.Distinct().Count() == charCount)
    {
        Console.WriteLine(index+1);
        break;
    }
    index++;
}

Console.ReadKey();

void Add(char c)
{
    if(buffer.Count == charCount)
        buffer.RemoveAt(0);

    buffer.Add(c);
}