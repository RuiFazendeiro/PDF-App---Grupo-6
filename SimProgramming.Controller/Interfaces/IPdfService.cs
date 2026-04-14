namespace SimProgramming.Controller.Interfaces;

using SimProgramming.Model;

/// <summary>
/// Interface que define o contrato para a geração de PDFs.
/// Segue o princípio da Inversão de Dependência para garantir baixo acoplamento.
/// </summary>
public interface IPdfService
{
    /// <summary>
    /// Transpõe um modelo de dados para um ficheiro PDF.
    /// </summary>
    /// <param name="documento">O objeto (Certificado ou Relatório) a processar.</param>
    /// <param name="caminhoArquivo">O destino no sistema de ficheiros.</param>
    void GerarDocumento(DocumentoBase documento, string caminhoArquivo);
}