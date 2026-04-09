using SimProgramming.Model;

namespace SimProgramming.Tests;

public class PessoaTests
{
    private Pessoa CriarPessoaValida() => new Pessoa
    {
        Nome = "Andreia Correia",
        Email = "andreia@email.com"
    };

    [Fact]
    public void Validar_PessoaCompleta_DeveRetornarTrue()
    {
        var pessoa = CriarPessoaValida();
        Assert.True(pessoa.Validar());
    }

    // --- Nome ---

    [Fact]
    public void Validar_SemNome_DeveRetornarFalse()
    {
        var pessoa = CriarPessoaValida();
        pessoa.Nome = "";
        Assert.False(pessoa.Validar());
    }

    [Fact]
    public void Validar_NomeMuitoCurto_DeveRetornarFalse()
    {
        var pessoa = CriarPessoaValida();
        pessoa.Nome = "A";
        Assert.False(pessoa.Validar());
    }

    [Fact]
    public void Validar_NomeComNumeros_DeveRetornarFalse()
    {
        var pessoa = CriarPessoaValida();
        pessoa.Nome = "Andreia123";
        Assert.False(pessoa.Validar());
    }

    // --- Email ---

    [Fact]
    public void Validar_EmailSemArroba_DeveRetornarFalse()
    {
        var pessoa = CriarPessoaValida();
        pessoa.Email = "andreiaemail.com";
        Assert.False(pessoa.Validar());
    }

    [Fact]
    public void Validar_EmailSemDominio_DeveRetornarFalse()
    {
        var pessoa = CriarPessoaValida();
        pessoa.Email = "andreia@";
        Assert.False(pessoa.Validar());
    }

    [Fact]
    public void Validar_EmailVazio_DeveRetornarFalse()
    {
        var pessoa = CriarPessoaValida();
        pessoa.Email = "";
        Assert.False(pessoa.Validar());
    }

    [Fact]
    public void Validar_EmailComEspacos_DeveRetornarFalse()
    {
        var pessoa = CriarPessoaValida();
        pessoa.Email = "   ";
        Assert.False(pessoa.Validar());
    }
}