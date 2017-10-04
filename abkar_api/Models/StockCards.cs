using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using abkar_api.Filters;

namespace abkar_api.Models
{
    public class StockCards
    {
        //Properties
        [Key]
        public int id { get; set; }
        [Required]
        [StringLength(255, ErrorMessage = "Kod en fazla 255 karakter olmalıdır.")]
        public string code { get; set; }
        [Required]
        [StringLength(255, ErrorMessage = "Stok tanımı en fazla 255 karakter olmalıdır.")]
        public string name { get; set; }
        [Numeral(ErrorMessage = "Adet sadece rakkamsal değer olmalıdır.")]
        public int unit { get; set; } = 0;
        [Required]
        [StringLength(255, ErrorMessage = "Stok tipi en fazla 255 karakter olmalıdır.")]
        public string stock_type { get; set; }
        public DateTime created_date { get; set; } = DateTime.Now;
        public DateTime? updated_date { get; set; }
    }

}