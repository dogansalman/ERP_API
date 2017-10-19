using abkar_api.Models;

namespace abkar_api.ModelViews
{
    public class _ProductionPersonnelOperation
    {
        public Machines machine { get; set; }
        public Operations operation { get; set; }
        public int operation_time { get; set; }
    }
}