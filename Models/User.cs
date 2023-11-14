namespace Admin_Panel.Models
{
    public partial class User
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int RoleId { get; set; }

        public string Password { get; set; } = null!;

        public string Login { get; set; } = null!;

        public string? Email { get; set; }

        public string? Surname { get; set; }

        public string? PhoneNumber { get; set; }

        public virtual Role Role { get; set; } = null!;
    }
}
