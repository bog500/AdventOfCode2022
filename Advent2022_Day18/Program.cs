Console.WriteLine("Advent of Code 2022");
Console.WriteLine("      Day 18");
Console.WriteLine("===================");

var lines = File.ReadAllLines("clue.part1.txt");

HashSet<Lava> drops = new();
HashSet<(int Id1, int Id2)> touches = new();

int id = 0;
foreach(var line in lines)
{
    int x = int.Parse(line.Split(',')[0]);
    int y = int.Parse(line.Split(',')[1]);
    int z = int.Parse(line.Split(',')[2]);

    Lava l = new()
    {
        Position = (x, y, z),
        Id = ++id
    };

    drops.Add(l);
}

foreach(var lava in drops)
{
    var touchingIds = drops.Where(o => o.Id != lava.Id && lava.Touches(o)).Select(o => o.Id);
    
    foreach(var i in touchingIds)
    {
        int id1 = Math.Min(i, lava.Id);
        int id2 = Math.Max(i, lava.Id);
        if (touches.Contains((id1, id2)) == false)
            touches.Add((id1, id2));
    }
}

int maxSides = drops.Count * 6;

int part1 = maxSides - (touches.Count * 2);

Console.WriteLine($"Part 1: {part1}");

public class Lava
{
    public (int X, int Y, int Z) Position;
    public int Id;

    public bool Touches(Lava lava)
    {
        if (this.Position.X == lava.Position.X && this.Position.Y == lava.Position.Y && Math.Abs(this.Position.Z - lava.Position.Z) <= 1)
            return true;

        if (this.Position.X == lava.Position.X && this.Position.Z == lava.Position.Z && Math.Abs(this.Position.Y - lava.Position.Y) <= 1)
            return true;

        if (this.Position.Z == lava.Position.Z && this.Position.Y == lava.Position.Y && Math.Abs(this.Position.X - lava.Position.X) <= 1)
            return true;

        return false;
    }
}