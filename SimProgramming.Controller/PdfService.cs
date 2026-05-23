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

        try
        {
            using var fileStream = new FileStream(caminhoArquivo, FileMode.Create, FileAccess.Write);
            GerarDocumento(documento, fileStream);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new PdfGenerationException($"Permissão negada ao gravar o ficheiro PDF em '{caminhoArquivo}'. Verifique as permissões de escrita no diretório.", ex);
        }
        catch (DirectoryNotFoundException ex)
        {
            throw new PdfGenerationException($"O diretório especificado não existe: '{Path.GetDirectoryName(caminhoArquivo)}'. Crie o diretório antes de tentar gerar o PDF.", ex);
        }
        catch (NotSupportedException ex)
        {
            throw new PdfGenerationException($"O caminho do ficheiro não é suportado: '{caminhoArquivo}'. Verifique o formato do caminho.", ex);
        }
        catch (IOException ex)
        {
            // Captura erros como disco cheio, ficheiro em uso por outro programa, etc.
            string mensagem = "Falha ao gravar o ficheiro PDF.";
            if (ex.Message.Contains("disco"))
                mensagem += " Possível falta de espaço em disco.";
            else if (ex.Message.Contains("use"))
                mensagem += " O ficheiro pode estar aberto noutro programa.";
            else
                mensagem += " Verifique se o caminho é válido e se tem permissão de escrita.";
            throw new PdfGenerationException(mensagem, ex);
        }
        catch (ArgumentException ex)
        {
            throw new PdfGenerationException($"Caminho do ficheiro inválido: '{caminhoArquivo}'.", ex);
        }
    }

    public void GerarDocumento(DocumentoBase documento, Stream stream)
    {
        if (documento == null) throw new ArgumentNullException(nameof(documento));
        if (stream == null) throw new ArgumentNullException(nameof(stream));

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

            pdf.Save(stream);
        }
        catch (OutOfMemoryException ex)
        {
            throw new PdfGenerationException("Memória insuficiente para gerar o PDF. A estrutura do documento pode ser demasiado complexa ou grande.", ex);
        }
        catch (NotSupportedException ex)
        {
            throw new PdfGenerationException("Operação não suportada ao gerar o PDF. Verifique o formato do stream ou tipo de documento.", ex);
        }
        catch (IOException ex)
        {
            throw new PdfGenerationException("Erro de I/O ao gravar o PDF. Verifique se o stream é gravável e se há espaço disponível.", ex);
        }
        catch (Exception ex) when (ex is not PdfGenerationException && ex is not DocumentValidationException)
        {
            throw new PdfGenerationException("Erro inesperado ao gerar o PDF.", ex);
        }
    }

    private static void DesenharRodapeCertificado(XGraphics gfx, PdfPage pagina, Certificado cert)
    {
        var fontePequena = new XFont("Arial", 9, XFontStyleEx.Regular);

        // Número de série (gerado a partir de hash do documento)
        string numeroSerie = $"CERT-{DateTime.Now:yyyyMMdd}-{cert.NomeFormando.GetHashCode() % 10000:D4}";
        gfx.DrawString($"Número de série: {numeroSerie}", fontePequena, XBrushes.Gray, 50, pagina.Height.Point - 20);
    }

    private static void DesenharCabecalhoCertificado(XGraphics gfx, PdfPage pagina)
    {
        var azul = XColor.FromArgb(35, 75, 145);
        var fontePequena = new XFont("Arial", 10, XFontStyleEx.Regular);

        // Cabeçalho com branding SimProgramming
        gfx.DrawString("SimProgramming - Equipa 6", fontePequena, new XSolidBrush(azul), 50, 10);
        gfx.DrawString($"Emitido em: {DateTime.Now:dd/MM/yyyy}", fontePequena, XBrushes.Gray, pagina.Width.Point - 200, 10);
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

        // Desenhar cabeçalho
        DesenharCabecalhoCertificado(gfx, pagina);

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

        // Desenhar rodapé padronizado
        DesenharRodapeCertificado(gfx, pagina, cert);
    }

    private static void DesenharRodapeRelatorio(XGraphics gfx, PdfPage pagina, Relatorio rel, int numeroPagina, int totalPaginas)
    {
        var fontePequena = new XFont("Arial", 9, XFontStyleEx.Regular);

        // Informações do rodapé
        gfx.DrawString($"Autor: {rel.Autor}", fontePequena, XBrushes.Gray, 50, pagina.Height.Point - 15);
        gfx.DrawString($"Página {numeroPagina} de {totalPaginas}", fontePequena, XBrushes.Gray, pagina.Width.Point / 2 - 30, pagina.Height.Point - 15);
    }

    private static void DesenharCabecalhoRelatorio(XGraphics gfx, PdfPage pagina, Relatorio rel)
    {
        var azul = XColor.FromArgb(35, 75, 145);
        var fontePequena = new XFont("Arial", 10, XFontStyleEx.Regular);

        // Cabeçalho com título do documento e data
        gfx.DrawString($"Relatório: {rel.Titulo}", fontePequena, new XSolidBrush(azul), 50, 15);
        gfx.DrawString($"Data: {rel.DataCriacao:dd/MM/yyyy}", fontePequena, XBrushes.Gray, pagina.Width.Point - 150, 15);
    }

    private static void GerarRelatorio(PdfDocument pdf, Relatorio rel)
    {
        var pagina = pdf.AddPage();
        pagina.Size = PageSize.A4;
        using var gfx = XGraphics.FromPdfPage(pagina);

        var fonteTitulo = new XFont("Arial", 20, XFontStyleEx.Bold);
        var fonteNormal = new XFont("Arial", 12, XFontStyleEx.Regular);

        // Desenhar cabeçalho
        DesenharCabecalhoRelatorio(gfx, pagina, rel);

        // Conteúdo principal com margens respeitando cabeçalho e rodapé
        gfx.DrawString(rel.Titulo, fonteTitulo, XBrushes.Black, 50, 55);
        gfx.DrawString($"Autor: {rel.Autor}", fonteNormal, XBrushes.Black, 50, 100);
        gfx.DrawString(rel.Conteudo, fonteNormal, XBrushes.Black, 50, 150);

        // Desenhar rodapé padronizado
        DesenharRodapeRelatorio(gfx, pagina, rel, 1, 1);
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

        try
        {
            if (!File.Exists(fontPath))
                throw new FileNotFoundException($"A fonte não foi encontrada no caminho: {fontPath}. Se estiver no Linux, instale as fontes msttcorefonts.");

            return File.ReadAllBytes(fontPath);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new PdfGenerationException($"Permissão negada ao aceder à fonte em '{fontPath}'. Verifique as permissões de leitura.", ex);
        }
        catch (FileNotFoundException ex)
        {
            throw new PdfGenerationException(ex.Message, ex);
        }
        catch (DirectoryNotFoundException ex)
        {
            throw new PdfGenerationException($"O diretório das fontes não existe: '{Path.GetDirectoryName(fontPath)}'.", ex);
        }
        catch (IOException ex)
        {
            throw new PdfGenerationException($"Erro ao ler o ficheiro de fonte: '{fontPath}'. O ficheiro pode estar corrompido ou em uso.", ex);
        }
        catch (NotSupportedException ex)
        {
            throw new PdfGenerationException($"Caminho de fonte não suportado: '{fontPath}'.", ex);
        }
    }

    public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        // Forçamos o mapeamento apenas para a Arial (normal ou negrito)
        return new FontResolverInfo(isBold ? "ArialBold" : "Arial");
    }
}