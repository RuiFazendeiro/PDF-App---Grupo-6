namespace SimProgramming.View
{
    /* Interface da View (modelo Curry & Grace)
       Define apenas operações de Input/Output */

    public interface IView
    {
        void ExibirMensagem(string mensagem);
        string LerInput(string prompt);
        void LimparConsola();
    }
}