using SimProgramming.Controller;
using SimProgramming.View;

// infraestrutura e UI
var view = new ConsoleView();
var pdfService = new PdfService();

// Injetar as dependências no Controller
var controller = new MainController(view, pdfService);

// Arrancar a aplicação
controller.Iniciar();