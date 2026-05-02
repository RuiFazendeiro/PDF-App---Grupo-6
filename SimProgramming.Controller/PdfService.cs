#region Usings

using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using SimProgramming.Controller.Exceptions;
using SimProgramming.Controller.Interfaces;
using SimProgramming.Model;
using System.Drawing;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

#endregion

namespace SimProgramming.Controller;

public class PdfService : IPdfService
{
    private static bool _fontesConfiguradas;

    public void GerarDocumento(DocumentoBase documento, string caminhoArquivo)
    {
        if (documento is null) throw new ArgumentNullException(nameof(documento));
        if (string.IsNullOrWhiteSpace(caminhoArquivo)) throw new ArgumentException("Caminho do ficheiro inválido.", nameof(caminhoArquivo));

        if (!documento.Validar())
        {
            throw new DocumentValidationException("Dados do documento inválidos. Verifique os campos obrigatórios.");
        }

        var dir = Path.GetDirectoryName(caminhoArquivo);
        if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);

        try
        {
            ConfigurarFontes();

            using var pdf = new PdfDocument();

            pdf.Info.Title = documento.Titulo;
            pdf.Info.Author = "SimProgramming";
            pdf.Info.Subject = documento.GetType().Name;

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
            throw new PdfGenerationException("Falha ao gravar o ficheiro PDF.", ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new PdfGenerationException("Acesso negado ao caminho do ficheiro.", ex);
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

        var largura = pagina.Width.Point;
        var azul = XColor.FromArgb(35, 75, 145);

        var fonteTitulo = new XFont("Arial", 24, XFontStyleEx.Bold);
        var fonteSubtitulo = new XFont("Arial", 15, XFontStyleEx.Regular);
        var fonteNome = new XFont("Arial", 22, XFontStyleEx.Bold);
        var fonteNormal = new XFont("Arial", 12, XFontStyleEx.Regular);

        gfx.DrawRectangle(new XPen(azul, 2), 40, 40, largura - 80, pagina.Height.Point - 80);

        gfx.DrawString(cert.Titulo, fonteTitulo, new XSolidBrush(azul),
            new XRect(60, 90, largura - 120, 40), XStringFormats.Center);

        gfx.DrawString("Certifica-se que", fonteSubtitulo, XBrushes.Black,
            new XRect(60, 180, largura - 120, 30), XStringFormats.Center);

        gfx.DrawString(cert.NomeFormando, fonteNome, new XSolidBrush(azul),
            new XRect(60, 240, largura - 120, 40), XStringFormats.Center);

        gfx.DrawString($"concluiu com sucesso o curso de {cert.Curso}.", fonteSubtitulo, XBrushes.Black,
            new XRect(70, 315, largura - 140, 40), XStringFormats.Center);

        gfx.DrawString($"Entidade emissora: {cert.EntidadeEmissora}", fonteNormal, XBrushes.Black, 70, 630);
        gfx.DrawString($"Data de emissão: {cert.DataEmissao:dd/MM/yyyy}", fonteNormal, XBrushes.Black, 70, 655);

        gfx.DrawLine(new XPen(azul, 1), largura - 240, 700, largura - 70, 700);
        gfx.DrawString("Assinatura", fonteNormal, XBrushes.Black, largura - 185, 720);
    }

    private static void GerarRelatorio(PdfDocument pdf, Relatorio rel)
    {
        var pagina = pdf.AddPage();
        pagina.Size = PageSize.A4;

        using var gfx = XGraphics.FromPdfPage(pagina);

        var fonteTitulo = new XFont("Arial", 20, XFontStyleEx.Bold);
        var fonteNormal = new XFont("Arial", 12, XFontStyleEx.Regular);
        var fontePequena = new XFont("Arial", 10, XFontStyleEx.Regular);

        gfx.DrawString(rel.Titulo, fonteTitulo, XBrushes.Black,
            new XRect(50, 50, pagina.Width.Point - 100, 40), XStringFormats.TopLeft);

        gfx.DrawString($"Autor: {rel.Autor}", fonteNormal, XBrushes.Black, 50, 110);
        gfx.DrawString($"Data de criação: {rel.DataCriacao:dd/MM/yyyy HH:mm}", fontePequena, XBrushes.Gray, 50, 135);

        DesenharTextoQuebrado(gfx, rel.Conteudo, fonteNormal, XBrushes.Black, 50, 180, pagina.Width.Point - 100, 18);
    }

    private static void DesenharTextoQuebrado(XGraphics gfx, string texto, XFont fonte, XBrush brush, double x, double y, double larguraMaxima, double alturaLinha)
    {
        var palavras = texto.Split(' ');
        var linha = string.Empty;

        foreach (var palavra in palavras)
        {
            var teste = string.IsNullOrEmpty(linha) ? palavra : $"{linha} {palavra}";

            if (gfx.MeasureString(teste, fonte).Width > larguraMaxima)
            {
                gfx.DrawString(linha, fonte, brush, x, y);
                linha = palavra;
                y += alturaLinha;
            }
            else
            {
                linha = teste;
            }
        }

        if (!string.IsNullOrWhiteSpace(linha))
        {
            gfx.DrawString(linha, fonte, brush, x, y);
        }
    }

    private static void ConfigurarFontes()
    {
        if (_fontesConfiguradas)
            return;

        if (Capabilities.Build.IsCoreBuild)
            GlobalFontSettings.UseWindowsFontsUnderWindows = true;

        _fontesConfiguradas = true;
    }
}

