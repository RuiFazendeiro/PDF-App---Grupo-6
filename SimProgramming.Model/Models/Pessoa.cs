
using SimProgramming.Model.Utils;
using System.Net.Mail;

namespace SimProgramming.Model;
public class Pessoa
{
    public required string Nome { get; set; }
    public required string Email { get; set; }

    public List<string> Validar()
    {
        var erros = new List<string>();

        if (string.IsNullOrWhiteSpace(Nome))
            erros.Add("O nome é obrigatório.");
        else
        {
            if (Nome.Trim().Length < 2)
                erros.Add("O nome deve ter pelo menos 2 caracteres.");

            if (Nome.Any(char.IsDigit))
                erros.Add("O nome não pode conter números.");
        }

        if (string.IsNullOrWhiteSpace(Email))
            erros.Add("O email é obrigatório.");
        else if (!ValidacaoUtils.EmailValido(Email))
            erros.Add("O email não é válido.");

        return erros;
    }
}