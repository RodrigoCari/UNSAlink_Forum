namespace ForoUniversitario.ApplicationLayer.Groups;

public class CreateGroupCommand
{
    public Guid AdminId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
