public class User
{
    public User()
    {
    
        Roles = new List<string>();
    }

    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Salt { get; set; }
    public IList<string> Roles { get; set; }
}
