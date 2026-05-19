namespace TodoMVC.Models;

public class PeliculasySeries
{
    public int Id { get; set; }
    public string? Titulo { get; set; }
    public string? Genero { get; set; }
    public DateTime Estreno { get; set; }
    public bool Completada {get; set; }

}
