using System.Diagnostics;
using System.Numerics;

//string cluefile = "clue1.txt";
//int yCheck = 10;




string cluefile = "clue.txt";
int yCheck = 2000000;


const double RadianConverter = System.Math.PI / 180.0;

List<Sensor> sensors = Setup();

HashSet<(int x, int y)> sensorsPositions = new();
HashSet<(int x, int y)> beaconsPositions = new();

foreach(var s in sensors)
{
    sensorsPositions.Add(s.Position);
    beaconsPositions.Add(s.Beacon);
}


using (new Timer())
{
    Part1();
}



//using (new Timer())
//{
    // Part2New();
//}






void Part1()
{
    int buffer = 1000000;

    int minX = Math.Min(sensors.Min(o => o.Position.x), sensors.Min(o => o.Beacon.x)) - buffer;
    int maxX = Math.Min(sensors.Max(o => o.Position.x), sensors.Max(o => o.Beacon.x)) + buffer;

    int noBeacon = 0;
    for(int x = minX; x <= maxX; x++)
    {
        (int x, int y) position = (x, yCheck);

        if (BeaconPossible(position) == false)
            noBeacon++;

    }

    Console.WriteLine("Part 1: " + noBeacon);
}

/*
void Part2New()
{
    int searchMinX = 0;
    int searchMaxX = 4000000;
    int searchMinY = 0;
    int searchMaxY = 4000000;
    int patternBuffer = 10000;


    var points = DeepSearch(searchMinX, searchMaxX, searchMinY, searchMaxY, patternBuffer);

    foreach(var p in points)
    {
        if(BeaconPossible(p, true))
        {
            Console.WriteLine($"Part 2: X:{p.x}  Y:{p.y}");
            Console.WriteLine($"Freq: {GetFreq(p)}");
        }    
    }
}
*/

/*
List<(int x, int y)> DeepSearch(int searchMinX, int searchMaxX, int searchMinY, int searchMaxY, int patternBuffer)
{
    List<(int x, int y)> list = new();

    if (patternBuffer < 1)
        patternBuffer = 1;

    int distanceBuffer = (int)(patternBuffer / 2.5);
    if (distanceBuffer < 1)
        distanceBuffer = 1;

    if (searchMaxX - searchMinX <= 1000)
    {
        patternBuffer = 1;
        distanceBuffer = 1;
    }
        

    for (int x = searchMinX; x <= searchMaxX; x += patternBuffer)
    {
        for (int y = searchMinY; y <= searchMaxY; y += patternBuffer)
        {
            if (BeaconPossible((x, y), true, distanceBuffer))
            {
                if(distanceBuffer == 1)
                {
                    list.Add((x, y));
                }
                else
                {
                    //Console.WriteLine($"Deeper..  X:{x}  Y:{y}  {patternBuffer}");
                    var subList = DeepSearch(x - patternBuffer, x + patternBuffer, y - patternBuffer, y + patternBuffer, patternBuffer / 10);
                    list.AddRange(subList);
                }
            }
        }
    }
    return list;
}
*/

/*
void Part2()
{

    List<(int x, int y)> toTest = new();

    foreach(Sensor s in sensors)
    {
        var edges = GetEdges(s);
        toTest.AddRange(edges);
    }

    var d = toTest.Distinct().ToList();

    int bb = 0;
    foreach(var pos in toTest)
    {
        bb++;

        if (BeaconPossible(pos, true))
        {
            BigInteger f = GetFreq(pos);
            Console.WriteLine($"X: {pos.x}  Y: {pos.y}  Freq: {f.ToString()}");
        }
        else
        {

        }
    }

}
*/


BigInteger GetFreq((int x, int y) p) => BigInteger.Add(BigInteger.Multiply(p.x, 4000000), p.y);

/*
int GetCircleX(int x, double radius, double angle) => (int)(x + radius * System.Math.Cos(angle));
int GetCircleY(int y, double radius, double angle) => (int)(y + radius * System.Math.Sin(angle));
*/

