namespace ChushkaWebApplication.Models
{
    using Enums;
    using System.Collections.Generic;

    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public Role Role { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
