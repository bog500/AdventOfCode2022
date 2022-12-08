int currentCal = 0;

var lines = File.ReadAllLines("clue.txt");

List<int> cal = new();

foreach(string line in lines)
{
    if(line == "")
    {
        cal.Add(currentCal);
        currentCal = 0;
    }
    else
    {
        currentCal += int.Parse(line);
    }
}

Console.WriteLine("Part1:" + cal.Max());

int top3 = cal.OrderDescending().Take(3).Sum();

Console.WriteLine("Part2:" + top3);

Console.ReadKey();