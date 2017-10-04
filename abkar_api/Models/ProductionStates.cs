using System;
using System.ComponentModel.DataAnnotations;


namespace abkar_api.Models
{
    public class ProductionStates
    {
        //Properties
        [Key]
        public int id { get; set; }
        [StringLength(255)]
        public string name { get; set; }
        public DateTime created_date { get; set; }
        public DateTime? updated_date { get; set; }

        
        /*
         * Production State update ile birlikte isme göre tüm production daki ismen eşleşen kayıtların state değeride update edilecek.
         *  */

    }
}