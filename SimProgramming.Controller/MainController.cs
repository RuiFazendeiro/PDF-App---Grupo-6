using SimProgramming.Controller.Interfaces;
using SimProgramming.View;

namespace SimProgramming.Controller;

public class MainController
{
    private readonly IView _view;
    private readonly IPdfService _pdfService;

    // Injeção de dependência: o Controller não cria as ferramentas, recebe-as.
    public MainController(IView view, IPdfService pdfService)
    {
        _view = view;
        _pdfService = pdfService;
    }

    public void Iniciar()
    {
        _view.LimparConsola();
        _view.ExibirMensagem("SimProgramming - Gerador de Documentos");
        
        // Fluxo Curry & Grace:
        // 1. Pedir dados (IView)
        // 2. Validar (Model)
        // 3. Se OK -> Gerar (IPdfService)
    }
}