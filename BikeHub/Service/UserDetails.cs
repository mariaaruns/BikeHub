using System.ComponentModel.DataAnnotations;

namespace BikeHub.Service
{
    public class UserDetails
    {
        public int Id { get; set; }
        public string Email { get; set; }

        public string Name { get; set; }
        public string Role { get; set; }
    }
}