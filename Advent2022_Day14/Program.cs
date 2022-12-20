Console.WriteLine("Advent of Code 2022 - Day 14");
Console.WriteLine("\n-------------------");

string cluefile = "clue1.txt";

Dictionary<(int X, int Y), Content> cave = new();

Part1();
Part2();

void Part1()
{
    Console.WriteLine("Part 1");
    Setup();
    int NbSand = Simulate();
    Print();
    Console.WriteLine($"Number of sand particules: {--NbSand}"); // fix off-by-one
    Console.WriteLine("-------------------");
}

void Part2()
{
    Console.WriteLine("Part 2");

    // reset setup
    Setup();
    AddFloor(2);

    //Print();
    int NbSand = Simulate();
    Console.WriteLine($"Number of sand particules: {NbSand}");
    Console.WriteLine("-------------------");
}

int Simulate()
{
    int nbSand = 0;
    bool canContinue = true;
    while (cave[(500,0)] == Content.Air && canContinue)
    {
        canContinue = AddSand((500, 0));
        nbSand++;
        Print();
    }

    //Print();

    return nbSand;
}

bool AddSand((int X, int Y) p)
{
    (int X, int Y) down = (p.X, p.Y + 1);
    (int X, int Y) left = (p.X-1, p.Y + 1);
    (int X, int Y) right = (p.X+1, p.Y + 1);

    // down void check
    if (cave.ContainsKey(down) == false)
        return false; // fell into void

    // down air check
    if (cave[down] == Content.Air)
        return AddSand(down); // fall 1 down



    if (!cave.ContainsKey(left))
        return false; // fell into void on left side

    if (cave[left] == Content.Air)
        return AddSand(left);


    if (!cave.ContainsKey(right))
        return false; // fell into void on right side

    if (cave[right] == Content.Air)
        return AddSand(right);

    // balanced
    if (cave[p] == Content.Air)
    {
        cave[p] = Content.Sand;
        return true;
    }
    //full
    return false;

}

void AddFloor(int dept)
{
    int currentMaxY = cave.Keys.Max(o => o.Y);
    int minX = cave.Keys.Min(o => o.X);
    int maxX = cave.Keys.Max(o => o.X);

    int realMaxY = currentMaxY + dept;

    int buffer = 10;

    for (int x = minX - realMaxY - buffer; x <= maxX + realMaxY + buffer; x++)
    {
        for (int y = 0 ; y <= realMaxY; y++)
        {
            if (cave.ContainsKey((x, y)))
                continue;

            cave[(x, y)] = (y - realMaxY) switch
            {
                0 => Content.Rock,
                _ => Content.Air
            };
        }
    }
}

void Setup()
{
    cave = new();

    var content = File.ReadAllText(cluefile).Replace("->", "\n").Replace("\r", "\n").Replace(" ","");

    int minX = int.MaxValue;
    int maxX = int.MinValue;
    int maxY = int.MinValue;

    foreach (string s in content.Split('\n'))
    {
        if (string.IsNullOrWhiteSpace(s))
            continue;

        var p = ParsePosition(s);

        minX = p.X < minX ? p.X : minX;
        maxX = p.X > maxX ? p.X : maxX;
        maxY = p.Y > maxY ? p.Y : maxY;
    }

    for(int x = minX; x <= maxX; x++)
    {
        for (int y = 0; y <= maxY; y++)
        {
            cave.Add((x, y), Content.Air);
        }
    }

    var lines = File.ReadAllLines(cluefile);

    foreach(string line in lines)
    {
        (int X, int Y)? prevP = null;

        string line2 = line.Replace(" ", "").Replace("->", ";");
        foreach(string s in line2.Split(';'))
        {
            var p = ParsePosition(s);
            if (prevP != null)
            {
                var list = GetAllPosition(prevP.Value, p);
                foreach(var rock in list)
                {
                    cave[rock] = Content.Rock;
                }
            }
            prevP = p;
        }
    }
}


List<(int X, int Y)> GetAllPosition((int X, int Y) from, (int X, int Y) to)
{
    List<(int X, int Y)> l = new() { from };

    while(from.X != to.X || from.Y != to.Y)
    {
        int newX = (to.X - from.X) switch
        {
            > 0 => from.X + 1,
            < 0 => from.X - 1,
            _ => from.X
        };

        int newY = (to.Y - from.Y) switch
        {
            > 0 => from.Y + 1,
            < 0 => from.Y - 1,
            _ => from.Y
        };

        from = (newX, newY);
        l.Add(from);
    }

    return l;
}

(int X, int Y) ParsePosition(string s)
{
    int x = int.Parse(s.Split(',')[0]);
    int y = int.Parse(s.Split(',')[1]);

    return (X: x, Y: y);
}

void Print()
{
    Console.Clear();

    int minX = cave.Keys.Min(o => o.X);
    int maxX = cave.Keys.Max(o => o.X);
    int minY = cave.Keys.Min(o => o.Y);
    int maxY = cave.Keys.Max(o => o.Y);

    for (int y = minY ; y <= maxY ; y++)
    {
        for (int x = minX; x <= maxX; x++)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;

            if (cave.Keys.Contains((x, y)) == false)
                continue;

            switch (cave[(x,y)])
            {
                case Content.Air:
                    Console.Write('.');
                    break;

                case Content.Rock:
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write('▓');
                    break;

                case Content.Sand:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write('o');
                    break;
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        Console.Write('\n');
    }
    Console.WriteLine("");
}

enum Content {  Air, Rock, Sand }