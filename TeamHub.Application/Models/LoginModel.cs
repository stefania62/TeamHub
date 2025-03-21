namespace TeamHub.API.Models
{
    /// <summary>
    /// Model for user login request.
    /// </summary>
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}