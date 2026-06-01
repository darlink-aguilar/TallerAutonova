namespace TallerAutonova.API.DTOs.Request
{
    public class PartRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int MinStock { get; set; }
        public int AdministratorId { get; set; }
    }
}