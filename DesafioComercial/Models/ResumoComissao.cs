namespace DesafioComercial.Models;

public class ResumoComissao
{
    public string Vendedor { get; set; } = string.Empty;
    public decimal TotalVendas { get; set; }
    public decimal TotalComissao { get; set; }
}