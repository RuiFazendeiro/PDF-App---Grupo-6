Projeto SimProgramming - Equipa 6

**Curso : Engenharia Informática - UAb  
**Unidade Curricular: Lab. de Desenvolvimento de Software

**API Selecionada:** [PDFsharp](http://www.pdfsharp.net/)

## 📋 Sobre o Projeto
Este repositório contém a aplicação demonstradora desenvolvida em **C#** para o Tópico 2. O objetivo é criar um gerador de documentos PDF (Relatórios/Certificados) que ilustre a separação de responsabilidades através do padrão arquitetónico **MVC**.

## 👥 Constituição da Equipa
* **Líder:** Rui Fazendeiro
* **Desenvolvedores:** Andreia Correia, Frederico Antão, Telmo Silva
* **Verificador:** João Ferreira

## 🏗️ Estrutura do Projeto (MVC)
Para manter a organização, o código deve ser estruturado nas seguintes pastas dentro de `/src`:

* **`/Model`**: Classes de dados e lógica de negócio (ex: estrutura do relatório).
* **`/View`**: Interface de utilizador (Consola/UI) e renderização final do PDF via PDFsharp.
* **`/Controller`**: Orquestração entre o input do utilizador e a geração do documento.

## 📄 Tipos de Documentos Suportados

A aplicação suporta a geração de dois tipos principais de documentos em PDF:

### 1. **Certificado**
Documento de certificação de formação com validação de:
- **Nome do Formando** (mínimo 2 caracteres, obrigatório)
- **Curso** (mínimo 2 caracteres, obrigatório)
- **Data de Emissão** (não pode ser no futuro, não pode ser anterior à data de criação)
- **Entidade Emissora** (obrigatório)

**Exemplo de Certificado gerado:**
- Título: "Certificado de Formação"
- Campos validados e renderizados no PDF
- Metadados automáticos (autor: SimProgramming - Equipa 6)

### 2. **Relatório**
Documento de relatório com validação de:
- **Autor** (mínimo 2 caracteres, apenas letras - sem números, obrigatório)
- **Conteúdo** (mínimo 10 caracteres, obrigatório)
- **Título** (mínimo 2 caracteres, obrigatório)

**Exemplo de Relatório gerado:**
- Inclui conteúdo estruturado
- Identificação do autor
- Validação rigorosa de dados

### Classe Base: DocumentoBase
Ambos os tipos herdam de `DocumentoBase` com validações comuns:
- **Título** (obrigatório, mínimo 2 caracteres)
- **Data de Criação** (não pode ser no futuro)

**Fluxo de Validação:**
1. Validação de dados do tipo específico (Certificado ou Relatório)
2. Validação de dados da classe base (DocumentoBase)
3. Se houver erros, lançamento de `DocumentValidationException`
4. Se válido, geração do PDF

## 📖 Guia de Utilização

### Arquitetura e Padrões de Design

#### **Interfaces Principais**

A aplicação utiliza interfaces para garantir **acoplamento** e **flexibilidade**:

##### IView
Interface para a camada de apresentação:
```csharp
public interface IView
{
    void LimparConsola();
    void ExibirMensagem(string mensagem);
    void ExibirTitulo(string titulo);
    void MostrarMenu(string titulo, List<string> opcoes);
    string LerInput(string prompt);
    void MostrarSucesso(string mensagem);
    void MostrarErro(string mensagem);
}
```

**Implementação Atual:** `ConsoleView` - fornece interface de consola amigável

##### IPdfService
Interface para a geração de documentos PDF:
```csharp
public interface IPdfService
{
    void GerarDocumento(DocumentoBase documento, string caminhoArquivo);
    void GerarDocumento(DocumentoBase documento, Stream stream);
}
```

**Implementações:**
- `PdfService` - Geração real de PDFs usando PDFsharp
- `MockPdfService` - Simulação para testes e modo experimental (com eventos reativos)

#### **Tratamento de Erros**

A aplicação usa exceções específicas para diferentes cenários:

| Exceção | Quando | Exemplo |
|---------|--------|---------|
| `DocumentValidationException` | Dados inválidos do documento | Nome formando vazio, data no futuro |
| `PdfGenerationException` | Falha ao gerar/gravar PDF | Disco cheio, permissões negadas, caminho inválido |
| `ArgumentException` | Parâmetros inválidos | Caminho do ficheiro nulo ou vazio |

**Tratamento no MainController:**
```csharp
try
{
    // Geração do documento
    _pdfService.GerarDocumento(certificado, "ficheiro.pdf");
}
catch (DocumentValidationException dex)
{
    _view.ExibirMensagem($"[Aviso] {dex.Message}");
}
catch (PdfGenerationException pex)
{
    _view.ExibirMensagem($"[Falha ao Gravar] {pex.Message}");
    if (pex.InnerException != null)
    {
        _view.ExibirMensagem($"[Detalhe Técnico] {pex.InnerException.Message}");
    }
}
```

### ConsoleView - Interface de Utilizador

A `ConsoleView` é a camada de apresentação da aplicação que fornece uma interface de consola amigável para interagir com o sistema. Esta classe implementa a interface `IView` e oferece métodos para:

- **Exibição de Mensagens**: Apresentar informações estruturadas ao utilizador
- **Menus Interativos**: Guiar o utilizador através de opções numeradas
- **Entrada de Dados**: Recolher informações do utilizador com prompts personalizados
- **Feedback Visual**: Indicar sucesso ou erro de operações com prefixos visuais

## 🔄 Modos de Operação

A aplicação pode ser executada em dois modos distintos, configuráveis no ficheiro `Program.cs`:

### Modo 1: Experimental (Com MockPdfService)
**Estado Atual:** ACTIVADO (predefinido)

**Características:**
- Não gera ficheiros PDF reais
- Simula o fluxo de geração com eventos reativos
- Ideal para testes e apresentações (páginas 159-161 da documentação)
- Menos dependências de sistema

**Código de Ativação:**
```csharp
IPdfService mockService = new MockPdfService();

// Subscreve ao evento de forma reativa (Acoplamento)
((MockPdfService)mockService).AoProcessar += (titulo) => {
    Console.WriteLine($"[Evento Reativo] O sistema intercetou a geração de: {titulo}");
};

MainController controllerExperimental = new MainController(view, mockService);
controllerExperimental.Iniciar();
```

**Exemplo de Output:**
```
[Evento Reativo] O sistema intercetou a geração de: Certificado de Formação
```

### Modo 2: Produção (Com PdfService Real)
**Estado Atual:** COMENTADO (descomentar para ativar)

**Características:**
- Gera ficheiros PDF reais usando a biblioteca PDFsharp
- Inclui validação completa de dados
- Tratamento robusto de erros (disco cheio, permissões, caminhos)
- Inclui metadados nos PDFs (título, autor)

**Código de Ativação:**
```csharp
IPdfService pdfService = new PdfService();
MainController controllerReal = new MainController(view, pdfService);
controllerReal.Iniciar();
```

**Como Alternar Entre Modos:**
1. Abra `SimProgramming.View/Program.cs`
2. Comente o bloco `MODO 1: CASO EXPERIMENTAL`
3. Descomente o bloco `MODO 2: PRODUÇÃO REAL`
4. Execute novamente: `dotnet run --project SimProgramming.View/SimProgramming.View.csproj`

### ConsoleView - Interface de Utilizador

A `ConsoleView` é a camada de apresentação da aplicação que fornece uma interface de consola amigável para interagir com o sistema. Esta classe implementa a interface `IView` e oferece métodos para:

- **Exibição de Mensagens**: Apresentar informações estruturadas ao utilizador
- **Menus Interativos**: Guiar o utilizador através de opções numeradas
- **Entrada de Dados**: Recolher informações do utilizador com prompts personalizados
- **Feedback Visual**: Indicar sucesso ou erro de operações com prefixos visuais

#### Métodos Principais

| Método | Descrição | Utilização |
|--------|-----------|-----------|
| `ExibirMensagem(string)` | Mostra uma mensagem simples | `view.ExibirMensagem("Bem-vindo ao sistema!")` |
| `ExibirTitulo(string)` | Exibe um título formatado com caixas visuais | `view.ExibirTitulo("Gestão de Certificados")` |
| `MostrarMenu(string, List<string>)` | Apresenta um menu numerado com opções | Ver exemplo abaixo |
| `LerInput(string)` | Lê uma linha de texto do utilizador | `string nome = view.LerInput("Introduza o nome: ")` |
| `MostrarSucesso(string)` | Mostra mensagem com prefixo [SUCESSO] | `view.MostrarSucesso("Certificado criado!")` |
| `MostrarErro(string)` | Mostra mensagem com prefixo [ERRO] | `view.MostrarErro("Email inválido!")` |
| `LimparConsola()` | Limpa o ecrã da consola | `view.LimparConsola()` |

## 🎯 Fluxo Real da Aplicação

O ponto de entrada da aplicação é o `MainController.Iniciar()`, que implementa o fluxo passo-a-passo:

### 1. **Inicialização**
```
MainController.Iniciar() 
→ Limpa a consola
→ Exibe título da aplicação
```

### 2. **Recolha de Dados**
```
Prompts do Utilizador:
- "Nome do Formando: "
- "Nome do Curso: "
```

### 3. **Criação do Objeto de Domínio**
```csharp
var certificado = new Certificado
{
    Titulo = "Certificado de Formação",
    NomeFormando = nome,           // Input do utilizador
    Curso = curso,                 // Input do utilizador
    DataCriacao = DateTime.Now,    // Automático
    DataEmissao = DateTime.Now,    // Automático
    EntidadeEmissora = "SimProgramming"
};
```

### 4. **Validação Robusta**
```
certificado.Validar()
↓
Verifica:
- Nome do formando (mínimo 2 caracteres)
- Curso (mínimo 2 caracteres)
- Data de emissão (não no futuro)
- Data de emissão (não anterior a data de criação)
- Título (mínimo 2 caracteres)
- Data de criação (não no futuro)

SE houver erros → Exibe lista de erros e termina
SE válido → Continua para geração de PDF
```

### 5. **Geração de PDF**
```
_pdfService.GerarDocumento(certificado, "Certificado_Equipa6.pdf")
↓
Depende do modo:
- MockPdfService: Simula a geração (dispara evento)
- PdfService: Gera ficheiro PDF real
```

### 6. **Tratamento de Erros**
```
Possíveis exceções:
├─ DocumentValidationException
│  └─ Dados inválidos do documento
├─ PdfGenerationException
│  ├─ Disco cheio
│  ├─ Permissões negadas
│  ├─ Caminho inválido
│  └─ Ficheiro aberto noutro programa
└─ Exception genérica
   └─ Erros inesperados
```

### 7. **Feedback ao Utilizador**
```
Sucesso: "Operação concluída. Verifique o ficheiro gerado."
Erro: Mensagem específica da exceção
```

## 💡 Exemplos Práticos

### Exemplo 1: Fluxo Principal de Geração de Certificado

Este exemplo mostra o fluxo real utilizado pela aplicação:

```csharp
public class FluxoPrincipal
{
    public static void Main()
    {
        // 1. Inicializa a View
        IView view = new ConsoleView();

        // 2. Seleciona o modo de operação
        IPdfService pdfService = new PdfService(); // Ou MockPdfService para teste

        // 3. Cria o controlador
        MainController controller = new MainController(view, pdfService);

        // 4. Executa o fluxo (que trata tudo automaticamente)
        controller.Iniciar();
    }
}
```

**O que acontece internamente:**
1. User é solicitado para introduzir "Nome do Formando" e "Nome do Curso"
2. Um objeto Certificado é criado com dados validados
3. Se válido, o PDF é gerado; caso contrário, erros são mostrados
4. Feedback visual é fornecido ao utilizador

### Exemplo 2: Criar e Validar um Certificado Manualmente

Este exemplo mostra como validar um certificado antes de gerar o PDF:

```csharp
// Dados do certificado
string nomeFormando = "Cristiano Ronaldo";
string curso = "Desenvolvimento em C#";

// Criar a instância do certificado
var certificado = new Certificado
{
    Titulo = "Certificado de Formação",
    NomeFormando = nomeFormando,
    Curso = curso,
    DataCriacao = DateTime.Now,
    DataEmissao = DateTime.Now,
    EntidadeEmissora = "SimProgramming"
};

// Validar o certificado
var erros = certificado.Validar();

if (erros.Count == 0)
{
    Console.WriteLine("✓ Certificado válido!");
    Console.WriteLine($"Formando: {certificado.NomeFormando}");
    Console.WriteLine($"Curso: {certificado.Curso}");

    // Gerar PDF
    var pdfService = new PdfService();
    pdfService.GerarDocumento(certificado, "certificado.pdf");
}
else
{
    Console.WriteLine("✗ Erros na validação:");
    foreach (var erro in erros)
    {
        Console.WriteLine($"  - {erro}");
    }
}
```

### Exemplo 3: Criar e Validar um Relatório

Este exemplo mostra como validar um relatório (com regras mais estritas que o certificado):

```csharp
// Dados do relatório
string autor = "Cristiano Ronaldo";
string conteudo = "Este é um relatório detalhado sobre os resultados do projecto SimProgramming.";
string titulo = "Relatório Técnico";

// Criar a instância do relatório
var relatorio = new Relatorio
{
    Titulo = titulo,
    Autor = autor,
    Conteudo = conteudo,
    DataCriacao = DateTime.Now
};

// Validar o relatório
var erros = relatorio.Validar();

if (erros.Count == 0)
{
    Console.WriteLine("✓ Relatório válido!");
    Console.WriteLine($"Autor: {relatorio.Autor}");
    Console.WriteLine($"Conteúdo: {relatorio.Conteudo.Substring(0, 50)}...");

    // Gerar PDF
    var pdfService = new PdfService();
    pdfService.GerarDocumento(relatorio, "relatorio.pdf");
}
else
{
    Console.WriteLine("✗ Erros na validação:");
    foreach (var erro in erros)
    {
        Console.WriteLine($"  - {erro}");
    }
}
```

**Regras de Validação do Relatório:**
- Autor: mínimo 2 caracteres, sem números
- Conteúdo: mínimo 10 caracteres
- Título: mínimo 2 caracteres

### Exemplo 4: Usar MockPdfService para Testes

Este exemplo mostra como usar o modo experimental com eventos:

```csharp
// 1. Criar mock service
var mockService = new MockPdfService();

// 2. Subscrever ao evento de processamento
mockService.AoProcessar += (titulo) => {
    Console.WriteLine($"[EVENTO] Sistema processou: {titulo}");
};

// 3. Usar normalmente
var view = new ConsoleView();
var controller = new MainController(view, mockService);
controller.Iniciar();

// Output esperado:
// [EVENTO] Sistema processou: Certificado de Formação
```

### Exemplo 5: Tratamento Completo de Erros

Este exemplo mostra como tratar diferentes cenários de erro:

```csharp
public class GestorComErros
{
    private readonly IView _view;
    private readonly IPdfService _pdfService;

    public GestorComErros(IView view, IPdfService pdfService)
    {
        _view = view;
        _pdfService = pdfService;
    }

    public void CriarCertificadoComTratamento()
    {
        _view.ExibirTitulo("Criar Certificado");

        try
        {
            // Recolher dados
            string nome = _view.LerInput("Nome do Formando: ");
            string curso = _view.LerInput("Nome do Curso: ");

            if (string.IsNullOrWhiteSpace(nome) || nome.Length < 2)
            {
                _view.MostrarErro("Nome deve ter pelo menos 2 caracteres!");
                return;
            }

            if (string.IsNullOrWhiteSpace(curso) || curso.Length < 2)
            {
                _view.MostrarErro("Curso deve ter pelo menos 2 caracteres!");
                return;
            }

            // Criar certificado
            var certificado = new Certificado
            {
                Titulo = "Certificado de Formação",
                NomeFormando = nome,
                Curso = curso,
                DataCriacao = DateTime.Now,
                DataEmissao = DateTime.Now,
                EntidadeEmissora = "SimProgramming"
            };

            // Gerar PDF com tratamento de erros
            _pdfService.GerarDocumento(certificado, "certificado.pdf");
            _view.MostrarSucesso("Certificado gerado com sucesso!");
        }
        catch (DocumentValidationException dex)
        {
            _view.MostrarErro($"Validação falhou: {dex.Message}");
        }
        catch (PdfGenerationException pex)
        {
            _view.MostrarErro($"Erro ao gerar PDF: {pex.Message}");
            if (pex.InnerException != null)
            {
                _view.ExibirMensagem($"Detalhe técnico: {pex.InnerException.Message}");
            }
        }
        catch (Exception ex)
        {
            _view.MostrarErro($"Erro inesperado: {ex.Message}");
        }
    }
}
```

## 🎯 Boas Práticas ao Usar ConsoleView

### ✅ Fazer

```csharp
// 1. Sempre validar dados antes de criar objetos
string nome = view.LerInput("Nome: ");
if (string.IsNullOrWhiteSpace(nome))
{
    view.MostrarErro("Nome não pode estar vazio!");
    return;
}

// 2. Usar títulos para estruturar operações
view.ExibirTitulo("Operação Principal");

// 3. Fornecer feedback claro ao utilizador
view.MostrarSucesso("✓ Operação concluída com êxito!");

// 4. Usar try-catch para erros inesperados
try
{
    // código
}
catch (Exception ex)
{
    view.MostrarErro($"Erro: {ex.Message}");
}
```

### ❌ Evitar

```csharp
// 1. Não criar objetos sem validação
string nome = view.LerInput("Nome: ");
var pessoa = new Pessoa { Nome = nome }; // Sem validar!

// 2. Não misturar menus sem estructura visual
Console.WriteLine("1. Opção 1");
view.MostrarMenu("Menu", opcoes); // Inconsistente

// 3. Não ignorar erros
var erros = objeto.Validar();
// ... sem processar erros

// 4. Não deixar a consola sem feedbacks
// Utilizador fica sem saber se operação foi bem-sucedida
```

## ⚠️ Estratégia de Tratamento de Erros

A aplicação implementa uma estratégia robusta de tratamento de erros em múltiplas camadas:

### Camada 1: Validação de Dados (Model)
Cada classe de domínio (Certificado, Relatório) implementa o método `Validar()`:
- Retorna uma lista de erros
- Não lança exceções - permite recolher todos os erros de uma vez
- **Vantagem:** Feedback completo ao utilizador

### Camada 2: Validação de PDF (PdfService)
O `PdfService` valida dados antes de gerar:
```csharp
public void GerarDocumento(DocumentoBase documento, Stream stream)
{
    var erros = documento.Validar();
    if (erros.Any())
    {
        throw new DocumentValidationException(string.Join(" | ", erros));
    }
    // ... resto da geração
}
```

### Camada 3: Tratamento de Exceções (MainController)
```csharp
try
{
    _pdfService.GerarDocumento(certificado, "ficheiro.pdf");
}
catch (DocumentValidationException dex)
{
    // Dados inválidos
    _view.ExibirMensagem($"[Aviso] {dex.Message}");
}
catch (PdfGenerationException pex)
{
    // Falha ao gerar/gravar PDF
    _view.ExibirMensagem($"[Falha ao Gravar] {pex.Message}");
    if (pex.InnerException != null)
    {
        _view.ExibirMensagem($"[Detalhe Técnico] {pex.InnerException.Message}");
    }
}
catch (Exception ex)
{
    // Erro inesperado
    _view.ExibirMensagem($"[Erro Crítico] {ex.Message}");
}
```

### Tipos de Erros Tratados

| Cenário | Exceção | Solução |
|---------|---------|--------|
| Nome formando vazio | `DocumentValidationException` | Solicitar novo input |
| Data no futuro | `DocumentValidationException` | Corrigir data |
| Disco cheio | `PdfGenerationException` | Liberar espaço em disco |
| Permissões negadas | `PdfGenerationException` | Verificar permissões do diretório |
| Ficheiro aberto noutro programa | `PdfGenerationException` | Fechar ficheiro noutro programa |
| Caminho inválido | `PdfGenerationException` | Verificar caminho do ficheiro |
| Erro desconhecido | `Exception` genérica | Verificar logs do sistema |

## 📌 Fluxo Típico de Utilização

1. **Iniciar a aplicação** → Chama `MainController.Iniciar()`
2. **Limpar e exibir título** → Consola limpa, mensagem de boas-vindas
3. **Recolher dados obrigatórios** → Solicita "Nome do Formando" e "Nome do Curso"
4. **Criar objeto Certificado** → Com dados fornecidos e metadata automática
5. **Validar dados** → Executa `certificado.Validar()`
6. **Se inválido** → Exibe lista de erros e termina
7. **Se válido** → Procede para geração de PDF
8. **Gerar PDF** → Chama `_pdfService.GerarDocumento(certificado, "ficheiro.pdf")`
9. **Feedback final** → "Operação concluída. Verifique o ficheiro gerado."

## 🛠️ Tecnologias Utilizadas
* Linguagem: **C#** (.NET)
* Versão: **.NET 9** (SDK mínimo recomendado: .NET 9)
* Biblioteca: **PDFsharp** (Instalar via NuGet)
* IDE Recomendada: **Visual Studio 2022** (ou posterior)

## 🚀 Começar a Usar

### Instalação e Execução

1. **Clone o repositório:**
```bash
git clone https://github.com/RuiFazendeiro/PDF-App---Grupo-6.git
cd PDF-App---Grupo-6
```

2. **Restaure as dependências:**
```bash
dotnet restore
```

3. **Compile a solução:**
```bash
dotnet build
```

4. **Execute a aplicação:**
```bash
dotnet run --project SimProgramming.View/SimProgramming.View.csproj
```

## 🧪 Testes

Para executar os testes unitários:

```bash
dotnet test SimProgramming.Tests/SimProgramming.Tests.csproj
```

## 🚦 Regras de Fluxo (Git)
1. **Nunca** fazer commit direto na branch `main`.
2. Criar uma branch própria para cada tarefa: `feature/nome-da-tarefa`.
3. Abrir um **Pull Request** para revisão do Verificador antes do merge.
4. Garantir que o ficheiro `.gitignore` está ativo para evitar ficheiros temporários (`bin/obj`).

Para uma análise detalhada da gestão de APIs externas, consulte o DEPENDENCIAS.md

```
PDF-App---Grupo-6/
├── SimProgramming.View/
│   ├── ConsoleView.cs          # Implementação da interface de consola
│   ├── Program.cs              # Ponto de entrada da aplicação
│   └── SimProgramming.View.csproj
├── SimProgramming.Model/
│   ├── Models/
│   │   ├── Pessoa.cs           # Modelo de pessoa com validação
│   │   ├── Certificado.cs      # Modelo de certificado com validação
│   │   ├── Relatorio.cs        # Modelo de relatório
│   │   └── DocumentoBase.cs    # Classe base para documentos
│   ├── Utils/
│   │   └── ValidacaoUtils.cs   # Funções de validação reutilizáveis
│   └── SimProgramming.Model.csproj
├── SimProgramming.Controller/
│   └── SimProgramming.Controller.csproj
├── SimProgramming.Tests/
│   └── SimProgramming.Tests.csproj
├── README.md                   # Este ficheiro
├── DEPENDENCIAS.md             # Documentação de dependências
└── SimProgramming.PDFsharp.sln # Ficheiro de solução

Para uma análise detalhada da gestão de APIs externas, consulte o **DEPENDENCIAS.md**.

## 🤝 Contacto e Suporte

Para dúvidas ou sugestões sobre o guia de utilização, contacte o líder da equipa ou abra uma **issue** no repositório.

---
*Este projeto foi desenvolvido no âmbito da SimProgramming 2026.*
