
using System;

List<(bool moved, int x)> list = new();

//Demo1();
Part1();

void Demo1()
{
    list = Parse("clue1.txt");
    Loop(1);
    int groove = Groove();
    Console.WriteLine("\nDemo1: " + groove);
}

void Part1()
{
    list = Parse("clue.txt");
    Loop(1);
    int groove = Groove();
    Console.WriteLine("\nPart1: " + groove);
}

void Loop(int times)
{
    //Print();
    for (int time = 0; time < times; time++)
    {

        for(int index = 0; index < list.Count; index++)
        {
            var val = list[index];

            if (val.moved)
                continue;

            list.RemoveAt(index);

            val.moved = true;
            int insertIndex = IndexLoop(index + val.x);
            list.Insert(insertIndex, val);
            //Print(insertIndex);

            index = Math.Max(-1, Math.Min(insertIndex, index-3));
        }

    }
}

int Groove()
{
    //var zero = list.First(o => o.x == 0);

    int zeroIndex = list.IndexOf((true, 0));

    int index1 = IndexLoop(zeroIndex + 1000);
    int index2 = IndexLoop(zeroIndex + 2000);
    int index3 = IndexLoop(zeroIndex + 3000);

    int i = IndexLoop(0);

    int i1 = list[index1].x;
    int i2 = list[index2].x;
    int i3 = list[index3].x;

    return i1 + i2 + i3;
}

void Print(int index = -1)
{
    Console.WriteLine("Printing:");

    int i = 0;
    foreach (var v in list.ToArray()[0..^1])
    {
        Console.ForegroundColor = index == i ? ConsoleColor.Green : ConsoleColor.Gray;
        Console.Write(v.x.ToString() + ", ");
        i++;
    }
    Console.ForegroundColor = index == i ? ConsoleColor.Green : ConsoleColor.Gray;
    Console.WriteLine(list.Last().x.ToString());

    Console.ForegroundColor = ConsoleColor.Gray;

}

int IndexLoop(int index)
{
    while (index >= list.Count)
        index = index - list.Count;

    while(index <= 0)
        index = index + list.Count;

    return index;
}

List<(bool moved, int x)> Parse(string filename)
{
    var lines = File.ReadAllLines(filename);
    List<(bool moved, int x)> l = new();
    foreach (var line in lines)
    {
        int x = int.Parse(line);
        var v = (false, x);
        l.Add(v);
    }
    return l;
}