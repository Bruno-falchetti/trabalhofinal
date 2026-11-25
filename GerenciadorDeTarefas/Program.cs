using System;
using System.Collections.Generic;
using System.Text;

namespace GerenciadorDeTarefas
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            GerenciadorTarefas gerenciador = new GerenciadorTarefas();
            int opcao = 0;

            do
            {
                ExibirMenu();
                string entradaMenu = Console.ReadLine();

                if (!int.TryParse(entradaMenu, out opcao))
                {
                    EscreverMensagem("\nOpção inválida! Digite um número inteiro entre 1 e 6.", ConsoleColor.Red);
                    continue;
                }

                Console.WriteLine();

                switch (opcao)
                {
                    case 1:
                        ExecutarCadastro(gerenciador);
                        break;
                    case 2:
                        ExecutarListagem(gerenciador);
                        break;
                    case 3:
                        ExecutarBusca(gerenciador);
                        break;
                    case 4:
                        ExecutarConclusao(gerenciador);
                        break;
                    case 5:
                        ExecutarDesfazer(gerenciador);
                        break;
                    case 6:
                        EscreverMensagem("Encerrando o Gerenciador de Tarefas. Até logo!", ConsoleColor.Cyan);
                        break;
                    default:
                        EscreverMensagem("Opção inexistente! Escolha uma opção de 1 a 6.", ConsoleColor.Red);
                        break;
                }

            } while (opcao != 6);
        }

        private static void ExibirMenu()
        {
            Console.WriteLine("\n==========================================");
            Console.WriteLine("      GERENCIADOR DE TAREFAS (C#)         ");
            Console.WriteLine("==========================================");
            Console.WriteLine("1 - Cadastrar tarefa");
            Console.WriteLine("2 - Listar tarefas");
            Console.WriteLine("3 - Buscar tarefa");
            Console.WriteLine("4 - Concluir tarefa");
            Console.WriteLine("5 - Desfazer a última conclusão");
            Console.WriteLine("6 - Sair");
            Console.Write("Escolha uma opção: ");
        }

        private static void ExecutarCadastro(GerenciadorTarefas gerenciador)
        {
            Console.Write("Digite a descrição da tarefa: ");
            string descricao = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(descricao))
            {
                EscreverMensagem("Erro: A descrição da tarefa não pode ser vazia.", ConsoleColor.Red);
                return;
            }

            Tarefa criada = gerenciador.CadastrarTarefa(descricao.Trim());
            EscreverMensagem($"Tarefa cadastrada com sucesso! ID: #{criada.Id} | Situação: {FormatarSituacao(criada.Situacao)}", ConsoleColor.Green);
        }

        private static void ExecutarListagem(GerenciadorTarefas gerenciador)
        {
            List<Tarefa> tarefas = gerenciador.ListarTarefas();

            if (tarefas.Count == 0)
            {
                EscreverMensagem("A lista de tarefas está vazia.", ConsoleColor.Yellow);
                return;
            }

            Console.WriteLine("--- LISTA DE TAREFAS CADASTRADAS ---");
            foreach (Tarefa tarefa in tarefas)
            {
                ConsoleColor cor = tarefa.Situacao == SituacaoTarefaEnum.CONCLUIDA
                    ? ConsoleColor.Green
                    : ConsoleColor.White;

                Console.ForegroundColor = cor;
                Console.WriteLine($"ID: #{tarefa.Id} | Descrição: {tarefa.Descricao} | Situação: {FormatarSituacao(tarefa.Situacao)}");
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private static void ExecutarBusca(GerenciadorTarefas gerenciador)
        {
            if (!LerIdentificador("Digite o ID da tarefa que deseja buscar: ", out int id))
            {
                return;
            }

            Tarefa tarefa = gerenciador.BuscarTarefa(id);

            if (tarefa == null)
            {
                EscreverMensagem($"Nenhuma tarefa encontrada com o ID {id}.", ConsoleColor.Red);
            }
            else
            {
                EscreverMensagem($"Tarefa encontrada -> ID: #{tarefa.Id} | Descrição: {tarefa.Descricao} | Situação: {FormatarSituacao(tarefa.Situacao)}", ConsoleColor.White);
            }
        }

        private static void ExecutarConclusao(GerenciadorTarefas gerenciador)
        {
            if (!LerIdentificador("Digite o ID da tarefa que deseja concluir: ", out int id))
            {
                return;
            }

            bool sucesso = gerenciador.ConcluirTarefa(id, out string mensagem);
            EscreverMensagem(mensagem, sucesso ? ConsoleColor.Green : ConsoleColor.Red);
        }

        private static void ExecutarDesfazer(GerenciadorTarefas gerenciador)
        {
            bool sucesso = gerenciador.DesfazerUltimaConclusao(out string mensagem);
            EscreverMensagem(mensagem, sucesso ? ConsoleColor.Yellow : ConsoleColor.Red);
        }

        private static bool LerIdentificador(string prompt, out int id)
        {
            Console.Write(prompt);
            string entrada = Console.ReadLine();

            if (!int.TryParse(entrada, out id))
            {
                EscreverMensagem("Entrada inválida! O identificador deve ser um número inteiro.", ConsoleColor.Red);
                return false;
            }

            return true;
        }

        private static string FormatarSituacao(SituacaoTarefaEnum situacao)
        {
            return situacao == SituacaoTarefaEnum.CONCLUIDA ? "Concluída" : "Pendente";
        }

        private static void EscreverMensagem(string texto, ConsoleColor cor)
        {
            Console.ForegroundColor = cor;
            Console.WriteLine(texto);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
