using System.IO;
using System.Numerics;

Part2();

void Part1()
{
    Util.Parse();

    Donkey root = Util.donkeys["root"];

    string part1 = root.GetResult().ToString();

    Console.WriteLine("Part 1:" + part1);
}

void Part2()
{

    Util.Parse();

    Donkey root = Util.donkeys["root"];
    Donkey human = Util.donkeys["humn"];

    root.Operator = OperatorEnum.eq;
    human.Operator = OperatorEnum.human;



    string formula = root.GetFormula().Replace(" ","");


    Console.WriteLine("Part 2 (put in WolframAlpha):");
    Console.WriteLine(formula);


}








static class Util
{
    public static Dictionary<string, Donkey> donkeys = new();

    public static void Parse()
    {
        donkeys = new();
        var lines = File.ReadAllLines("clue.txt");
        foreach(var line in lines)
        {
            Donkey d = new(line[..4]);
            if(line.Length == 17)
            {
                d.Left = line[6..10];
                d.Right = line[13..17];
                d.Operator = line[11] switch
                {
                    '+' => OperatorEnum.add,
                    '-' => OperatorEnum.minus,
                    '*' => OperatorEnum.multi,
                    '/' => OperatorEnum.div,
                    _ => OperatorEnum.val
                };
            }
            else
            {
                d.Val = int.Parse(line[6..]);
                d.Operator = OperatorEnum.val;
            }
            donkeys.Add(d.Code, d);
        }
    }

}

class Donkey
{
    public readonly string Code;

    private string formula;

    private BigInteger? _val;
    public BigInteger Val;
    public string Left;
    public string Right;
    public OperatorEnum Operator;

    public Donkey(string code)
    {
        this.Code = code;
    }


    public string GetFormula()
    {
        if (this.formula != null)
            return this.formula;

        if (Operator == OperatorEnum.val)
            return Val.ToString();

        if (Operator == OperatorEnum.human)
            return "humn";

        string leftVal = LeftFormula();
        string rightVal = RightFormula();


        if (!leftVal.Contains("humn") && !rightVal.Contains("humn"))
            return GetResult().ToString();


        if (!leftVal.Contains("humn"))
        {
            leftVal = LeftValue().ToString();
        }

        if (!rightVal.Contains("humn"))
        {
            rightVal = RightValue().ToString();
        }

        this.formula = Operator switch
        {
            OperatorEnum.add => $"({leftVal} + {rightVal})",
            OperatorEnum.minus => $"({leftVal} - {rightVal})",
            OperatorEnum.multi => $"({leftVal} * {rightVal})",
            OperatorEnum.div => $"({leftVal} / {rightVal})",
            OperatorEnum.eq => $"{leftVal} = {rightVal}",
            OperatorEnum.human => "humn",
            _ => Val.ToString()
        };


        return this.formula;
    }

    public BigInteger LeftValue()
    {
        return Util.donkeys[Left].GetResult();
    }

    public BigInteger RightValue()
    {
        return Util.donkeys[Right].GetResult();
    }

    public string LeftFormula()
    {
        return Util.donkeys[Left].GetFormula();
    }

    public string RightFormula()
    {
        return Util.donkeys[Right].GetFormula();
    }

    public BigInteger GetResult()
    {
        if (_val.HasValue)
            return _val.Value;



        if (Operator == OperatorEnum.val)
            return Val;

        BigInteger leftVal = LeftValue();
        BigInteger rightVal = RightValue();

        _val = Operator switch
        {
            OperatorEnum.add => leftVal + rightVal,
            OperatorEnum.minus => leftVal - rightVal,
            OperatorEnum.multi => leftVal * rightVal,
            OperatorEnum.div => leftVal / rightVal,
            OperatorEnum.eq => leftVal - rightVal,
            _ => Val
        };

        return _val.Value;
    }

}

enum OperatorEnum { val, add, minus, div, multi, eq, human }