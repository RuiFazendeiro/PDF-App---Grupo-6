using System;
using SimProgramming.Controller;
using SimProgramming.Controller.Interfaces;
using SimProgramming.View;

namespace SimProgramming.View;

class Program
{
    static void Main(string[] args)
    {
        // 1. Inicializa a View Concreta
        IView view = new ConsoleView();

        // =====================================================================================
        // MODO 1: CASO EXPERIMENTAL (Descomentado para mostrar na atividade da PlataformAberta)
        // =====================================================================================
        Console.WriteLine("=== MODO EXPERIMENTAL ACTIVADO (PP. 159-161) ===");
        IPdfService mockService = new MockPdfService();
        
        // Subscreve ao evento de forma reativa (Acoplamento Fraco)
        ((MockPdfService)mockService).AoProcessar += (titulo) => {
            Console.WriteLine($"[Evento Reativo] O sistema intercetou a geração de: {titulo}");
        };

        MainController controllerExperimental = new MainController(view, mockService);
        controllerExperimental.Iniciar();

        // =====================================================================
        // MODO 2: PRODUÇÃO REAL (Com PDFSharp)
        // =====================================================================
        // Console.WriteLine("=== MODO PRODUÇÃO (PDFSHARP) ===");
        // IPdfService pdfService = new PdfService();
        // MainController controllerReal = new MainController(view, pdfService);
        // controllerReal.Iniciar();
    }
}