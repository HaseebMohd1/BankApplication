using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Currency
    {
        [Key]
        public string? CurrencyCode { get; set; }

        public string Country { get; set; }

        public int ConversionValue { get; set; }
    }

}
