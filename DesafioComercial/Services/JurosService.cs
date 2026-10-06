using DesafioComercial.Models;

namespace DesafioComercial.Services;

public class JurosService
{
    private const decimal TaxaDiaria = 0.025m;

    public ResultadoJuros Calcular(
        decimal valor,
        DateTime dataVencimento)
    {
        if (valor <= 0)
        {
            throw new ArgumentException(
                "O valor deve ser maior que zero."
            );
        }

        DateTime dataAtual = DateTime.Today;

        int diasAtraso =
            (dataAtual - dataVencimento.Date).Days;

        if (diasAtraso < 0)
        {
            diasAtraso = 0;
        }

        decimal valorJuros =
            valor * TaxaDiaria * diasAtraso;

        decimal valorTotal =
            valor + valorJuros;

        return new ResultadoJuros
        {
            ValorOriginal = valor,
            DataVencimento = dataVencimento.Date,
            DataCalculo = dataAtual,
            DiasAtraso = diasAtraso,
            TaxaDiaria = TaxaDiaria,
            ValorJuros = valorJuros,
            ValorTotal = valorTotal
        };
    }
}