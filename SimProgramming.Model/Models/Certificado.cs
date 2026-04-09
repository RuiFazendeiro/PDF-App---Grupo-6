
namespace SimProgramming.Model;

public class Certificado : DocumentoBase
{
    public required string NomeFormando { get; set; }
    public required string Curso { get; set; }
    public DateTime DataEmissao { get; set; }
    public required string EntidadeEmissora { get; set; }

    public override bool Validar()
    {
        return base.Validar()
            && !string.IsNullOrWhiteSpace(NomeFormando)
            && NomeFormando.Trim().Length >= 2
            && !string.IsNullOrWhiteSpace(Curso)
            && Curso.Trim().Length >= 2
            && !string.IsNullOrWhiteSpace(EntidadeEmissora)
            && DataEmissao != default
            && DataEmissao <= DateTime.Now
            && DataEmissao >= DataCriacao;
    }
}

