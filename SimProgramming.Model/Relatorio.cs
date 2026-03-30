
namespace SimProgramming.Model;
public class Relatorio : DocumentoBase
{
    public required string Autor { get; set; }
    public required string Conteudo { get; set; }

    public override bool Validate()
    {
        return base.Validate()
            && !string.IsNullOrWhiteSpace(Autor)
            && !string.IsNullOrWhiteSpace(Conteudo);
    }
}