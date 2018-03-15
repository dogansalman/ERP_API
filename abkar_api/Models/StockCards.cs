using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using abkar_api.Filters;
using System.Collections.Generic;

namespace abkar_api.Models
{
    public class StockCards
    {
        //Properties
        [Key]
        public int id { get; set; }
        [Required]
        [StringLength(255, ErrorMessage = "Kod en fazla 255 karakter olmalıdır.")]
        [Index("IX_StockCard", 1, IsUnique = true)]
        public string code { get; set; }
        [Required]
        [StringLength(255, ErrorMessage = "Stok tanımı en fazla 255 karakter olmalıdır.")]
        public string name { get; set; }
        [Numeral(ErrorMessage = "Adet sadece rakkamsal değer olmalıdır.")]
        public int unit { get; set; } = 0;
        [Numeral(ErrorMessage = "Adet sadece rakkamsal değer olmalıdır.")]
        public int per_production_unit { get; set; } = 0;
        [Required]
        [StringLength(255, ErrorMessage = "Stok tipi en fazla 255 karakter olmalıdır.")]
        public string stock_type { get; set; }
        public DateTime created_date { get; set; } = DateTime.Now;
        public DateTime? updated_date { get; set; }
        public string photo{ get; set; }
        [NotMapped]
        public StockCardProcessNo process_no { get; set; }
        [NotMapped]
        public ICollection<StockCardProcessNo> stockcard_process_no { get; set; }
        private int _per_unit;
        [NotMapped]
        public int per_unit
        {
            get { return this.per_production_unit > 0 ? this.unit / this.per_production_unit : 0; }
            set { _per_unit = value; }
        }


        public bool deleted { get; set; } = false;
    }

}