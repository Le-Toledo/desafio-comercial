using DesafioComercial.Models;
using DesafioComercial.Services;

namespace DesafioComercial.Tests;

public class EstoqueServiceTests
{
    private readonly EstoqueService _service = new();

    [Fact]
    public void Entrada_DeveAumentarEstoque()
    {
        // Arrange
        Produto produto = new()
        {
            CodigoProduto = 101,
            DescricaoProduto = "Caneta Azul",
            Estoque = 100
        };

        // Act
        MovimentacaoEstoque movimentacao =
            _service.Movimentar(
                produto,
                TipoMovimentacao.Entrada,
                20,
                "Recebimento de mercadoria"
            );

        // Assert
        Assert.Equal(120, produto.Estoque);
        Assert.Equal(100, movimentacao.EstoqueAnterior);
        Assert.Equal(120, movimentacao.EstoqueFinal);
    }

    [Fact]
    public void Saida_DeveDiminuirEstoque()
    {
        // Arrange
        Produto produto = new()
        {
            CodigoProduto = 101,
            DescricaoProduto = "Caneta Azul",
            Estoque = 100
        };

        // Act
        MovimentacaoEstoque movimentacao =
            _service.Movimentar(
                produto,
                TipoMovimentacao.Saida,
                30,
                "Venda de mercadoria"
            );

        // Assert
        Assert.Equal(70, produto.Estoque);
        Assert.Equal(100, movimentacao.EstoqueAnterior);
        Assert.Equal(70, movimentacao.EstoqueFinal);
    }

    [Fact]
    public void SaidaMaiorQueEstoque_DeveLancarExcecao()
    {
        // Arrange
        Produto produto = new()
        {
            CodigoProduto = 101,
            DescricaoProduto = "Caneta Azul",
            Estoque = 10
        };

        // Act + Assert
        InvalidOperationException excecao =
            Assert.Throws<InvalidOperationException>(() =>
                _service.Movimentar(
                    produto,
                    TipoMovimentacao.Saida,
                    20,
                    "Venda de mercadoria"
                )
            );

        Assert.Equal(
            "Estoque insuficiente para realizar a saída.",
            excecao.Message
        );

        // Garante que o estoque não foi alterado
        Assert.Equal(10, produto.Estoque);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void QuantidadeInvalida_DeveLancarExcecao(
        int quantidade)
    {
        // Arrange
        Produto produto = new()
        {
            CodigoProduto = 101,
            DescricaoProduto = "Caneta Azul",
            Estoque = 100
        };

        // Act + Assert
        Assert.Throws<ArgumentException>(() =>
            _service.Movimentar(
                produto,
                TipoMovimentacao.Entrada,
                quantidade,
                "Recebimento"
            )
        );
    }

    [Fact]
    public void DescricaoVazia_DeveLancarExcecao()
    {
        // Arrange
        Produto produto = new()
        {
            CodigoProduto = 101,
            DescricaoProduto = "Caneta Azul",
            Estoque = 100
        };

        // Act + Assert
        Assert.Throws<ArgumentException>(() =>
            _service.Movimentar(
                produto,
                TipoMovimentacao.Entrada,
                10,
                ""
            )
        );
    }
}