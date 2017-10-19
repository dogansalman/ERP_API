using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using abkar_api.Models;
using abkar_api.ModelViews;

namespace abkar_api.ModelViews
{
    public class _ProductionPersonnelsWithOperation
    {
        public ICollection<_ProductionPersonnelOperation> operations { get; set; }
        public Personnel personnel { get; set; }
    }
}