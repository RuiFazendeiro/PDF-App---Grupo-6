using System.Net.Mail;

namespace SimProgramming.Model.Utils;

public static class ValidacaoUtils
{
    public static bool EmailValido(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var endereco = new MailAddress(email);
            return endereco.Address == email.Trim();
        }
        catch
        {
            return false;
        }
    }
}