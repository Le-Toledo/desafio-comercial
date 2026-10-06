using DesafioComercial.Models;

namespace DesafioComercial.Services;

public class EstoqueService
{
    public MovimentacaoEstoque Movimentar(
        Produto produto,
        TipoMovimentacao tipo,
        int quantidade,
        string descricao)
    {
        if (quantidade <= 0)
        {
            throw new ArgumentException(
                "A quantidade deve ser maior que zero."
            );
        }

        if (string.IsNullOrWhiteSpace(descricao))
        {
            throw new ArgumentException(
                "A descrição da movimentação é obrigatória."
            );
        }

        int estoqueAnterior = produto.Estoque;

        switch (tipo)
        {
            case TipoMovimentacao.Entrada:
                produto.Estoque += quantidade;
                break;

            case TipoMovimentacao.Saida:
                if (quantidade > produto.Estoque)
                {
                    throw new InvalidOperationException(
                        "Estoque insuficiente para realizar a saída."
                    );
                }

                produto.Estoque -= quantidade;
                break;

            default:
                throw new ArgumentException(
                    "Tipo de movimentação inválido."
                );
        }

        return new MovimentacaoEstoque
        {
            CodigoProduto = produto.CodigoProduto,
            Tipo = tipo,
            Quantidade = quantidade,
            Descricao = descricao,
            EstoqueAnterior = estoqueAnterior,
            EstoqueFinal = produto.Estoque
        };
    }
}