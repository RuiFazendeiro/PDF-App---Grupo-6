#region Usings

using SimProgramming.Controller;
using SimProgramming.Controller.Exceptions;
using SimProgramming.Model;
#endregion

namespace SimProgramming.Tests
{
    public class PdfServiceTests
    {
        private readonly PdfService _service;

        public PdfServiceTests()
        {
            _service = new PdfService();
        }

        [Fact]
        public void GerarDocumento_ValidCertificado_CreatesFile()
        {
            var cert = new Certificado
            {
                Titulo = "Titulo",
                DataCriacao = DateTime.Now.AddMinutes(-1),
                NomeFormando = "Joao",
                Curso = "C# Básico",
                EntidadeEmissora = "SimProgramming",
                DataEmissao = DateTime.Now
            };

            using var memoryStream = new MemoryStream();
            _service.GerarDocumento(cert, memoryStream);

            Assert.True(memoryStream.Length > 0);
        }

        [Fact]
        public void GerarDocumento_InvalidDocument_ThrowsArgumentExceptionOrValidation()
        {
            var cert = new Certificado
            {
                Titulo = "",
                DataCriacao = default,
                NomeFormando = "",
                Curso = "",
                EntidadeEmissora = "",
                DataEmissao = default
            };

            using var memoryStream = new MemoryStream();
            Assert.Throws<DocumentValidationException>(() => _service.GerarDocumento(cert, memoryStream));
        }

        [Fact]
        public void GerarDocumento_ValidRelatorio_CreatesFile()
        {
            var relatorio = new Relatorio
            {
                Titulo = "Relatório de Testes",
                DataCriacao = DateTime.Now.AddHours(-2),
                Autor = "Andreia Correia",
                Conteudo = "Este é um conteúdo de teste para o relatório com mais de 10 caracteres."
            };

            using var memoryStream = new MemoryStream();
            _service.GerarDocumento(relatorio, memoryStream);

            Assert.True(memoryStream.Length > 0);
        }

        [Fact]
        public void GerarDocumento_ValidCertificado_WithFileStream_CreatesFile()
        {
            var cert = new Certificado
            {
                Titulo = "Certificado Teste",
                DataCriacao = DateTime.Now.AddDays(-1),
                NomeFormando = "Maria Santos",
                Curso = "Python Avançado",
                EntidadeEmissora = "SimProgramming",
                DataEmissao = DateTime.Now
            };

            // Teste com FileStream para garantir compatibilidade
            string tempFile = Path.Combine(Path.GetTempPath(), $"test_cert_{Guid.NewGuid()}.pdf");
            try
            {
                _service.GerarDocumento(cert, tempFile);
                Assert.True(File.Exists(tempFile));
                Assert.True(new FileInfo(tempFile).Length > 0);
            }
            finally
            {
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }
    }
}

