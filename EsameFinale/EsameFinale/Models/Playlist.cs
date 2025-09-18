namespace EsameFinale.Models
{
    public class Playlist
    {
        public int IdPlaylist { get; set; }
        public string? Titolo { get; set; }
        public string Autore { get; set; } = "Utente";
        public List<Canzone>? Elenco { get; set; }
        
    }
}
