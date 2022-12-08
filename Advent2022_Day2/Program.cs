

var lines = File.ReadAllLines("clue.txt");

Part1();
Part2();

Console.ReadKey();

void Part1()
{
    int score = 0;
    foreach (var line in lines)
    {
        RPS ennemy = ConvertPlay(line[0]);
        RPS me = ConvertPlay(line[2]);
        score += (int)me + (int)VictoryCheck(ennemy, me);
    }
    Console.WriteLine("Part1:" + score);
}

void Part2()
{
    int score = 0;
    foreach (var line in lines)
    {
        RPS ennemy = ConvertPlay(line[0]);
        WLD result = ConvertWin(line[2]);
        score += (int)result + (int)PlayCheck(ennemy, result);
    }
    Console.WriteLine("Part2:" + score);
}

WLD VictoryCheck(RPS ennemy, RPS me)
{
    if (ennemy == me)
        return WLD.Draw;
    if ((ennemy == RPS.Rock && me == RPS.Scisor)
        || (ennemy == RPS.Paper && me == RPS.Rock)
        || (ennemy == RPS.Scisor && me == RPS.Paper))
        return WLD.Lose;
    return WLD.Win;
}

RPS PlayCheck(RPS ennemy, WLD result)
{
    switch(result)
    {
        case WLD.Draw:
            return ennemy;
        case WLD.Win:
            switch(ennemy)
            {
                case RPS.Rock: return RPS.Paper;
                case RPS.Paper: return RPS.Scisor;
                default:
                case RPS.Scisor: return RPS.Rock;
            }
        default:
        case WLD.Lose:
            switch (ennemy)
            {
                case RPS.Rock: return RPS.Scisor;
                case RPS.Paper: return RPS.Rock;
                default:
                case RPS.Scisor: return RPS.Paper;
            }
    }
}

RPS ConvertPlay(char c)
{
    switch(c)
    {
        case 'X':
        case 'A': return RPS.Rock;
        case 'Y':
        case 'B': return RPS.Paper;
        default: return RPS.Scisor;
    }
}


WLD ConvertWin(char c)
{
    switch (c)
    {
        case 'X': return WLD.Lose;
        case 'Y': return WLD.Draw;
        default: return WLD.Win;
    }
}

enum RPS { Rock = 1, Paper = 2, Scisor = 3 };
enum WLD { Win = 6, Lose = 0, Draw = 3 };