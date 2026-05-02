
namespace SimProgramming.Model;

public class Certificado : DocumentoBase
{
    public required string NomeFormando { get; set; }
    public required string Curso { get; set; }
    public DateTime DataEmissao { get; set; }
    public required string EntidadeEmissora { get; set; }

    public override List<string> Validar()
    {
        var erros = base.Validar();

        if (string.IsNullOrWhiteSpace(NomeFormando))
            erros.Add("O nome do formando é obrigatório.");
        else if (NomeFormando.Trim().Length < 2)
            erros.Add("O nome do formando deve ter pelo menos 2 caracteres.");

        if (string.IsNullOrWhiteSpace(Curso))
            erros.Add("O curso é obrigatório.");
        else if (Curso.Trim().Length < 2)
            erros.Add("O curso deve ter pelo menos 2 caracteres.");

        if (string.IsNullOrWhiteSpace(EntidadeEmissora))
            erros.Add("A entidade emissora é obrigatória.");

        if (DataEmissao == default)
            erros.Add("A data de emissão é obrigatória.");
        else
        {
            if (DataEmissao > DateTime.Now)
                erros.Add("A data de emissão não pode ser no futuro.");

            if (DataEmissao < DataCriacao)
                erros.Add("A data de emissão não pode ser anterior à data de criação.");
        }

        return erros;
    }
}

