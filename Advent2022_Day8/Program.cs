using System.ComponentModel.Design;

Console.WriteLine("Advent of Code 2022 - Day 8");
Console.WriteLine("\n-------------------");


var lines = File.ReadAllLines("clue.txt");


var trees = Setup();

int maxPosX = trees.Keys.Max(o => o.X);
int maxPosY = trees.Keys.Max(o => o.Y);

Console.WriteLine("Part1:" + GetTotalTreeVisible());
Console.WriteLine("Part2:" + GetMaxScenicScore());

Console.ReadKey();

Dictionary<Position, int> Setup()
{
    Dictionary<Position, int> trees = new();
    int posY = 0;
    int posX = 0;

    foreach (string line in lines)
    {
        foreach (char c in line)
        {
            int height = c - '0';
            trees.Add(new Position(posX, posY), height);
            posX++;
        }
        posX = 0;
        posY++;
    }
    return trees;
}

int GetTotalTreeVisible()
{
    int nbvisibleTrees = 0;
    foreach (var tree in trees)
    {
        if (IsVisible(tree.Value, tree.Key.X, tree.Key.Y))
        {
            nbvisibleTrees++;
        }
    }
    return nbvisibleTrees;
}
int GetMaxScenicScore()
{
    int maxScore = 0;
    foreach (var t in trees)
    {
        int localScore = GetScenicScore(t.Key);
        if (localScore > maxScore)
        {
            maxScore = localScore;
        }
    }
    return maxScore;
}

int GetScenicScore(Position pos)
{
    int score1 = 0;
    int score2 = 0;
    int score3 = 0;
    int score4 = 0;

    if (IsEdge(pos)) return 0;

    int treeHeight = trees[pos];


    for (int x = pos.X - 1; x >= 0; x--)
    {
        score1++;
        if (trees[new Position(x, pos.Y)] >= treeHeight)
        {
            break;
        }
    }

    for (int x = pos.X + 1; x <= maxPosX; x++)
    {
        score2++;
        if (trees[new Position(x, pos.Y)] >= treeHeight)
        {
            break;
        }
    }

    for (int y = pos.Y - 1; y >= 0; y--)
    {
        score3++;
        if (trees[new Position(pos.X, y)] >= treeHeight)
        {
            break;
        }
    }

    for (int y = pos.Y + 1; y <= maxPosY; y++)
    {
        score4++;
        if (trees[new Position(pos.X, y)] >= treeHeight)
        {
            break;
        }
    }

    return score1 * score2 * score3 * score4;
}

bool IsEdge(Position pos)
{
    if (pos.X == 0 || pos.Y == 0)
    {
        return true;
    }
    if (pos.X == maxPosX || pos.Y == maxPosY)
    {
        return true;
    }
    return false;
}

bool IsVisible(int height, int posX, int posY)
{
    if (trees.Where(o => o.Key.X < posX && o.Key.Y == posY).All(o => o.Value < height))
    {
        return true;
    }

    if (trees.Where(o => o.Key.X > posX && o.Key.Y == posY).All(o => o.Value < height))
    {
        return true;
    }

    if (trees.Where(o => o.Key.X == posX && o.Key.Y < posY).All(o => o.Value < height))
    {
        return true;
    }

    if (trees.Where(o => o.Key.X == posX && o.Key.Y > posY).All(o => o.Value < height))
    {
        return true;
    }

    return false;
}



class Position
{
    public int X;
    public int Y;

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
}