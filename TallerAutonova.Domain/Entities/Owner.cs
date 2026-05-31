namespace TallerAutonova.Domain.Entities
{
    public class Owner
    {
        public int Id { get; set; }  
        public string  Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        // Navigation Property
        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

    }
}