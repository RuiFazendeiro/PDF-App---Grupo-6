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
        var erros = pessoa.Validar();
        Assert.Empty(erros);
    }

    // --- Nome ---

    [Fact]
    public void Validar_SemNome_DeveRetornarErro()
    {
        var pessoa = CriarPessoaValida();
        pessoa.Nome = "";

        var erros = pessoa.Validar();

        Assert.NotEmpty(erros);
    }

    [Fact]
    public void Validar_NomeComNumeros_DeveRetornarErroEspecifico()
    {
        var pessoa = CriarPessoaValida();
        pessoa.Nome = "Andreia123";

        var erros = pessoa.Validar();

        Assert.Contains("O nome não pode conter números.", erros);
    }

    [Fact]
    public void Validar_NomeComNumeros_DeveRetornarFalse()
    {
        var pessoa = CriarPessoaValida();
        pessoa.Nome = "Andreia123";
        var erros = pessoa.Validar();
        Assert.NotEmpty(erros);
    }

    // --- Email ---

    [Fact]
    public void Validar_EmailSemArroba_DeveRetornarFalse()
    {
        var pessoa = CriarPessoaValida();
        pessoa.Email = "andreiaemail.com";
        var erros = pessoa.Validar();
        Assert.NotEmpty(erros);
    }

    [Fact]
    public void Validar_EmailSemDominio_DeveRetornarFalse()
    {
        var pessoa = CriarPessoaValida();
        pessoa.Email = "andreia@";
        var erros = pessoa.Validar();
        Assert.NotEmpty(erros);
    }

    [Fact]
    public void Validar_EmailVazio_DeveRetornarFalse()
    {
        var pessoa = CriarPessoaValida();
        pessoa.Email = "";
        var erros = pessoa.Validar();
        Assert.NotEmpty(erros);
    }

    [Fact]
    public void Validar_EmailComEspacos_DeveRetornarFalse()
    {
        var pessoa = CriarPessoaValida();
        pessoa.Email = "   ";
        var erros = pessoa.Validar();
        Assert.NotEmpty(erros);
    }
}