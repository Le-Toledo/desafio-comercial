using DesafioComercial.Services;

namespace DesafioComercial.Tests;

public class ComissaoServiceTests
{
    private readonly ComissaoService _service = new();

    [Theory]
    [InlineData(99.99, 0)]
    [InlineData(100, 1)]
    [InlineData(499.99, 4.9999)]
    [InlineData(500, 25)]
    public void CalcularComissao_DeveAplicarPercentualCorreto(
        double valorVenda,
        double comissaoEsperada)
    {
        // Arrange
        decimal valor = (decimal)valorVenda;
        decimal esperado = (decimal)comissaoEsperada;

        // Act
        decimal resultado =
            _service.CalcularComissao(valor);

        // Assert
        Assert.Equal(esperado, resultado);
    }
}