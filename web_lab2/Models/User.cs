using System.Collections.Generic;
using web_lab2.Abstractions;

namespace web_lab2.Models
{
    public class User : IEntity<int>
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public List<Role> Roles { get; set; } = new();

        public List<Order> Orders { get; set; } = new();
    }
}