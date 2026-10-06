using DesafioComercial.Models;

namespace DesafioComercial.Services;

public class ComissaoService
{
    public decimal CalcularComissao(decimal valorVenda)
    {
        return valorVenda switch
        {
            < 100m => 0m,
            < 500m => valorVenda * 0.01m,
            _ => valorVenda * 0.05m
        };
    }

    public IEnumerable<ResumoComissao> CalcularComissoesPorVendedor(
        IEnumerable<Venda> vendas)
    {
        return vendas
            .GroupBy(venda => venda.Vendedor)
            .Select(grupo => new ResumoComissao
            {
                Vendedor = grupo.Key,
                TotalVendas = grupo.Sum(venda => venda.Valor),
                TotalComissao = grupo.Sum(
                    venda => CalcularComissao(venda.Valor)
                )
            })
            .OrderBy(resultado => resultado.Vendedor);
    }
}