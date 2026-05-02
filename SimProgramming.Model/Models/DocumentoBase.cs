namespace SimProgramming.Model;

public abstract class DocumentoBase
{
    public required string Titulo { get; set; }
    public DateTime DataCriacao { get; set; }

    public virtual bool Validar()
    {
        return !string.IsNullOrWhiteSpace(Titulo)
            && Titulo.Trim().Length >= 2
            && DataCriacao != default
            && DataCriacao <= DateTime.Now;
    }
}