namespace DesafioComercial.Models;

public class MovimentacaoEstoque
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public int CodigoProduto { get; set; }

    public TipoMovimentacao Tipo { get; set; }

    public string Descricao { get; set; } = string.Empty;

    public int Quantidade { get; set; }

    public int EstoqueAnterior { get; set; }

    public int EstoqueFinal { get; set; }

    public DateTime DataMovimentacao { get; set; } = DateTime.Now;
}