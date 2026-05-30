namespace TallerAutonova.Domain.Entities
{
    internal class Repuesto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public int Stock_min{ get; set; } // StockMin

        // Navigation Property
    }
}
