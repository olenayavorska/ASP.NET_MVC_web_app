using System.Collections.Generic;
using web_lab2.Abstractions;

namespace web_lab2.Models
{
    public class Order: IEntity<int>
    {
        public int Id { get; set; }
        public List<Book> Books { get; set; } = new();

        public List<OrdersBooks> OrdersDetails { get; set; } = new();
        
        public int CustomerId { get; set; }
        public User Customer { get; set; }
    }
}