namespace ShooperAPI.Models
{
    public class User
    {
        public int Id { get; set; } // Clé primaire
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "client";
    }


}
