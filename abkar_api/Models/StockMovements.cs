using System.ComponentModel.DataAnnotations;
using abkar_api.Filters;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace abkar_api.Models
{
    public class StockMovements
    {
        //Properties
        [Key]
        public int id { get; set; }
        public int stockcard_id { get; set; }
        [StringLength(255, ErrorMessage = "irsaliye no en fazla 255 karakter olmalıdır.")]
        public string waybill { get; set; }
        [StringLength(255, ErrorMessage = "Tedarikçi en fazla 255 karakter olmalıdır.")]
        public string supplier { get; set; }
        [StringLength(1000, ErrorMessage = "Adres en fazla 1000 karakter olmalıdır.")]
        public string not { get; set; }
        [Required]
        [Numeral(ErrorMessage = "Sadece rakkamsal değer olmalıdır.")]
        public int unit { get; set; } = 0;
        public int? production_id { get; set; } = null;
        [Required]
        public bool? incoming_stock { get; set; } = true;
        public bool? is_junk { get; set; } = false;
        [NotMapped]
        public int junk { get; set; } = 0;
        public bool on_requisition { get; set; } = false;
        public DateTime created_date { get; set; } = DateTime.Now;
        public DateTime? updated_date { get; set; }

    }

}