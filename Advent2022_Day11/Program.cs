Console.WriteLine("Advent of Code 2022 - Day 11");
Console.WriteLine("\n-------------------");


Simulation(1, 3, 20);
Simulation(2, 1, 10000);

void Simulation(long partNum, long worryDiviser, long nbRounds)
{
    List<Monkey> monkeys = Setup(worryDiviser);

    long commonDivisor = 1;
    foreach(var m in monkeys)
    {
        commonDivisor *= m.DivisibleTest;
    }

    for (long i = 0; i < nbRounds; i++)
    {
        foreach (Monkey m in monkeys)
        {
            m.DoInspection(monkeys, commonDivisor);
        }
    }

    List<Monkey> top2 = monkeys.OrderByDescending(o => o.NbInspections).Take(2).ToList();

    long partAnswer = top2[0].NbInspections * top2[1].NbInspections;

    Console.WriteLine($"Part{partNum}: {partAnswer}");

}






List<Monkey> Setup(long worryDiviser)
{
    var lines = File.ReadAllLines("clue.txt");

    long nbLines = 0;

    List<Monkey> monkeys = new();

    while (nbLines < lines.Count())
    {
        Monkey m = null;
        for (long i = 0; i is < 7; i++)
        {
            string line = lines[nbLines];
            
            switch (i)
            {
                case 0:
                    m = new Monkey(GetLastInt(line));
                    m.WorryDiviser = worryDiviser;
                    break;
                case 1:
                    m.Items = ParseItems(line);
                    break;
                case 2:
                    SetOperation(m, line);
                    break;
                case 3:
                    m.DivisibleTest = GetLastInt(line);
                    break;
                case 4:
                    m.DestinationTrue = GetLastInt(line);
                    break;
                case 5:
                    m.DestinationFalse = GetLastInt(line);
                    break;
                case 6:
                    monkeys.Add(m);
                    break;
                default:
                    break;
            }
            nbLines++;
        }
    }
    return monkeys;
}

void SetOperation(Monkey m, string line)
{
    if(line.Contains("old * old"))
    {
        m.OperationSquare = true;
    }
    else if(line.Contains("old +"))
    {
        m.OperationAdd = GetLastInt(line);
    }
    else if (line.Contains("old *"))
    {
        m.OperationMulti = GetLastInt(line);
    }
    else
    {
        throw new Exception();
    }
}

int GetLastInt(string s)
{
    return int.Parse(s.Split(' ').Last().Replace(":", "").Replace(" ",""));
}

List<long> ParseItems(string s)
{
    string items = s.Split(':').Last();
    items = items.Replace(":", "").Replace(" ", "");
    List<long> list = new();
    foreach(string sub in items.Split(','))
    {
        list.Add(long.Parse(sub));
    }
    return list;
}

class Monkey
{
    public Monkey(long nb)
    {
        this.Number = nb;
    }

    public List<long> Items = new();

    public long Number { get; private set; }

    public long NbInspections = 0;

    public long DivisibleTest = 1;

    public long DestinationTrue = 0;

    public long DestinationFalse = 0;

    public long OperationMulti = 1;

    public long OperationAdd = 0;

    public bool OperationSquare = false;

    public long WorryDiviser = 1;

    public void DoInspection(List<Monkey> monkeys, long commonDiv)
    {
        foreach(var itm in Items)
        {
            this.NbInspections++;

            long val2 = itm;

            if(this.OperationSquare)
                val2 = itm * itm;
            else
                val2 = itm * OperationMulti + OperationAdd;

            if (WorryDiviser == 1)
                val2 = val2 % commonDiv;
            else
                val2 = val2 / WorryDiviser;

            if (val2 % this.DivisibleTest == 0)
                monkeys.First(o => o.Number == this.DestinationTrue).Items.Add(val2);
            else
                monkeys.First(o => o.Number == this.DestinationFalse).Items.Add(val2);
        }
        this.Items.Clear();
    }
}