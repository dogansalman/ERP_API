using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using abkar_api.Filters;
using System;

namespace abkar_api.Models
{
    public class StockMovements
    {
        //Properties
        [Key]
        public int id { get; set; }
        public int stockcard_id { get; set; }
        [Required]
        [StringLength(255, ErrorMessage = "irsaliye no en fazla 255 karakter olmalıdır.")]
        public string waybill { get; set; }
        [StringLength(255, ErrorMessage = "Tedarikçi en fazla 255 karakter olmalıdır.")]
        [Required]
        public string supplier { get; set; }
        [StringLength(1000, ErrorMessage = "Adres en fazla 1000 karakter olmalıdır.")]
        public string not { get; set; }
        [Required]
        [Numeral(ErrorMessage = "Sadece rakkamsal değer olmalıdır.")]
        public int unit { get; set; } = 0;
        [Required]
        public bool? movement_type { get; set; } = true;
        public DateTime created_date { get; set; } = DateTime.Now;
        public DateTime? updated_date { get; set; }

    }

}