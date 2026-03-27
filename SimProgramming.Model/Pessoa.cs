
namespace SimProgramming.Model;
public class Pessoa
{
    public required string Nome { get; set; }
    public required string Email { get; set; }

    public bool Validate()
    {
        return !string.IsNullOrWhiteSpace(Nome);
    }
}