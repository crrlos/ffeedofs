using System.ComponentModel.DataAnnotations;

public class ClienteViewModel
{
    public int Id { get; set; }
    [Required]
    [MaxLength(10)]
    [RegularExpression(@"\d{8}-\d{1}")]
    [Display(Name = "Número de DUI")]
    public string Codigo { get; set; }
    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; }
    [Required]
    [MaxLength(100)]
    public string Apellidos { get; set; }
}