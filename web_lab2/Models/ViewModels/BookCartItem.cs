using System.Collections.Generic;

namespace web_lab2.Models.ViewModels
{
    public class BookCartItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<Sage> Sages { get; set; }

        public int Quantity { get; set; } = 0;
    }
}