using System.Collections.Generic;
using web_lab2.Abstractions;

namespace web_lab2.Models
{
    public class Book: IEntity<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<Sage> Sages { get; set; } = new();

        public List<Order> Orders { get; set; } = new();

        public List<OrdersBooks> OrdersDetails { get; set; } = new();
    }
}