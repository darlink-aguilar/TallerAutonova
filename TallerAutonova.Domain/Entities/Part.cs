namespace TallerAutonova.Domain.Entities
{
    public class Part
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int MinStock { get; set; }
        public int AdministratorId { get; set; } // FK

        // Navigation Property
        public Administrator Administrator { get; set; } = null!;
    }
}
