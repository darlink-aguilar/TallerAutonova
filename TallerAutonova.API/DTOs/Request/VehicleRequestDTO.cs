namespace TallerAutonova.API.DTOs.Request
{
    public class VehicleRequestDTO
    {
        public int Id { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Plate { get; set; } = string.Empty;
        public string Speed { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool State { get; set; }
        public int Year { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public string OwnerPhone { get; set; } = string.Empty;
    }
}