/*
double RealDistance((int x, int y) p1, (int x, int y) p2)
{
    int xLength = Math.Abs(p1.x - p2.x);
    int yLength = Math.Abs(p1.y - p2.y);

    double xp = Math.Pow(xLength, 2);
    double yp = Math.Pow(yLength, 2);

    double hypo = Math.Sqrt(xp + yp);
    return hypo;
}
*/

/*

bool IgnoreAngle((int x, int y) p, double degree, double radius)
{
    double angle = degree * RadianConverter;

    int x = GetCircleX(p.x, radius, angle);

    if (x < -5 || x > 4000005)
        return true;

    int y = GetCircleX(p.y, radius, angle);

    if (y < -5 || y > 4000005)
        return true;

    return false;
}
*/

/*
HashSet<(int x, int y)> GetEdges(Sensor s)
{
    HashSet<(int x, int y)> l = new();

    double radius = RealDistance(s.Beacon, s.Position);

    if (IgnoreAngle(s.Position, 0, radius)
        && IgnoreAngle(s.Position, 90, radius)
        && IgnoreAngle(s.Position, 180, radius)
        && IgnoreAngle(s.Position, 270, radius)
        )
        return l;

    for (double i = 0.0; i < 360.0; i += 0.001)
    {

        double angle = i * RadianConverter;

        int x = GetCircleX(s.Position.x, radius, angle);

        if (x < -5 || x > 4000005)
            continue;

        int y = GetCircleX(s.Position.y, radius, angle);

        if (y < -5 || y > 4000005)
            continue;

        l.Add((x, y));
        l.Add((x-1, y));
        l.Add((x+1, y));

        l.Add((x, y-1));
        l.Add((x-1, y-1));
        l.Add((x+1, y-1));

        l.Add((x, y+1));
        l.Add((x-1, y+1));
        l.Add((x+1, y+1));
    }
    return l;
}
*/


bool BeaconPossible((int x, int y) position, bool ignoreBeancon = false, int buffer = 0)
{
    if (sensorsPositions.Contains(position))
    {
        return false;
    }

    if (beaconsPositions.Contains(position) && ignoreBeancon == false)
    {
        return true;
    }

    foreach (var s in sensors)
    {
        int distance = Util.GetDistance(position, s.Position) + buffer;
        if (distance <= s.BeaconDistance)
        {
            return false;
        }
    }
    return true;
}

List<Sensor> Setup()
{
    var lines = File.ReadAllLines(cluefile);

    List<Sensor> sensors = new();

    foreach (var line in lines)
    {
        string clean = line
            .Replace("Sensor at x=", "")
            .Replace(" closest beacon is at x=", "")
            .Replace("y=", "")
            .Replace(" ", "");

        (int x, int y) position = Util.Parse(clean.Split(':')[0]);
        (int x, int y) beacon = Util.Parse(clean.Split(':')[1]);

        sensors.Add(new(position, beacon));
    }
    return sensors;
}


static class Util
{
    public static int GetDistance((int x, int y) p1, (int x, int y) p2)
    {
        return Math.Abs(p1.x - p2.x) + Math.Abs(p1.y - p2.y);
    }

    public static (int x, int y) Parse(string s)
    {
        int x = int.Parse(s.Split(',')[0]);
        int y = int.Parse(s.Split(',')[1]);
        return (x, y);
    }
}


class Sensor
{
    public (int x, int y) Position { get; private set; }

    public (int x, int y) Beacon { get; private set; }

    public int BeaconDistance { get; private set; }

    public Sensor((int x, int y) position, (int x, int y) beacon)
    {
        this.Position = position;
        this.Beacon = beacon;
        this.BeaconDistance = Util.GetDistance(position, beacon);
    }
}

class Timer : IDisposable
{
    Stopwatch sw = new();
    
    public Timer()
    {
        sw.Start();
    }

    public void Dispose()
    {
        sw.Stop();
        Console.WriteLine("Time taken: {0}s", sw.Elapsed.TotalSeconds);
    }
}