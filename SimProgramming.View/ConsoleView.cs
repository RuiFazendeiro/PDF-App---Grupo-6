using SimProgramming.Controller.Interfaces;

namespace SimProgramming.View;

public class ConsoleView : IView
{
    #region Output

    public void ExibirMensagem(string mensagem)
    {
        Console.WriteLine(mensagem);
    }

    public void ExibirTitulo(string titulo)
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine($"   {titulo}");
        Console.WriteLine("========================================");
        Console.WriteLine();
    }

    public void MostrarSucesso(string mensagem)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[SUCESSO] {mensagem}");
        Console.ResetColor();
    }

    public void MostrarErro(string mensagem)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ERRO] {mensagem}");
        Console.ResetColor();
    }

    public void MostrarAviso(string mensagem)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[AVISO] {mensagem}");
        Console.ResetColor();
    }

    public void MostrarMenu()
    {
        Console.WriteLine("========================================");
        Console.WriteLine("Menu");
        Console.WriteLine("========================================");
        Console.WriteLine();

        Console.WriteLine("1. Gerar Certificado");
        Console.WriteLine("2. Ajuda");
        Console.WriteLine("0. Sair");
        Console.WriteLine();
        Console.Write("Escolha uma opção: ");
    }

    #endregion

    #region Input

    public string LerInput(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine() ?? string.Empty;
    }

    #endregion

    #region Core

    public void LimparConsola()
    {
        Console.Clear();
    }

    #endregion
}