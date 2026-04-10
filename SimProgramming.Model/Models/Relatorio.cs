
namespace SimProgramming.Model;
public class Relatorio : DocumentoBase
{
    public required string Autor { get; set; }
    public required string Conteudo { get; set; }

    public override bool Validar()
    {
        return base.Validar()
            && !string.IsNullOrWhiteSpace(Autor)
            && Autor.Trim().Length >= 2
            && !string.IsNullOrWhiteSpace(Conteudo)
            && Conteudo.Trim().Length >= 10;
    }
}