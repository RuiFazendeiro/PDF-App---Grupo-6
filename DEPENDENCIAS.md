📂 Análise de Dependências e Abstração (Equipa 6)

1. Introdução
Este documento descreve as dependências externas do projeto PDF-App e a estratégia para mitigar riscos de acoplamento. O foco é a proteção do núcleo através de interfaces, garantindo adaptabilidade (p. 161).

2. Dependência Externa: PDFSharp (v6.1.1)

Finalidade: Renderização de Certificado e Relatorio em formato PDF.

Localização: Restrita à classe PdfService.cs.

Riscos: Dependência de API de terceiros e dificuldade em testes unitários isolados.

3. Proteção via Interfaces: IPdfService
Para evitar a "contaminação" do MVC, aplicámos a Inversão de Dependência. O MainController comunica apenas com a abstração.

🛡️ Isolamento da API
O contrato definido em IPdfService.cs é:

public interface IPdfService 
{
    void GerarPdf(DocumentoBase documento, string caminhoArquivo);
}
Nota: O motor de geração pode ser trocado sem alterar a lógica do Controller ou da View.

💉 Injeção de Dependência
A ligação ocorre no Program.cs. O MainController recebe as interfaces no construtor e utiliza o método Iniciar() para orquestrar o fluxo, eliminando o uso de new PdfService() ou new ConsoleView() dentro do controlador.

4. Benefícios para a Qualidade

Testabilidade: Permite ao João (Tester) injetar um MockPdfService.

Manutenibilidade: Erros técnicos são encapsulados em PdfGenerationException, isolando falhas da biblioteca externa.

Reutilização: O MainController e os modelos são agnósticos à tecnologia de saída.
