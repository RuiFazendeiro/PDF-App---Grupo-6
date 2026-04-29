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
        Console.WriteLine("[SUCESSO] " + mensagem);
    }

    public void MostrarErro(string mensagem)
    {
        Console.WriteLine("[ERRO] " + mensagem);
    }

    public void MostrarMenu(string titulo, List<string> opcoes)
    {
        Console.WriteLine("========================================");
        Console.WriteLine(titulo);
        Console.WriteLine("========================================");
        Console.WriteLine();

        for (int i = 0; i < opcoes.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {opcoes[i]}");
        }

        Console.WriteLine();
        Console.Write("Escolha uma opção: ");
    }

    #endregion

    #region Input

    public string LerInput(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }

    #endregion

    #region Core

    public void LimparConsola()
    {
        Console.Clear();
    }

    #endregion
}