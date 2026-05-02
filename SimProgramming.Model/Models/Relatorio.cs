

namespace SimProgramming.Model;
public class Relatorio : DocumentoBase
{
    public required string Autor { get; set; }
    public required string Conteudo { get; set; }

    public override List<string> Validar()
    {
        var erros = base.Validar();

        // Autor
        if (string.IsNullOrWhiteSpace(Autor))
            erros.Add("O autor é obrigatório.");
        else
        {
            if (Autor.Trim().Length < 2)
                erros.Add("O nome do autor deve ter pelo menos 2 caracteres.");

            if (Autor.Any(char.IsDigit))
                erros.Add("O nome do autor não pode conter números.");
        }

        // Conteúdo
        if (string.IsNullOrWhiteSpace(Conteudo))
            erros.Add("O conteúdo do relatório é obrigatório.");
        else if (Conteudo.Trim().Length < 10)
            erros.Add("O conteúdo deve ter pelo menos 10 caracteres.");

        return erros;
    }
}