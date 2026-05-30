namespace TallerAutonova.Domain.Entities
{
    public class Part
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public int StockMin{ get; set; } 

        // Navigation Property
    }
}
