
using SimProgramming.Model.Utils;
using System.Net.Mail;

namespace SimProgramming.Model;
public class Pessoa
{
    public required string Nome { get; set; }
    public required string Email { get; set; }

    public bool Validar()
    {
        return !string.IsNullOrWhiteSpace(Nome)
            && Nome.Trim().Length >= 2
            && Nome.All(c => !char.IsDigit(c))
            && ValidacaoUtils.EmailValido(Email);
    }
}