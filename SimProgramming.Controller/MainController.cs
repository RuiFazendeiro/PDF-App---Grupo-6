#region Usings

using SimProgramming.Controller.Interfaces;
using SimProgramming.Model;
using SimProgramming.Controller.Exceptions;

#endregion

namespace SimProgramming.Controller;

public class MainController
{
    private readonly IView _view;
    private readonly IPdfService _pdfService;

    public MainController(IView view, IPdfService pdfService)
    {
        _view = view;
        _pdfService = pdfService;
    }

    public void Iniciar()
    {
        _view.LimparConsola();
        _view.ExibirMensagem("=== SimProgramming: Gerador de Documentos ===");
        try
        {
            // 1. Captura de dados (View Passiva)
            string nome = _view.LerInput("Nome do Formando: ");
            string curso = _view.LerInput("Nome do Curso: ");

            // 2. Criação do Objeto de Domínio (Model da Andreia)
            var certificado = new Certificado
            {
                Titulo = "Certificado de Formação",
                NomeFormando = nome,
                Curso = curso,
                DataCriacao = DateTime.Now,
                DataEmissao = DateTime.Now,
                EntidadeEmissora = "SimProgramming"
            };

            // 3. Validação
            if (!certificado.Validar())
            {
                _view.ExibirMensagem("Erro: Dados introduzidos não cumprem os requisitos de validação.");
                return;
            }

            _view.ExibirMensagem("\nDados validados com sucesso. A gerar PDF...");

            // 4. Chamada ao Serviço (Trabalho do Frederico)
            _pdfService.GerarDocumento(certificado, "Certificado_Equipa6.pdf");

            _view.ExibirMensagem("Operação concluída. Verifique o ficheiro gerado.");
        }
        // --- BLOCOS CATCH ATUALIZADOS AQUI ---
        catch (DocumentValidationException dex)
        {
            _view.ExibirMensagem($"\n[Aviso] {dex.Message}");
        }
        catch (PdfGenerationException pex)
        {
            _view.ExibirMensagem($"\n[Falha ao Gravar] {pex.Message}");
            
            // 👇 Esta é a linha que vai revelar o erro de fonte do Linux!
            if (pex.InnerException != null)
            {
                _view.ExibirMensagem($"[Detalhe Técnico do Linux] {pex.InnerException.Message}");
            }
        }
        catch (ArgumentException aex)
        {
            _view.ExibirMensagem($"\n[Erro Interno] Parâmetro inválido: {aex.Message}");
        }
        catch (Exception ex)
        {
            _view.ExibirMensagem($"\n[Erro Crítico] {ex.Message}");
        }
    }
}