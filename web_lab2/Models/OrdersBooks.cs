namespace web_lab2.Models
{
    public class OrdersBooks
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }

        public int Number { get; set; }
    }
}