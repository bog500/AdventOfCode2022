Console.WriteLine("Advent of Code 2022 - Day 8");
Console.WriteLine("\n-------------------");


var lines = File.ReadAllLines("clue1.txt");

List<MyClass> list = new List<MyClass>() {  };


foreach (string line in lines)
{
    if (line.StartsWith(""))
    {
       
    }
    else if (line.StartsWith(""))
    {
    }
}

int totalPart1 = 0;

foreach (var d in list.Where(o => true))
{
    totalPart1 += 0;
}

Console.WriteLine("Part1:" + totalPart1);

int totalPart2 = 0;

Console.WriteLine("Part2:" + totalPart2);

Console.ReadKey();


class MyClass
{
    public string Attr1 { get; set; }
    public int Attr2 { get; set; }
}