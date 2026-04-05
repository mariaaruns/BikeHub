namespace Bikehub.Hybrid.Authhandler
{
    public class UserSession
    {
        public HashSet<string> Roles { get; set; } = new();
        public HashSet<string>? Policies { get; set; } = new();
        public string Token { get; set; }

        public string Username { get; set; }
        public string? UserId { get; internal set; }

        public void Clear()
        {
            Roles.Clear();
            Policies.Clear();
            Username = null;
            Token = null;
        }
    }
}