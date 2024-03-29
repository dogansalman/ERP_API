﻿using System;
using abkar_api.Filters;
using System.ComponentModel.DataAnnotations;

namespace abkar_api.Models
{
    public class SupplyRequisitions
    {
        //Properties
        [Key]
        public int id { get; set; }
        [Required]
        [Numeral(ErrorMessage = "id sadece rakkamsal değer olmalıdır.")]
        public int stockcard_id { get; set; }
        [Required]
        [StringLength(90, ErrorMessage = "Tedarikçi en fazla 90 karakter olabilir")]
        public string supplier { get; set; }
        [StringLength(1000, ErrorMessage = "Mesaj en fazla 1000 karakter olabilir")]
        public string message { get; set; }
        [Numeral(ErrorMessage = "Adet sadece rakkamsal değer olmalıdır.")]
        public int unit { get; set; }
        [Numeral()]
        public int real_unit { get; set; } = 0;
        [Numeral()]
        public int state { get; set; } = 0;
        public string waybill { get; set; }
        [Numeral()]
        public Boolean notify { get; set; } = false;
        [Required]
        public DateTime delivery_date { get; set; }
        [Required]
        public int supplier_id { get; set; }
        public DateTime created_date { get; set; } = DateTime.Now;
        public DateTime? updated_date { get; set; }


    }
}