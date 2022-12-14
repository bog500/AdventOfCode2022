using System.Linq;

Console.WriteLine("Advent of Code 2022 - Day 13");
Console.WriteLine("\n-------------------");



var pairs = Setup();

Part1(pairs);

Part2(pairs);

void Part1(List<ElementPair> pairs)
{
    List<int> indices = new();
    int pairNb = 1;
    foreach (var p in pairs)
    {
        if (Comparer.Compare(p.Left, p.Right) == true)
            indices.Add(pairNb);
        pairNb++;
    }

    int nbRightOrder = indices.Sum();

    Console.WriteLine($"Part 1: {nbRightOrder}");
}

void Part2(List<ElementPair> pairs)
{
    Element six = Parser.Parse("6");
    Element two = Parser.Parse("2");

    List<Element> elements = pairs.Select(o => o.Left).Union(pairs.Select(o => o.Right)).ToList();
    elements.AddRange(new []{ six, two });

    elements.Sort();

    int pos6 = elements.IndexOf(six) + 1;
    int pos2 = elements.IndexOf(two) + 1;

    Console.WriteLine($"Part 2: {pos6* pos2}");
}

List<ElementPair> Setup()
{
    List<ElementPair> pairs = new();

    var lines = File.ReadAllLines("clue.txt");

    int nbLines = 0;

    Element left = null;
    Element right = null;
    foreach (var line in lines)
    {
        if (nbLines % 3 == 0)
            left = Parser.Parse(line);
        else if (nbLines % 3 == 1)
            right = Parser.Parse(line);
        else
            pairs.Add(new ElementPair(left, right));

        nbLines++;
    }
    pairs.Add(new ElementPair(left, right));

    return pairs;
}

class ElementPair
{
    public Element Left;
    public Element Right;

    public ElementPair(Element left, Element right)
    {
        this.Left = left;
        this.Right = right;
    }
}

class Parser
{
    public static Element Parse(string s)
    {
        Element baseElement = new Element(null);

        Element currentE = baseElement;

        s = s.Replace("10", "a");

        foreach(char c in s)
        {
            if(c == '[')
            {
                Element e = new Element(currentE);
                currentE.Sub.Add(e);
                currentE = e;
            }
            else if(c == ']')
            {
                currentE = currentE.Parent;
            }
            else if (c == ',') 
            {
                //ignore
            }
            else
            {
                int i = c switch
                {
                    'a' => 10,
                    _ => c - '0'
                };
                currentE.Sub.Add(new Element(i, currentE));
            }
        }

        return baseElement;
    }
}

class Element : IComparable
{
    public Element? Parent;

    public List<Element> Sub = new();

    public int Value;

    public Element(int i, Element? parent)
    {
        this.Value = i;
    }

    public Element(Element parent)
    {
        this.Parent = parent;
        this.Value = -1;
    }

    public override string ToString()
    {
        if (this.Value > -1)
            return this.Value.ToString();

        string s = "[";
        foreach(Element e in this.Sub)
        {
            s += e.ToString();
            if(this.Sub.LastOrDefault() != e)
            {
                s += ",";
            }
        }
        s += "]";
        return s;
    }

    public int CompareTo(object? obj)
    {
        if (obj == null || obj.GetType() != typeof(Element))
            return -1;

        return Comparer.Compare(this, (Element)obj) switch
        {
            true => -1,
            false => 1,
            _ => 0
        };
    }
}

class Comparer
{
    public static bool? Compare(Element left, Element right)
    {
        Convert(left, right);

        int max = Math.Min(left.Sub.Count, right.Sub.Count);

        for(int i = 0; i < max; i++)
        {
            if(Compare(left.Sub[i], right.Sub[i]) == true)
                return true;

            if (Compare(left.Sub[i], right.Sub[i]) == false)
                return false;
        }

        if(left.Sub.Count != right.Sub.Count)
        {
            return left.Sub.Count < right.Sub.Count;
        }

        if (left.Value < right.Value)
            return true;

        if (left.Value > right.Value)
            return false;

        return null;
    }

    static void Convert(Element e1, Element e2)
    {
        if(e1.Value > -1 && e2.Value == -1)
        {
            // e1 is INT and e2 is list
            e1.Sub.Add(new Element(e1.Value, e1));
            e1.Value = -1;
        }else if (e1.Value == -1 && e2.Value > -1)
        {
            // e1 is INT and e2 is list
            e2.Sub.Add(new Element(e2.Value, e2));
            e2.Value = -1;
        }
    }
}