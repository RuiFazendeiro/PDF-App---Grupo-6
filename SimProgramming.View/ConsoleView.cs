using SimProgramming.Controller.Interfaces;

namespace SimProgramming.View;

/* View Passiva (modelo Curry & Grace)
   Responsável apenas por Input/Output.
   Não contém lógica, validação ou decisão.*/

public class ConsoleView : IView
    {
        #region Output

        public void ExibirMensagem(string mensagem)
        {
            Console.WriteLine(mensagem);
        }

        public void LimparConsola()
        {
            Console.Clear();
        }

        #endregion

        #region Input

        public string LerInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine() ?? string.Empty;
        }

        #endregion
    }
