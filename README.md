# Desafio Comercial - C# / .NET

Aplicação console desenvolvida em C# com .NET 10 para resolução de um desafio técnico envolvendo regras de comissão de vendas, controle de estoque e cálculo de juros.

O projeto foi estruturado separando modelos, regras de negócio e testes automatizados, buscando manter o código organizado, legível e de fácil manutenção.

## Funcionalidades

### 1. Cálculo de comissões

O sistema lê as vendas a partir de um arquivo JSON e calcula a comissão individual de cada venda conforme as seguintes regras:

- Venda abaixo de R$ 100,00: sem comissão
- Venda de R$ 100,00 até R$ 499,99: 1%
- Venda a partir de R$ 500,00: 5%

As vendas são agrupadas por vendedor e o sistema apresenta:

- total vendido;
- total de comissão.

### 2. Controle de estoque

O sistema permite realizar movimentações de entrada e saída de produtos.

Cada movimentação possui:

- identificador único (GUID);
- código do produto;
- tipo da movimentação;
- descrição;
- quantidade;
- estoque anterior;
- estoque final;
- data e hora da movimentação.

Também foram implementadas validações para impedir:

- quantidades iguais ou menores que zero;
- movimentações sem descrição;
- saída superior ao estoque disponível;
- seleção de produtos inexistentes.

O estoque atualizado é persistido em JSON e as movimentações são armazenadas em um histórico.

### 3. Cálculo de juros

O sistema recebe:

- valor original;
- data de vencimento.

Caso o pagamento esteja vencido, são calculados juros simples de 2,5% ao dia sobre o valor informado.

O resultado apresenta:

- valor original;
- data de vencimento;
- data do cálculo;
- dias em atraso;
- taxa diária;
- valor dos juros;
- valor total atualizado.

Datas futuras ou com vencimento no dia atual não geram juros.

## Tecnologias utilizadas

- C#
- .NET 10
- System.Text.Json
- LINQ
- xUnit
- Git

## Estrutura do projeto

```text
DesafioComercial.Repositorio/
├── DesafioComercial.slnx
├── DesafioComercial/
│   ├── Data/
│   ├── Models/
│   ├── Services/
│   ├── Program.cs
│   └── DesafioComercial.csproj
│
└── DesafioComercial.Tests/
    ├── ComissaoServiceTests.cs
    ├── EstoqueServiceTests.cs
    ├── JurosServiceTests.cs
    └── DesafioComercial.Tests.csproj