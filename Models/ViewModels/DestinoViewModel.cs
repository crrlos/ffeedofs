using System.ComponentModel.DataAnnotations;

public class DestinoViewModel
{
    public int Id { get; set; }
    [Required]
    [MaxLength(10)]
    public string Codigo { get; set; }
    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; }
    
}