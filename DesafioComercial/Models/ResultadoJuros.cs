namespace DesafioComercial.Models;

public class ResultadoJuros
{
    public decimal ValorOriginal { get; set; }

    public DateTime DataVencimento { get; set; }

    public DateTime DataCalculo { get; set; }

    public int DiasAtraso { get; set; }

    public decimal TaxaDiaria { get; set; }

    public decimal ValorJuros { get; set; }

    public decimal ValorTotal { get; set; }
}