using Microsoft.AspNetCore.Http;

namespace web_lab2.Models.ViewModels
{
    public class SageViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public IFormFile Photo { get; set; }

        public string City { get; set; }
    }
}