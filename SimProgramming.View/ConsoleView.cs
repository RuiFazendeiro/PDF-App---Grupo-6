using System;

namespace SimProgramming.View
{
    /* View Passiva (modelo Curry & Grace)
       Responsável apenas por I/O de texto (Input/Output).
       Não tem qualquer lógica de decisão, validação de negócio ou processamento.*/

    public class ConsoleView
    {
        #region Métodos de Output

        // Exibe qualquer mensagem
        public void ExibirMensagem(string mensagem)
        {
            Console.WriteLine(mensagem);
        }

        // Limpa o ecrã
        public void LimparConsola()
        {
            Console.Clear();
        }

        // Exibe mensagem de sucesso quando o PDF é gerado.
        public void ExibirDocumentoGerado(string caminhoPdf)
        {
            ExibirMensagem($"\nDocumento PDF gerado com sucesso!");
            ExibirMensagem($"Localização: {caminhoPdf}");
            ExibirMensagem("Prima qualquer tecla para continuar...");
            Console.ReadKey();
        }

        #endregion

        #region Métodos de Input

        // Método privado para normalizar o input do utilizador
        private string ObterInputNormalizado(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }

        public string ObterTitulo()
        {
            return ObterInputNormalizado("Insira o título do documento: ");
        }

        public string ObterNomeUtilizador()
        {
            return ObterInputNormalizado("Insira o nome do utilizador: ");
        }

        // Captura a Data -- Se o utilizador inserir data inválida, pede novamente
        public DateTime ObterData()
        {
            while (true)
            {
                string input = ObterInputNormalizado("Insira a data do documento (dd/mm/aaaa): ");
                if (DateTime.TryParse(input, out DateTime data))
                {
                    return data;
                }
                ExibirMensagem("Data inválida. Por favor, tente novamente.");
            }
        }

        // Captura o Conteúdo
        public string ObterConteudo()
        {
            ExibirMensagem("Insira o conteúdo do documento (prima ENTER para terminar):");
            return ObterInputNormalizado("");
        }

        // Campo específico do Relatorio (herda de Relatorio.cs)
        public string ObterAutor()
        {
            return ObterInputNormalizado("Insira o autor do relatório: ");
        }

        // Campos específicos do Certificado (herda de Certificado.cs)
        public string ObterNomeFormando()
        {
            return ObterInputNormalizado("Insira o nome do formando: ");
        }

        public string ObterCurso()
        {
            return ObterInputNormalizado("Insira o nome do curso: ");
        }

        // Captura a Data -- Se o utilizador inserir data inválida, pede novamente
        public DateTime ObterDataEmissao()
        {
            while (true)
            {
                string input = ObterInputNormalizado("Insira a data de emissão (dd/mm/aaaa): ");
                if (DateTime.TryParse(input, out DateTime data))
                {
                    return data;
                }
                ExibirMensagem("Data inválida. Por favor, tente novamente.");
            }
        }

        public string ObterEntidadeEmissora()
        {
            return ObterInputNormalizado("Insira a entidade emissora: ");
        }

        #endregion

        #region Menu

        // Mostra o menu principal e devolve a opção escolhida
        // O Controller decide o que fazer com a opção escolhida
        public int ObterOpcaoMenu()
        {
            ExibirMensagem("\n=== MENU ===");
            ExibirMensagem("1. Gerar Relatório");
            ExibirMensagem("2. Gerar Certificado");
            ExibirMensagem("3. Limpar / Reiniciar");
            ExibirMensagem("0. Sair");
            ExibirMensagem("====================");

            string input = ObterInputNormalizado("Escolha uma opção: ");

            if (int.TryParse(input, out int opcao))
                return opcao;

            return -1; // valor inválido (Controller trata)
        }

        #endregion
    }
}