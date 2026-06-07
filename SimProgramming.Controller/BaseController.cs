#region Usings

using SimProgramming.Controller.Interfaces;
using SimProgramming.Model;
using SimProgramming.Controller.Exceptions;

#endregion

namespace SimProgramming.Controller;

public abstract class BaseController
{
    protected readonly IView _view;

    protected BaseController(IView view)
    {
        _view = view;
    }

    protected bool InputValido(string input, string mensagemErro)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            _view.MostrarErro(mensagemErro);
            return false;
        }

        return true;
    }

    protected bool OpcaoValida(string opcao, params string[] opcoesValidas)
    {
        if (string.IsNullOrWhiteSpace(opcao))
        {
            _view.MostrarErro("Opção vazia.");
            return false;
        }

        if (!opcoesValidas.Contains(opcao))
        {
            _view.MostrarErro("Opção inválida.");
            return false;
        }

        return true;
    }
}