namespace SimProgramming.Model;

public abstract class DocumentoBase
{
    public required string Titulo { get; set; }
    public DateTime DataCriacao { get; set; }

    public virtual bool Validate()
    {
        return !string.IsNullOrWhiteSpace(Titulo)
            && DataCriacao != default;
    }
}