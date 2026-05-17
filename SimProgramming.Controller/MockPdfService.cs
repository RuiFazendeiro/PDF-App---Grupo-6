using System;
using System.IO;
using System.Text;
using SimProgramming.Controller.Interfaces;
using SimProgramming.Model.Models;

namespace SimProgramming.Controller;

public class MockPdfService : IPdfService
{
    public delegate void FluxoExperimentalHandler(string titulo);
    public event FluxoExperimentalHandler AoProcessar;

    public void GerarDocumento(DocumentoBase documento, string caminhoArquivo)
    {
        using var fileStream = new FileStream(caminhoArquivo, FileMode.Create, FileAccess.Write);
        GerarDocumento(documento, fileStream);
    }

    public void GerarDocumento(DocumentoBase documento, Stream stream)
    {
        if (documento == null) throw new ArgumentNullException(nameof(documento));
        if (stream == null) throw new ArgumentNullException(nameof(stream));

        // Acoplamento fraco: Dispara evento intermédio antes de "desenhar"
        AoProcessar?.Invoke(documento.Titulo);

        // Escreve dados mockados usando a assinatura de Stream da Andreia
        string mockData = $"%PDF-1.4_EXPERIMENTAL_TEAM6\n/Modulo (Caso Experimental)\n/Doc ({documento.Titulo})\n%%EOF";
        byte[] buffer = Encoding.UTF8.GetBytes(mockData);
        stream.Write(buffer, 0, buffer.Length);
    }
}