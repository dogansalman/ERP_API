using abkar_api.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace abkar_api.Models
{
    public class Customers
    {

        //Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public string company { get; set; }
        [Required]
        [StringLength(255, ErrorMessage = "Adres en fazla 255 karakter olmalıdır.")]
        public string adress { get; set; }
        [Required]
        [StringLength(90, ErrorMessage = "İl en fazla 90 karakter olmalıdır.")]
        public string city { get; set; }
        [Required]
        [StringLength(90, ErrorMessage = "İlçe en fazla 90 karakter olmalıdır.")]
        public string town { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Geçersiz e-posta adresi")]
        public string email { get; set; }
        [PhoneMask("9999999999", ErrorMessage = "Telefon numarası {1}. formatında olmalıdır.")]
        public string phone { get; set; }
        [Required]
        [StringLength(255)]
        public string name { get; set; }
        [Required]
        [StringLength(255)]
        public string lastname { get; set; }
        [Required]
        [StringLength(255,ErrorMessage = "soyad en fazla 255 karakter olmalıdır.")]
        public string password { get; set; }
        [Required]
        [Range(0,1,ErrorMessage = "Durum bilgisi hatalı")]
        public Boolean state { get; set; }
        public DateTime? created_date { get; set; } = DateTime.Now;
        public DateTime? updated_date { get; set; }

        private List<Customers> customers = new List<Customers>();
        
     
    }
}