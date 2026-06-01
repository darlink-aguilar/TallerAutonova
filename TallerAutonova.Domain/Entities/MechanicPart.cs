

namespace TallerAutonova.Domain.Entities
{
    public class MechanicPart : AuditBase
    {
        public int MechanicId { get; set; } // FK a Mechanic

        public int PartId { get; set; } // FK a Part



        // Navigation Properties

        public Mechanic Mechanic { get; set; } = null!;

        public Part Part { get; set; } = null!;

    }
}
