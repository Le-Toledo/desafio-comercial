# Desafio Comercial - C# / .NET

Aplicação console desenvolvida em **C# com .NET 10** para resolução de um desafio técnico envolvendo regras de comissão de vendas, controle de estoque e cálculo de juros.

O projeto foi estruturado com separação entre modelos, regras de negócio e testes automatizados, buscando manter o código organizado, legível e de fácil manutenção.

## Funcionalidades

### 1. Cálculo de Comissões

O sistema lê as vendas a partir de um arquivo JSON e calcula a comissão individual de cada venda conforme as seguintes regras:

- Venda abaixo de R$ 100,00: sem comissão
- Venda de R$ 100,00 até R$ 499,99: 1%
- Venda a partir de R$ 500,00: 5%

As vendas são agrupadas por vendedor e o sistema apresenta:

- Total vendido
- Total de comissão

O agrupamento e a consolidação dos dados são realizados utilizando LINQ.

---

### 2. Controle de Estoque

O sistema permite realizar movimentações de **entrada e saída** de produtos.

Cada movimentação possui:

- Identificador único (GUID)
- Código do produto
- Tipo da movimentação
- Descrição
- Quantidade
- Estoque anterior
- Estoque final
- Data e hora da movimentação

Também foram implementadas validações para impedir:

- Quantidades iguais ou menores que zero
- Movimentações sem descrição
- Saídas superiores ao estoque disponível
- Seleção de produtos inexistentes

Após uma movimentação válida, o estoque atualizado é persistido em arquivo JSON.

As movimentações também são armazenadas em um histórico, permitindo maior rastreabilidade das operações.

---

### 3. Cálculo de Juros

O sistema recebe:

- Valor original
- Data de vencimento

Caso o valor esteja vencido, são calculados **juros simples de 2,5% ao dia** sobre o valor informado.

O resultado apresenta:

- Valor original
- Data de vencimento
- Data do cálculo
- Dias em atraso
- Taxa diária
- Valor dos juros
- Valor total atualizado

Datas futuras ou com vencimento no dia atual não geram juros.

> Como o desafio informa uma taxa de 2,5% ao dia sem especificar capitalização composta, foi adotado o cálculo de juros simples.

---

## Tecnologias Utilizadas

- C#
- .NET 10
- System.Text.Json
- LINQ
- xUnit
- Git
- GitHub

---

## Estrutura do Projeto

```text
desafio-comercial/
├── DesafioComercial.slnx
├── .gitignore
├── README.md
│
├── DesafioComercial/
│   ├── Data/
│   │   ├── estoque.json
│   │   ├── movimentacoes.json
│   │   └── vendas.json
│   │
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
```

---

## Organização da Aplicação

### Models

Representam as entidades e estruturas de dados utilizadas pela aplicação, como vendas, produtos, movimentações e resultados dos cálculos.

### Services

Concentram as principais regras de negócio:

- Cálculo de comissão
- Movimentação de estoque
- Cálculo de juros

Essa separação evita concentrar as regras de negócio no `Program.cs` e facilita a manutenção e os testes.

### Data

Contém os arquivos JSON utilizados para leitura e persistência dos dados.

### Tests

Contém os testes automatizados responsáveis por validar as principais regras de negócio da aplicação.

---

## Como Executar

### Pré-requisitos

É necessário possuir o **.NET 10 SDK** instalado.

### 1. Clonar o repositório

```bash
git clone https://github.com/Le-Toledo/desafio-comercial.git
cd desafio-comercial
```

### 2. Restaurar as dependências

```bash
dotnet restore DesafioComercial.slnx
```

### 3. Compilar a solução

```bash
dotnet build DesafioComercial.slnx
```

### 4. Executar a aplicação

```bash
dotnet run --project DesafioComercial/DesafioComercial.csproj
```

A aplicação disponibiliza um menu com acesso às três funcionalidades:

```text
1 - Relatório de comissões
2 - Movimentação de estoque
3 - Calcular juros
0 - Sair
```

---

## Testes Automatizados

O projeto possui testes automatizados utilizando **xUnit** para validar as principais regras de negócio.

### Comissões

São testados os limites das diferentes faixas de comissão, incluindo:

- Venda abaixo de R$ 100,00
- Venda exatamente em R$ 100,00
- Venda abaixo de R$ 500,00
- Venda exatamente em R$ 500,00

### Estoque

São validados cenários como:

- Entrada de produtos
- Saída de produtos
- Bloqueio de saída superior ao estoque disponível
- Garantia de que uma operação inválida não altera o estoque
- Quantidade zero
- Quantidades negativas
- Descrição obrigatória

### Juros

São testados:

- Cálculo para valor vencido
- Vencimento no dia atual
- Vencimento futuro
- Valores inválidos

Atualmente, a suíte possui **17 casos de teste automatizados**.

Para executar todos os testes:

```bash
dotnet test DesafioComercial.slnx
```

---

## Decisões Técnicas

### `decimal` para valores monetários

Foi utilizado `decimal` para representar valores financeiros, por ser mais apropriado para cálculos monetários e evitar problemas de precisão associados a tipos de ponto flutuante.

### Separação das regras de negócio

As principais regras foram isoladas em classes de serviço, mantendo o `Program.cs` responsável principalmente pelo fluxo da aplicação e interação com o usuário.

### LINQ

LINQ é utilizado para agrupar as vendas por vendedor e consolidar os valores de vendas e comissões.

A comissão é calculada individualmente para cada venda antes da consolidação por vendedor.

### Identificação das movimentações

Cada movimentação de estoque recebe um `Guid`, permitindo uma identificação única sem depender de um contador externo.

### Rastreabilidade do estoque

Cada movimentação registra:

- Estoque anterior
- Quantidade movimentada
- Estoque final
- Data e hora
- Descrição da operação

### Persistência em JSON

Os dados são lidos e gravados utilizando `System.Text.Json`.

A leitura e escrita foram centralizadas através de métodos genéricos, reduzindo duplicação de código.

### Validações

Foram implementadas validações para evitar estados inválidos, como:

- Estoque negativo
- Quantidades inválidas
- Movimentações sem descrição
- Valores financeiros inválidos

### Testes automatizados

As principais regras de negócio possuem cobertura através de testes xUnit, incluindo casos de limite e cenários de erro.

---

## Possível Evolução

Em um cenário de produção, a persistência em arquivos JSON poderia ser substituída por um banco de dados, como **SQL Server**, utilizando uma camada de acesso a dados.

As operações de atualização de estoque e registro da movimentação poderiam ser realizadas dentro de uma transação, garantindo consistência entre os dados.

A separação atual das regras de negócio facilita essa evolução sem exigir grandes alterações nos serviços existentes.

---

## Autor

**Leandro Toledo**

GitHub: `Le-Toledo`