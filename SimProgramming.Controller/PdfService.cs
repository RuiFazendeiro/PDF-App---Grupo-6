using SimProgramming.Controller.Interfaces;
using SimProgramming.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimProgramming.Controller;

public class PdfService : IPdfService
{
    /// <summary>
    /// Transpõe um modelo de dados para um ficheiro PDF.
    /// </summary>
    /// <param name="documento">O objeto (Certificado ou Relatório) a processar.</param>
    /// <param name="caminhoArquivo">O destino no sistema de ficheiros.</param>
    public void GerarDocumento(DocumentoBase documento, string caminhoArquivo)
    {

    }
}

