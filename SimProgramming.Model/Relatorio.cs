
namespace SimProgramming.Model;
public class Relatorio : DocumentoBase
{
    public required string Author { get; set; }
    public required string Content { get; set; }

    public override bool Validate()
    {
        return base.Validate()
            && !string.IsNullOrWhiteSpace(Author)
            && !string.IsNullOrWhiteSpace(Content);
    }
}