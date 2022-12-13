var lines = File.ReadAllLines("clue.txt");

char startChar = 'a';
char endChar = 'z';

Dictionary<Position, CellValue> cells = new();

Position startPos = null;
Position endPos = null;

InitPositions();

InitConnectedCells();

// start recursion
cells[endPos].SetPathValue(0);

// used for coloring
cells[startPos].SetInPath();

int minA = cells.Where(o => o.Value.Height == 'a').Min(o => o.Value.PathValue);

Print(minA);

Console.WriteLine($"Part1 - shortest: {cells[startPos].PathValue}");

Console.WriteLine($"Part2 - shortest from any A: {minA}");



void InitPositions()
{
    int x = 0;
    int y = 0;
    foreach (var line in lines)
    {
        foreach (char c in line)
        {
            Position p = new Position(x, y);
            char height = GetHeight(c);
            cells.Add(p, new CellValue(height));

            if (c == 'E')
                endPos = p;
            else if (c == 'S')
                startPos = p;

            x++;
        }
        x = 0;
        y++;
    }
}

void InitConnectedCells()
{
    foreach(var p in cells)
    {
        p.Value.CellsTo = GetConnectedTo(p.Key);
        p.Value.CellsFrom = GetConnectedFrom(p.Key);
    }
}

List<CellValue> GetConnectedTo(Position cell)
{
    List<CellValue> connected = new();

    var voisins = GetVoisins(cell);

    foreach (var voisin in voisins)
    {
        if (CanVisit(cell, voisin))
            connected.Add(cells[voisin]);
    }

    return connected;
}

List<CellValue> GetConnectedFrom(Position cell)
{
    List<CellValue> connected = new();

    var voisins = GetVoisins(cell);

    foreach (var voisin in voisins)
    {
        if (CanVisit(voisin, cell)) // reversed
            connected.Add(cells[voisin]);
    }

    return connected;
}

List<Position> GetVoisins(Position cell)
{
    List<Position> voisins = new();

    voisins.Add(new Position(cell.X + 1, cell.Y));
    voisins.Add(new Position(cell.X - 1, cell.Y));
    voisins.Add(new Position(cell.X, cell.Y + 1));
    voisins.Add(new Position(cell.X, cell.Y - 1));

    return voisins;
}


bool CanVisit(Position from, Position to)
{
    if (cells.ContainsKey(from) == false)
        return false;

    if (cells.ContainsKey(to) == false)
        return false;

    if (to == from)
        return false;

    if (Math.Abs(to.X - from.X) > 1)
        return false;

    if (Math.Abs(to.Y - from.Y) > 1)
        return false;

    if (cells[to].Height < 'a' || cells[to].Height > 'z')
        return false;

    bool ok = cells[to].Height <= cells[from].Height + 1;

    return ok;

}



char GetHeight(char c)
{
    return c switch
    {
        'S' => GetHeight(startChar),
        'E' => GetHeight(endChar),
        _ => c
    };
}


void Print(int shortestA)
{
    Console.Clear();
    int currentY = 0;
    foreach (var c in cells)
    {
        if (currentY != c.Key.Y)
        {
            currentY = c.Key.Y;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write('\n');
        }

        if (startPos == c.Key)
            Console.BackgroundColor = ConsoleColor.Red;
        else if (endPos == c.Key)
            Console.BackgroundColor = ConsoleColor.Red;
        else if (c.Value.IsInPath)
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
        else if(c.Value.Height == 'a' && c.Value.PathValue == shortestA)
            Console.BackgroundColor = ConsoleColor.DarkBlue;

        Console.Write(c.Value.Height);
        Console.BackgroundColor = ConsoleColor.Black;
    }
    Console.WriteLine("");
}

class CellValue
{
    public int PathValue { get; private set; }
    public char Height { get; private set; }

    public bool IsInPath { get; set; }

    public List<CellValue> CellsTo = new();
    public List<CellValue> CellsFrom = new();

    public CellValue(char h)
    { 
        this.Height = h;
        this.PathValue = int.MaxValue;
    }

    // used for coloring
    public void SetInPath()
    {
        this.IsInPath = true;
        foreach (var p in this.CellsTo)
        {
            if (p.PathValue == this.PathValue - 1)
            {
                p.SetInPath();
                return;
            }
        }
    }

    public void SetPathValue(int v)
    {
        this.PathValue = v;

        foreach (var p in this.CellsFrom)
        {
            if (p.PathValue > v + 1)
                p.SetPathValue(v + 1);
        }
    }
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

    private int _hashcode;

    public override int GetHashCode()
    {
        if (_hashcode == 0)
            _hashcode = (this.X + ";" + this.Y).GetHashCode();

        return _hashcode;
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

    public static bool operator ==(Position p1, Position p2) => p1.Equals(p2);

    public static bool operator !=(Position p1, Position p2) => !(p1 == p2);
}
