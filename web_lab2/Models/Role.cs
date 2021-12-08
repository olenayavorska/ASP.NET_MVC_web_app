using System.Collections.Generic;
using web_lab2.Abstractions;

namespace web_lab2.Models
{
    public class Role : IEntity<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<User> Users { get; set; } = new();
    }
}