using abkar_api.Filters;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace abkar_api.ModelViews
{
    public class _ComPassword
    {
        [Required]
        [StringLength(255)]
        public string password { get; set; }
        [Required]
        [StringLength(255)]
        public string newPassword { get; set; }
        [Required]
        [StringLength(255)]
        public string reply { get; set; }
    }
    public class _ComCustomers
    {        

        [Required]
        [StringLength(255)]
        [Index("IX_Company", 1, IsUnique = true)]
        public string company { get; set; }
        [Required]
        [StringLength(255, ErrorMessage = "Adres en fazla 255 karakter olmalıdır.")]
        public string adress { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Geçersiz e-posta adresi")]
        public string email { get; set; }
        [Numeral(ErrorMessage = "Telefon numarası sadece rakkamsal olmalıdır.")]
        public string phone { get; set; }
        [Required]
        [StringLength(255)]
        public string name { get; set; }
        [Required]
        [StringLength(255)]
        public string lastname { get; set; }
        public DateTime? created_date { get; set; } = DateTime.Now;
        public DateTime? updated_date { get; set; }
    }
}