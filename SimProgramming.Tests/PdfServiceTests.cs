#region Usings

using SimProgramming.Controller;
using SimProgramming.Controller.Exceptions;
using SimProgramming.Model;
#endregion

namespace SimProgramming.Tests
{
    public class PdfServiceTests : IDisposable
    {
        private readonly string _tempFile;
        private readonly PdfService _service;

        public PdfServiceTests()
        {
            _tempFile = Path.Combine(Path.GetTempPath(), $"test_doc_{Guid.NewGuid()}.txt");
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

            _service.GerarDocumento(cert, _tempFile);

            Assert.True(File.Exists(_tempFile));
            var content = File.ReadAllText(_tempFile);
            Assert.Contains("NomeFormando: Joao", content);
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

            Assert.Throws<DocumentValidationException>(() => _service.GerarDocumento(cert, _tempFile));
        }

        public void Dispose()
        {
            try { if (File.Exists(_tempFile)) File.Delete(_tempFile); } catch { }
        }
    }
}
