namespace TallerAutonova.Domain.Entities
{
    public class Mechanic : User
    {
        // Navigation Property
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
