using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace abkar_api.ModelViews
{
    public class _QualityFiles
    {
        [Required]
        [StringLength(500)]
        public string title { get; set; }
        public DateTime date { get; set; }
    }
}