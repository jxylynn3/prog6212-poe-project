namespace ST10448420_CMCsystem.Models
{
    public class HR
    {
        public string HRID { get; set; } = Guid.NewGuid().ToString().Substring(0,10);//helps generate unique ID for HR super user
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
