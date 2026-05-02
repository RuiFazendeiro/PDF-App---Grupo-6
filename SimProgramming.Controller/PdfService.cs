using System.IO;
using System;
using System.Runtime.InteropServices;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using SimProgramming.Controller.Exceptions;
using SimProgramming.Controller.Interfaces;
using SimProgramming.Model;

namespace SimProgramming.Controller;

public class PdfService : IPdfService
{
    private static bool _fontesConfiguradas;

    public void GerarDocumento(DocumentoBase documento, string caminhoArquivo)
    {
        if (documento == null) throw new ArgumentNullException(nameof(documento));
        if (string.IsNullOrWhiteSpace(caminhoArquivo)) throw new ArgumentException("Caminho inválido.", nameof(caminhoArquivo));

        var erros = documento.Validar();

        if (erros.Any())
        {
            throw new DocumentValidationException(string.Join(" | ", erros));
        }

        try
        {
            ConfigurarFontes();

            using var pdf = new PdfDocument();
            pdf.Info.Title = documento.Titulo;
            pdf.Info.Author = "SimProgramming - Equipa 6";

            if (documento is Certificado cert)
            {
                GerarCertificado(pdf, cert);
            }
            else if (documento is Relatorio rel)
            {
                GerarRelatorio(pdf, rel);
            }
            else
            {
                throw new PdfGenerationException("Tipo de documento não suportado pelo serviço.");
            }

            pdf.Save(caminhoArquivo);
        }
        catch (IOException ex)
        {
            throw new PdfGenerationException("Falha ao gravar o ficheiro PDF. Pode estar aberto noutro programa.", ex);
        }
        catch (Exception ex) when (ex is not PdfGenerationException)
        {
            throw new PdfGenerationException("Erro inesperado ao gerar o PDF.", ex);
        }
    }

    private static void GerarCertificado(PdfDocument pdf, Certificado cert)
    {
        var pagina = pdf.AddPage();
        pagina.Size = PageSize.A4;
        using var gfx = XGraphics.FromPdfPage(pagina);

        var azul = XColor.FromArgb(35, 75, 145);
        var fonteTitulo = new XFont("Arial", 24, XFontStyleEx.Bold);
        var fonteNome = new XFont("Arial", 22, XFontStyleEx.Bold);
        var fonteNormal = new XFont("Arial", 12, XFontStyleEx.Regular);

        // Moldura
        gfx.DrawRectangle(new XPen(azul, 2), 40, 40, pagina.Width.Point - 80, pagina.Height.Point - 80);

        // Textos Centrais
        gfx.DrawString(cert.Titulo, fonteTitulo, new XSolidBrush(azul), new XRect(0, 100, pagina.Width.Point, 40), XStringFormats.Center);
        gfx.DrawString("Certifica-se que", fonteNormal, XBrushes.Black, new XRect(0, 180, pagina.Width.Point, 30), XStringFormats.Center);
        gfx.DrawString(cert.NomeFormando, fonteNome, new XSolidBrush(azul), new XRect(0, 230, pagina.Width.Point, 40), XStringFormats.Center);
        gfx.DrawString($"concluiu com sucesso o curso de {cert.Curso}.", fonteNormal, XBrushes.Black, new XRect(0, 300, pagina.Width.Point, 40), XStringFormats.Center);
        
        // Rodapé fiel ao PR do Frederico
        gfx.DrawString($"Entidade emissora: {cert.EntidadeEmissora}", fonteNormal, XBrushes.Black, 70, 630);
        gfx.DrawString($"Data de emissão: {cert.DataEmissao:dd/MM/yyyy}", fonteNormal, XBrushes.Black, 70, 650);
        
        // Linha de assinatura
        gfx.DrawLine(new XPen(azul, 1), pagina.Width.Point - 240, 700, pagina.Width.Point - 70, 700);
        gfx.DrawString("Assinatura", fonteNormal, XBrushes.Black, pagina.Width.Point - 185, 720);
    }

    private static void GerarRelatorio(PdfDocument pdf, Relatorio rel)
    {
        var pagina = pdf.AddPage();
        pagina.Size = PageSize.A4;
        using var gfx = XGraphics.FromPdfPage(pagina);

        var fonteTitulo = new XFont("Arial", 20, XFontStyleEx.Bold);
        var fonteNormal = new XFont("Arial", 12, XFontStyleEx.Regular);

        gfx.DrawString(rel.Titulo, fonteTitulo, XBrushes.Black, 50, 50);
        gfx.DrawString($"Autor: {rel.Autor}", fonteNormal, XBrushes.Black, 50, 100);
        gfx.DrawString(rel.Conteudo, fonteNormal, XBrushes.Black, 50, 150);
    }

    private static void ConfigurarFontes()
    {
        if (_fontesConfiguradas) return;
        
        // Agora aplicamos sempre o resolver, independentemente do OS
        if (GlobalFontSettings.FontResolver == null)
        {
            GlobalFontSettings.FontResolver = new UniversalFontResolver();
        }
        
        _fontesConfiguradas = true;
    }
}

// Classe Universal que sabe encontrar a fonte Arial tanto no Windows como no Linux
public class UniversalFontResolver : IFontResolver
{
    public byte[] GetFont(string faceName)
    {
        bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        string fontPath = string.Empty;

        if (isWindows)
        {
            // Caminho nativo das fontes no Windows
            fontPath = faceName == "ArialBold" 
                ? @"C:\Windows\Fonts\arialbd.ttf" 
                : @"C:\Windows\Fonts\arial.ttf";
        }
        else
        {
            // Caminho das fontes no Linux / Codespaces
            fontPath = faceName == "ArialBold" 
                ? "/usr/share/fonts/truetype/msttcorefonts/arialbd.ttf" 
                : "/usr/share/fonts/truetype/msttcorefonts/arial.ttf";
        }

        if (!File.Exists(fontPath))
            throw new FileNotFoundException($"A fonte não foi encontrada no caminho: {fontPath}. Se estiver no Linux, instale as fontes msttcorefonts.");
            
        return File.ReadAllBytes(fontPath);
    }

    public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        // Forçamos o mapeamento apenas para a Arial (normal ou negrito)
        return new FontResolverInfo(isBold ? "ArialBold" : "Arial");
    }
}