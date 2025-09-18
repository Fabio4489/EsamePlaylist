namespace EsameFinale.Models
{
    public class Canzone
    {
        public int IdCanzone { get; set; }
        public int Durata { get; set; }
        public string Titolo { get; set; } = null!;
        public string NomeAutore { get; set; } = "Non riconosciuto";
        public string CognomeAutore { get; set; } = "Non riconosciuto";
    }
}
