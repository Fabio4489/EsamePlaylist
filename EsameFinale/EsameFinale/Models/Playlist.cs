using System.ComponentModel.DataAnnotations;

namespace EsameFinale.Models
{
    public class Playlist
    {
        [Key]
        public int IdPlaylist { get; set; }
        public string? Titolo { get; set; }
        public string Autore { get; set; } = "Utente";

        public List<Canzone>? Elenco { get; set; } = new();
    }

    public class Canzone
    {
        [Key]
        public int IdCanzone { get; set; }
        public int Durata { get; set; }
        public string Titolo { get; set; } = null!;
        public string NomeAutore { get; set; } = "Non riconosciuto";
        public string CognomeAutore { get; set; } = "";
        public int? PlaylistId { get; set; }
    }
}
