namespace SearchService.BL.DomainModels;

public class Service
{
    public int Id { get; }
    public string Name { get; }
    public Position Position { get; }

    public Service(int id, string name, Position position)
    {
        Id = id;
        Name = name;
        Position = position;
    }
}