Console.WriteLine("Advent of Code 2022 - Day 7");
Console.WriteLine("\n-------------------");


var lines = File.ReadAllLines("clue.txt");

Dir currentDir = new Dir()
{
    Path = "/"
};

List<Dir> directories = new List<Dir>() { currentDir };


foreach (string line in lines)
{
    if(line.StartsWith("$"))
    {
        //cmd
        if(line.StartsWith("$ cd"))
        {
            string newFolder = line.Split(' ').Last();
            if(newFolder == "/")
            {
                currentDir = directories.First(o => o.Path == "/");
            }
            else if (newFolder == "..")
            {
                string newPath = "/" + string.Join('/', currentDir.Path.Split('/')[..^1]);
                newPath = newPath.Replace("//", "/");
                currentDir = directories.First(o => o.Path == newPath);
            }
            else
            {
                string newPath = Path.Combine(currentDir.Path, newFolder).Replace("\\", "/");
                currentDir = new Dir() { Path = newPath };
                directories.Add(currentDir);
            }
        }
    }
    else if(!line.StartsWith("dir"))
    {
        //file
        int size = int.Parse(line.Split(' ').First());

        foreach (var d in directories.Where(o => currentDir.Path.StartsWith(o.Path)))
        {
            d.Size += size;
        }
    }
}

int totalUsedSize = 0;

foreach(var d in directories.Where(o => o.Size < 100000))
{
    totalUsedSize += d.Size;
}

Console.WriteLine("Part1:" + totalUsedSize);

//tota disk size = 70000000
int available = 70000000 - directories.First(o => o.Path == "/").Size;

//required free space = 30000000
int smallest = directories.Where(o => o.Size >= (30000000 - available)).Min(o => o.Size);

Console.WriteLine("Part2:" + smallest);

Console.ReadKey();


class Dir
{
    public string Path { get; set; }
    public int Size { get; set; }
}