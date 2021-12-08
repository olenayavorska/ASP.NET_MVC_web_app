using System.Collections.Generic;
using web_lab2.Abstractions;

namespace web_lab2.Models
{
    public class Sage : IEntity<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public byte[] Photo { get; set; }

        public string City { get; set; }

        public List<Book> Books { get; set; } = new();
    }
}