using DesafioComercial.Models;
using DesafioComercial.Services;

namespace DesafioComercial.Tests;

public class JurosServiceTests
{
    private readonly JurosService _service = new();

    [Fact]
    public void ValorVencido_DeveCalcularJurosCorretamente()
    {
        // Arrange
        decimal valor = 1000m;
        DateTime vencimento = DateTime.Today.AddDays(-5);

        // Act
        ResultadoJuros resultado =
            _service.Calcular(valor, vencimento);

        // Assert
        Assert.Equal(5, resultado.DiasAtraso);
        Assert.Equal(0.025m, resultado.TaxaDiaria);
        Assert.Equal(125m, resultado.ValorJuros);
        Assert.Equal(1125m, resultado.ValorTotal);
    }

    [Fact]
    public void VencimentoHoje_NaoDeveGerarJuros()
    {
        // Arrange
        decimal valor = 1000m;
        DateTime vencimento = DateTime.Today;

        // Act
        ResultadoJuros resultado =
            _service.Calcular(valor, vencimento);

        // Assert
        Assert.Equal(0, resultado.DiasAtraso);
        Assert.Equal(0m, resultado.ValorJuros);
        Assert.Equal(1000m, resultado.ValorTotal);
    }

    [Fact]
    public void VencimentoFuturo_NaoDeveGerarJuros()
    {
        // Arrange
        decimal valor = 1000m;
        DateTime vencimento =
            DateTime.Today.AddDays(10);

        // Act
        ResultadoJuros resultado =
            _service.Calcular(valor, vencimento);

        // Assert
        Assert.Equal(0, resultado.DiasAtraso);
        Assert.Equal(0m, resultado.ValorJuros);
        Assert.Equal(1000m, resultado.ValorTotal);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void ValorInvalido_DeveLancarExcecao(
        double valorInformado)
    {
        // Arrange
        decimal valor = (decimal)valorInformado;

        DateTime vencimento =
            DateTime.Today.AddDays(-5);

        // Act + Assert
        Assert.Throws<ArgumentException>(() =>
            _service.Calcular(
                valor,
                vencimento
            )
        );
    }
}