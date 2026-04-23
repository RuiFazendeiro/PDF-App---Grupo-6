#region Usings

using SimProgramming.Controller.Exceptions;
using SimProgramming.Controller.Interfaces;
using SimProgramming.Model;

#endregion

namespace SimProgramming.Controller;

public class PdfService : IPdfService
{
    /// <summary>
    /// Implementação simples que valida o documento e escreve um ficheiro de texto.
    /// Exceções de validação e I/O são encapsuladas em tipos específicos.
    /// </summary>
    public void GerarDocumento(DocumentoBase documento, string caminhoArquivo)
    {
        if (documento is null) throw new ArgumentNullException(nameof(documento));
        if (string.IsNullOrWhiteSpace(caminhoArquivo)) throw new ArgumentException("Caminho do ficheiro inválido.", nameof(caminhoArquivo));

        // Validação de negócio
        if (!documento.Validar())
        {
            throw new DocumentValidationException("Dados do documento inválidos. Verifique os campos obrigatórios.");
        }

        var dir = Path.GetDirectoryName(caminhoArquivo);
        if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);

        try
        {
            using var writer = new StreamWriter(caminhoArquivo, false);
            writer.WriteLine($"Documento: {documento.GetType().Name}");
            writer.WriteLine($"DataCriação: {documento.DataCriacao:yyyy-MM-dd HH:mm}");

            if (documento is Certificado cert)
            {
                writer.WriteLine($"NomeFormando: {cert.NomeFormando}");
                writer.WriteLine($"Curso: {cert.Curso}");
                writer.WriteLine($"EntidadeEmissora: {cert.EntidadeEmissora}");
                writer.WriteLine($"DataEmissao: {cert.DataEmissao:yyyy-MM-dd}");
            }
            else if (documento is Relatorio rel)
            {
                writer.WriteLine($"Autor: {rel.Autor}");
                writer.WriteLine();
                writer.WriteLine("Conteudo:");
                writer.WriteLine(rel.Conteudo);
            }
            else
            {
                writer.WriteLine("Tipo de documento não suportado pelo serviço.");
            }
        }
        catch (IOException ex)
        {
            throw new PdfGenerationException("Falha ao gravar o ficheiro do documento.", ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new PdfGenerationException("Acesso negado ao caminho do ficheiro.", ex);
        }
        catch (Exception ex)
        {
            throw new PdfGenerationException("Erro inesperado ao gerar o documento.", ex);
        }
    }
}

