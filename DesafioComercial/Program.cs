using System.Globalization;
using System.Text.Json;
using DesafioComercial.Models;
using DesafioComercial.Services;

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");

while (true)
{
    Console.Clear();

    Console.WriteLine("==============================================");
    Console.WriteLine("              DESAFIO COMERCIAL");
    Console.WriteLine("==============================================");
    Console.WriteLine("1 - Relatório de comissões");
    Console.WriteLine("2 - Movimentação de estoque");
    Console.WriteLine("3 - Calcular juros");
    Console.WriteLine("0 - Sair");
    Console.WriteLine("==============================================");
    Console.Write("Escolha uma opção: ");

    string? opcao = Console.ReadLine();

    switch (opcao)
    {
        case "1":
            ExecutarRelatorioComissoes();
            break;

        case "2":
            ExecutarMovimentacaoEstoque();
            break;

        case "3":
            ExecutarCalculoJuros();
            break;

        case "0":
            Console.WriteLine("\nPrograma encerrado.");
            return;

        default:
            Console.WriteLine("\nOpção inválida.");
            Pausar();
            break;
    }
}


static void ExecutarRelatorioComissoes()
{
    Console.Clear();

    try
    {
        DadosVendas dados =
            LerJson<DadosVendas>("Data/vendas.json");

        if (dados.Vendas.Count == 0)
        {
            Console.WriteLine("Nenhuma venda encontrada.");
            Pausar();
            return;
        }

        ComissaoService service = new();

        IEnumerable<ResumoComissao> resultados =
            service.CalcularComissoesPorVendedor(dados.Vendas);

        Console.WriteLine("==============================================");
        Console.WriteLine("          RELATÓRIO DE COMISSÕES");
        Console.WriteLine("==============================================");

        foreach (ResumoComissao resultado in resultados)
        {
            Console.WriteLine();
            Console.WriteLine($"Vendedor: {resultado.Vendedor}");
            Console.WriteLine($"Total vendido: {resultado.TotalVendas:C}");
            Console.WriteLine($"Comissão: {resultado.TotalComissao:C}");
        }

        Console.WriteLine();
        Console.WriteLine("==============================================");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro: {ex.Message}");
    }

    Pausar();
}


static void ExecutarMovimentacaoEstoque()
{
    Console.Clear();

    try
    {
        const string caminhoEstoque = "Data/estoque.json";
        const string caminhoMovimentacoes = "Data/movimentacoes.json";

        DadosEstoque dados =
            LerJson<DadosEstoque>(caminhoEstoque);

        Console.WriteLine("==============================================");
        Console.WriteLine("             CONTROLE DE ESTOQUE");
        Console.WriteLine("==============================================");

        foreach (Produto produto in dados.Estoque)
        {
            Console.WriteLine(
                $"{produto.CodigoProduto} - " +
                $"{produto.DescricaoProduto} " +
                $"| Estoque: {produto.Estoque}"
            );
        }

        Console.WriteLine();
        Console.Write("Informe o código do produto: ");

        if (!int.TryParse(
                Console.ReadLine(),
                out int codigoProduto))
        {
            Console.WriteLine("\nCódigo do produto inválido.");
            Pausar();
            return;
        }

        Produto? produtoSelecionado =
            dados.Estoque.FirstOrDefault(
                produto =>
                    produto.CodigoProduto == codigoProduto
            );

        if (produtoSelecionado is null)
        {
            Console.WriteLine("\nProduto não encontrado.");
            Pausar();
            return;
        }

        Console.WriteLine();
        Console.WriteLine(
            $"Produto: {produtoSelecionado.DescricaoProduto}"
        );

        Console.WriteLine(
            $"Estoque atual: {produtoSelecionado.Estoque}"
        );

        Console.WriteLine();
        Console.WriteLine("1 - Entrada");
        Console.WriteLine("2 - Saída");
        Console.Write("Tipo da movimentação: ");

        if (!int.TryParse(
                Console.ReadLine(),
                out int tipoInformado) ||
            !Enum.IsDefined(
                typeof(TipoMovimentacao),
                tipoInformado))
        {
            Console.WriteLine(
                "\nTipo de movimentação inválido."
            );

            Pausar();
            return;
        }

        TipoMovimentacao tipo =
            (TipoMovimentacao)tipoInformado;

        Console.Write("Quantidade: ");

        if (!int.TryParse(
                Console.ReadLine(),
                out int quantidade))
        {
            Console.WriteLine("\nQuantidade inválida.");
            Pausar();
            return;
        }

        Console.Write(
            "Descrição da movimentação: "
        );

        string descricao =
            Console.ReadLine() ?? string.Empty;

        EstoqueService service = new();

        MovimentacaoEstoque movimentacao =
            service.Movimentar(
                produtoSelecionado,
                tipo,
                quantidade,
                descricao
            );

        // Persiste o estoque atualizado
        SalvarJson(caminhoEstoque, dados);

        // Registra a movimentação no histórico
        DadosMovimentacoes historico =
            LerJson<DadosMovimentacoes>(
                caminhoMovimentacoes
            );

        historico.Movimentacoes.Add(
            movimentacao
        );

        SalvarJson(
            caminhoMovimentacoes,
            historico
        );

        Console.WriteLine();
        Console.WriteLine("==============================================");
        Console.WriteLine("       MOVIMENTAÇÃO REALIZADA COM SUCESSO");
        Console.WriteLine("==============================================");

        Console.WriteLine(
            $"ID: {movimentacao.Id}"
        );

        Console.WriteLine(
            $"Produto: {produtoSelecionado.DescricaoProduto}"
        );

        Console.WriteLine(
            $"Tipo: {movimentacao.Tipo}"
        );

        Console.WriteLine(
            $"Descrição: {movimentacao.Descricao}"
        );

        Console.WriteLine(
            $"Quantidade: {movimentacao.Quantidade}"
        );

        Console.WriteLine(
            $"Estoque anterior: {movimentacao.EstoqueAnterior}"
        );

        Console.WriteLine(
            $"Estoque final: {movimentacao.EstoqueFinal}"
        );

        Console.WriteLine(
            $"Data: {movimentacao.DataMovimentacao:dd/MM/yyyy HH:mm:ss}"
        );

        Console.WriteLine("==============================================");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine();

        Console.WriteLine(
            $"Movimentação não realizada: {ex.Message}"
        );
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine();

        Console.WriteLine(
            $"Dados inválidos: {ex.Message}"
        );
    }
    catch (Exception ex)
    {
        Console.WriteLine();

        Console.WriteLine(
            $"Erro: {ex.Message}"
        );
    }

    Pausar();
}


