📂 Análise de Dependências e Abstração (Equipa 6)

1. Introdução

Este documento descreve as dependências externas do projeto PDF-APP na SimProgramming e a estratégia arquitetónica adotada para mitigar os riscos associados ao acoplamento de bibliotecas de terceiros.
O foco principal é a proteção do núcleo da aplicação através de interfaces, garantindo a sua adaptabilidade e manutenção a longo prazo.

2. Dependência Externa: PDFSharp (v6.2.4)

A aplicação utiliza a biblioteca PDFSharp como motor de renderização de documentos.

Finalidade: Transpor os modelos de dados (Certificado e Relatorio) para o formato binário PDF.

Localização: A utilização da biblioteca está estritamente confinada à classe PdfService.cs no projeto SimProgramming.Controller.

Riscos Identificados: Dependência de uma API específica, dificuldade em realizar testes unitários sem gerar ficheiros físicos e potencial obsolescência da biblioteca.

3. Proteção via Interfaces: O Papel do IPdfService

Para evitar que o motor de PDF "contamine" toda a arquitetura MVC, implementámos o princípio da Inversão de Dependência.

🛡️ Isolamento da API
O contrato de serviço é definido pela interface IPdfService, que reside numa camada de abstração:

public interface IPdfService 
{
    void GerarDocumento(DocumentoBase documento, string caminhoArquivo);
}

Desta forma, o MainController não tem conhecimento da existência do PDFSharp.
Ele comunica apenas com a interface, o que permite trocar o motor de geração (ex: migrar para iText ou QuestPDF) sem alterar uma única linha de lógica no Controller ou na View.

💉 Injeção de Dependência
A ligação entre a interface e a implementação concreta é feita no arranque da aplicação (Program.cs), onde a instância de PdfService é injetada no Controller:

var pdfService = new PdfService();
var controller = new MainController(view, pdfService);

4. Benefícios para a Qualidade do Software
A aplicação destes mecanismos de independência garante os seguintes atributos de qualidade:

Testabilidade: O Verificador (João) pode criar um MockPdfService para testar o fluxo do Controller sem necessidade de instalar fontes ou manipular ficheiros PDF reais.

Manutenibilidade: Erros específicos da biblioteca (capturados em PdfGenerationException) são encapsulados no serviço, protegendo a experiência do utilizador.

Reutilização: O componente MainController e os modelos de domínio podem ser reutilizados em futuros projetos da empresa, independentemente da tecnologia de saída documental utilizada.