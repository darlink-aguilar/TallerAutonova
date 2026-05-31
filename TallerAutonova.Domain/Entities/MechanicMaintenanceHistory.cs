
namespace TallerAutonova.Domain.Entities
{
    public class MechanicMaintenanceHistory
    {

        public int MechanicId { get; set; } // FK a Mechanic
        public int MaintenanceHistoryId { get; set; } // FK a MaintenanceHistory

        // Navigation Properties

        public Mechanic Mechanic { get; set; } = null!;

        public MaintenanceHistory MaintenanceHistory { get; set; } = null!;

        public ICollection<MechanicMaintenanceHistory> MechanicMaintenanceHistories { get; set; } = new List<MechanicMaintenanceHistory>();


    }
}
