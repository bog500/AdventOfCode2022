Console.WriteLine("Advent of Code 2022 - Day 9");
Console.WriteLine("\n-------------------");


var lines = File.ReadAllLines("clue.txt");

var headPath = new List<Position>() { new() };

foreach (string line in lines)
{
    string direction = line.Split(' ').First();
    int move = int.Parse(line.Split(' ').Last());

    for(int i = 0; i < move; i++)
    {
        var currentHead = headPath.Last().Clone();

        currentHead.Y = direction switch
        {
            "U" => currentHead.Y+1,
            "D" => currentHead.Y-1,
            _ => currentHead.Y
        };

        currentHead.X = direction switch
        {
            "L" => currentHead.X-1,
            "R" => currentHead.X+1,
            _ => currentHead.X
        };

        headPath.Add(currentHead);
    }
}


Console.WriteLine("Part1:" + RopeCalculator(1));
Console.WriteLine("Part2:" + RopeCalculator(9));


Console.ReadKey();

int RopeCalculator(int size)
{

    Dictionary<int, List<Position>> tailsPath = new Dictionary<int, List<Position>>();

    for (int i = 1; i <= size; i++)
    {
        tailsPath.Add(i, new List<Position>() { new() });
    }


    foreach (var headPos in headPath)
    {
        for (int i = 1; i <= size; i++)
        {
            Position lead = i == 1 ? headPos : tailsPath[i - 1].Last();

            Position follow = tailsPath[i].Last();

            Position next = GetNextPos(lead, follow);
            if (next != follow)
            {
                tailsPath[i].Add(next);
            }
        }
    }

    return tailsPath[size].Distinct().Count();
}

Position GetNextPos(Position head, Position tail)
{
    var newTail = (head.X - tail.X, head.Y - tail.Y) switch
        {
            ( > 1, > 1) => (head.X - 1, head.Y - 1),
            ( > 1, < -1) => (head.X - 1, head.Y + 1),
            ( < -1, > 1) => (head.X + 1, head.Y - 1),
            ( < -1, < -1) => (head.X + 1, head.Y + 1),
            ( > 1, _) => (head.X - 1, head.Y),
            ( < -1, _) => (head.X + 1, head.Y),
            (_, > 1) => (head.X, head.Y - 1),
            (_, < -1) => (head.X, head.Y + 1),
            _ => (tail.X, tail.Y),
        };

    return new Position(newTail.Item1, newTail.Item2);

}

class Position
{
    public int X;
    public int Y;

    public Position() { }

    public Position(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public override int GetHashCode()
    {
        return (this.X + ";" + this.Y).GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (obj.GetType() != typeof(Position)) return false;

        return ((Position)obj).X == this.X && ((Position)obj).Y == this.Y;
    }

    public Position Clone()
    {
        return new Position(this.X, this.Y);
    }
}