namespace SimProgramming.Model;

public abstract class DocumentoBase
{
    public required string Titulo { get; set; }
    public DateTime DataCriacao { get; set; }

    public virtual List<string> Validar()
    {
        var erros = new List<string>();

        if (string.IsNullOrWhiteSpace(Titulo))
            erros.Add("O título é obrigatório.");
        else if (Titulo.Trim().Length < 2)
            erros.Add("O título deve ter pelo menos 2 caracteres.");

        if (DataCriacao == default)
            erros.Add("A data de criação é obrigatória.");
        else if (DataCriacao > DateTime.Now)
            erros.Add("A data de criação não pode ser no futuro.");

        return erros;
    }
}