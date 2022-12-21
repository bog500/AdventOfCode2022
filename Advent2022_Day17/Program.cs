
using System.Collections.Generic;
using System.IO;
using System.Numerics;

Rock rock_horizon = new("horizon");
Rock rock_plus = new("plus");
Rock rock_l = new("L");
Rock rock_vertical = new("vertical");
Rock rock_sq = new("square");
SetupRocks();

int fallTick = 1;

var winds = File.ReadAllBytes("clue1.txt").Select(o => Convert.ToChar(o)).Where(o => o == '<' || o == '>').ToList();
int windIndex = 0;

Rock rock = rock_sq.Clone(); // will call Next in loop

HashSet<(int x, int y)> space = new();

Part1();
Part2();



void Part2()
{
    /* PART 2 IS NOT WORKING */

    // reset
    windIndex = 0;
    rock = rock_sq.Clone(); // will call Next in loop
    space = new();


    int nbSimulate = 5 * winds.Count; //50455
    long mod = 1000000000000 % nbSimulate;

    BigInteger smallHeight = Run(nbSimulate);

    BigInteger bigHeight = smallHeight * ((1000000000000 - mod) / nbSimulate);


    // reset
    windIndex = 0;
    rock = rock_sq.Clone(); // will call Next in loop
    space = new();

    BigInteger missingHeight = Run((int)mod);

    BigInteger total = bigHeight + missingHeight;
    Console.WriteLine($"Part 2 Height = {total}");
}

void Part1()
{
    // reset
    windIndex = 0;
    rock = rock_sq.Clone(); // will call Next in loop
    space = new();

    //run
    int part1 = Run(2022);
    Console.WriteLine($"Part 1 Height = {part1}");
}



int Run(int nbRocks)
{
    for (int i = 0; i < nbRocks; i++)
    {
        rock = rock.Next.Clone();
        rock.Position = (2, SpaceHeight() + 4);
        //Print(rock);

        int windDir = NextWind();
        if (CanPush(rock, windDir))
        {
            rock.Push(windDir);
            //Print(rock);
        }

        while (CanFall(rock, fallTick))
        {
            rock.Fall(fallTick);
            //Print(rock);

            windDir = NextWind();

            if (CanPush(rock, windDir))
            {
                rock.Push(windDir);
                //Print(rock);
            }

        }
        AddToSpace(rock);
    }
    return SpaceHeight() + 1;
}

int SpaceHeight()
{
    if(space.Any())
        return space.Max(o => o.y);
    return -1;
}

void AddToSpace(Rock rock)
{
    foreach(var p in rock.PartsLocation())
    {
        space.Add(p);
    }
}

bool CanPush(Rock rock, int x)
{
    foreach (var p in rock.PartsLocation())
    {
        if (p.x + x < 0) //left
            return false;

        if (p.x + x > 6) //right
            return false;

        if (space.Contains((p.x + x, p.y))) // blocked
            return false;
    }
    return true;
}


bool CanFall(Rock rock, int y)
{
    foreach(var p in rock.PartsLocation())
    {
        if (p.y - y < 0) //bottom
            return false;

        if (space.Contains((p.x, p.y-y))) // blocked
            return false;
    }
    return true;
}



void SetupRocks()
{
    rock_horizon.Parts.Add((0, 0));
    rock_horizon.Parts.Add((1, 0));
    rock_horizon.Parts.Add((2, 0));
    rock_horizon.Parts.Add((3, 0));
    rock_horizon.Next = rock_plus;


    rock_plus.Parts.Add((0, 1));
    rock_plus.Parts.Add((1, 0));
    rock_plus.Parts.Add((1, 1));
    rock_plus.Parts.Add((1, 2));
    rock_plus.Parts.Add((2, 1));
    rock_plus.Next = rock_l;


    rock_l.Parts.Add((0, 0));
    rock_l.Parts.Add((1, 0));
    rock_l.Parts.Add((2, 0));
    rock_l.Parts.Add((2, 1));
    rock_l.Parts.Add((2, 2));
    rock_l.Next = rock_vertical;

    rock_vertical.Parts.Add((0, 0));
    rock_vertical.Parts.Add((0, 1));
    rock_vertical.Parts.Add((0, 2));
    rock_vertical.Parts.Add((0, 3));
    rock_vertical.Next = rock_sq;

    rock_sq.Parts.Add((0, 0));
    rock_sq.Parts.Add((0, 1));
    rock_sq.Parts.Add((1, 0));
    rock_sq.Parts.Add((1, 1));
    rock_sq.Next = rock_horizon;
}

int NextWind()
{
    char wind = winds[windIndex];
    windIndex++;

    if (windIndex == winds.Count)
        windIndex = 0;

    int windDir = wind switch
    {
        '<' => -1,
        '>' => 1,
        _ => 0 //huh?
    };

    return windDir;
}

void Print(Rock rock = null)
{
    Console.Clear();

    int minX = 0;
    int minY = 0;

    int maxX = 6;
    int maxY = 6;

    if(space.Any())
    {
        minX = Math.Min(minX, space.Min(o => o.x));
        minY = Math.Min(minY, space.Min(o => o.y));

        maxX = Math.Max(maxX, space.Max(o => o.x));
        maxY = Math.Max(maxY, space.Max(o => o.y));
    }

    HashSet<(int x, int y)> rockParts = new();
    if(rock != null)
    {
        rockParts = rock.PartsLocation();
        maxX = Math.Max(maxX, rockParts.Max(o => o.x));
        maxY = Math.Max(maxY, rockParts.Max(o => o.y));
    }


    for (int y = maxY; y >= minY ; y--)
    {
        for (int x = minX; x <= maxX; x++)
        {
            if (space.Contains((x, y)))
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write('#');
            } 
            else if(rockParts.Contains((x, y)))
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write('@');
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write('.');
            }
                
        }
        Console.Write('\n');
    }
}



class Rock
{
    public (int x, int y) Position;
    public HashSet<(int x, int y)> Parts = new();
    public readonly string Name;

    public Rock(string name)
    {
        this.Name = name;
    }

    public HashSet<(int x, int y)> PartsLocation()
    {
        HashSet<(int x, int y)> ploc = new();

        foreach (var p in Parts)
        {
            ploc.Add((this.Position.x + p.x, this.Position.y + p.y));
        }

        return ploc;
    }

    public Rock Clone()
    {
        Rock r = new(this.Name)
        {
            Position = this.Position,
            Parts = new(this.Parts),
            Next = this.Next
        };
        return r;
    }

    public void Fall(int y = 1)
    {
        this.Position.y = this.Position.y - 1;
    }

    public void Push(int windDir)
    {
        this.Position.x = this.Position.x + windDir;
    }

    public Rock Next;
}

