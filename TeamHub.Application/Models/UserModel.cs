namespace TeamHub.Application.Models;

/// <summary>
/// Model for updating user profile details.
/// </summary>
public class UserModel
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string VirtualPath { get; set; }
    public List<string> Roles { get; set; }
}
