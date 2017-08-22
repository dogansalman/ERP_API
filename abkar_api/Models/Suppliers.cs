using abkar_api.Filters;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace abkar_api.Models
{
    public class Suppliers
    {
        //Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        [StringLength(255, ErrorMessage = "Firma adı en fazla 255 karakter olmalıdır.")]
        public string company { get; set; }
        [Required]
        [StringLength(255, ErrorMessage = "Adres en fazla 255 karakter olmalıdır.")]
        public string adress { get; set; }
        [Required]
        [StringLength(255, ErrorMessage = "şehir en fazla 255 karakter olmalıdır.")]
        public string city { get; set; }
        [Required]
        [StringLength(255, ErrorMessage = "İlçe en fazla 255 karakter olmalıdır.")]
        public string town { get; set; }
        [Numeral(ErrorMessage = "Telefon numarası sadece rakkamsal olmalıdır.")]
        public string phone { get; set; }
        [Required]
        [StringLength(255, ErrorMessage = "İlgili adı en fazla 255 karakter olmalıdır.")]
        public string name { get; set; }
        [Required]
        [StringLength(255, ErrorMessage = "İlgili soyadı en fazla 255 karakter olmalıdır.")]
        public string lastname { get; set; }
        [EmailAddress(ErrorMessage = "Geçersiz e-posta adresi")]
        public string email { get; set; }
        public DateTime? created_date { get; set; } = DateTime.Now;
        public DateTime? updated_date { get; set; }

    }
}