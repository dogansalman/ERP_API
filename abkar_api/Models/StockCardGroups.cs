using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace abkar_api.Models
{
    public class StockCardGroups
    {
        [Key]
        public int id { get; set; }
        public int supply_requisitions_id { get; set; }
        public int group_unit { get; set; }
        public int stcokcard_id { get; set; }
        public string barcode { get; set; }

    }
}