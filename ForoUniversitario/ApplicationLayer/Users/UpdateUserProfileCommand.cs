namespace ForoUniversitario.ApplicationLayer.Users;

public class UpdateUserProfileCommand
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<string> Interests { get; set; } = new();
}