static void ExecutarCalculoJuros()
{
    Console.Clear();

    Console.WriteLine("==============================================");
    Console.WriteLine("              CÁLCULO DE JUROS");
    Console.WriteLine("==============================================");

    try
    {
        Console.Write(
            "Informe o valor: R$ "
        );

        if (!decimal.TryParse(
                Console.ReadLine(),
                NumberStyles.Number,
                CultureInfo.GetCultureInfo("pt-BR"),
                out decimal valor))
        {
            Console.WriteLine("\nValor inválido.");
            Pausar();
            return;
        }

        Console.Write(
            "Informe a data de vencimento (dd/MM/yyyy): "
        );

        if (!DateTime.TryParseExact(
                Console.ReadLine(),
                "dd/MM/yyyy",
                CultureInfo.GetCultureInfo("pt-BR"),
                DateTimeStyles.None,
                out DateTime dataVencimento))
        {
            Console.WriteLine(
                "\nData inválida. Utilize o formato dd/MM/yyyy."
            );

            Pausar();
            return;
        }

        JurosService service = new();

        ResultadoJuros resultado =
            service.Calcular(
                valor,
                dataVencimento
            );

        Console.WriteLine();
        Console.WriteLine("==============================================");
        Console.WriteLine("             RESULTADO DO CÁLCULO");
        Console.WriteLine("==============================================");

        Console.WriteLine(
            $"Valor original: {resultado.ValorOriginal:C}"
        );

        Console.WriteLine(
            $"Data de vencimento: {resultado.DataVencimento:dd/MM/yyyy}"
        );

        Console.WriteLine(
            $"Data do cálculo: {resultado.DataCalculo:dd/MM/yyyy}"
        );

        Console.WriteLine(
            $"Dias de atraso: {resultado.DiasAtraso}"
        );

        Console.WriteLine(
            $"Taxa diária: {resultado.TaxaDiaria:P2}"
        );

        Console.WriteLine(
            $"Valor dos juros: {resultado.ValorJuros:C}"
        );

        Console.WriteLine(
            $"Valor total: {resultado.ValorTotal:C}"
        );

        Console.WriteLine("==============================================");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine();

        Console.WriteLine(
            $"Dados inválidos: {ex.Message}"
        );
    }
    catch (Exception ex)
    {
        Console.WriteLine();

        Console.WriteLine(
            $"Erro: {ex.Message}"
        );
    }

    Pausar();
}


static T LerJson<T>(
    string caminhoArquivo)
{
    if (!File.Exists(caminhoArquivo))
    {
        throw new FileNotFoundException(
            $"Arquivo '{caminhoArquivo}' não encontrado."
        );
    }

    string json =
        File.ReadAllText(caminhoArquivo);

    if (string.IsNullOrWhiteSpace(json))
    {
        throw new InvalidOperationException(
            $"O arquivo '{caminhoArquivo}' está vazio."
        );
    }

    var opcoes =
        new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

    T? dados =
        JsonSerializer.Deserialize<T>(
            json,
            opcoes
        );

    if (dados is null)
    {
        throw new JsonException(
            $"Não foi possível interpretar o arquivo '{caminhoArquivo}'."
        );
    }

    return dados;
}


static void SalvarJson<T>(
    string caminhoArquivo,
    T dados)
{
    var opcoes =
        new JsonSerializerOptions
        {
            WriteIndented = true
        };

    string json =
        JsonSerializer.Serialize(
            dados,
            opcoes
        );

    File.WriteAllText(
        caminhoArquivo,
        json
    );
}


static void Pausar()
{
    Console.WriteLine();

    Console.Write(
        "Pressione ENTER para continuar..."
    );

    Console.ReadLine();
}