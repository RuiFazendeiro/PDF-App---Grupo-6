
namespace SimProgramming.Model;

public class Certificado : DocumentoBase
{
    public required string NomeFormando { get; set; }
    public required string Curso { get; set; }
    public DateTime DataEmissao { get; set; }
    public required string EntidadeEmissora { get; set; }

    public override bool Validate()
    {
        return !string.IsNullOrWhiteSpace(NomeFormando)
        && !string.IsNullOrWhiteSpace(Curso)
        && !string.IsNullOrWhiteSpace(EntidadeEmissora)
        && DataEmissao != default;
    }
}

