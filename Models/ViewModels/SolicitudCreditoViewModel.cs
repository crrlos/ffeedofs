using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class SolicitudCreditoViewModel
{
    [Required]
    [Range(1, int.MaxValue)]
    [Display(Name="Cliente")]
    public int ClienteId { get; set; }
    [Required]
    [Range(1, double.MaxValue)]
    public double Ingresos { get; set; }
    [Required]
    [Range(1, double.MaxValue)]
    public double Egresos { get; set; }
    [Required]
    [Range(1, double.MaxValue)]
    public double MontoSolicitado { get; set; }
    [Required]
    [Range(1, double.MaxValue)]
    public int Plazo { get; set; }
    [Required]
    [Range(0, double.MaxValue)]
    public double Tasa { get; set; }
    [Required]
    [Range(0.000000000001, int.MaxValue)]
    [Display(Name="Destino")]
    public int DestinoId { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    [Display(Name="Tipo de crédito")]
    public int TipoCreditoId { get; set; }

    public List<ClienteDTO> Clientes {get;set;}
    public List<DestinoDTO> Destinos {get;set;}
    public List<TipoCreditoDTO> TiposCreditos {get;set;}


}