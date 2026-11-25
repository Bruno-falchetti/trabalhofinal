using System.Collections.Generic;

namespace GerenciadorDeTarefas
{
    public class GerenciadorTarefas
    {
        private int contadorId = 0;
        private readonly List<Tarefa> tarefas;
        private readonly Stack<int> historicoConcluidas;

        public GerenciadorTarefas()
        {
            this.tarefas = new List<Tarefa>();
            this.historicoConcluidas = new Stack<int>();
        }

        public Tarefa CadastrarTarefa(string descricao)
        {
            Tarefa novaTarefa = new Tarefa(++this.contadorId, descricao);
            this.tarefas.Add(novaTarefa);
            return novaTarefa;
        }

        public List<Tarefa> ListarTarefas()
        {
            return this.tarefas;
        }

        public Tarefa BuscarTarefa(int id)
        {
            foreach (Tarefa tarefa in this.tarefas)
            {
                if (tarefa.Id == id)
                {
                    return tarefa;
                }
            }
            return null;
        }

        public bool ConcluirTarefa(int id, out string mensagem)
        {
            Tarefa tarefa = BuscarTarefa(id);

            if (tarefa == null)
            {
                mensagem = $"Erro: Nenhuma tarefa encontrada com o ID {id}.";
                return false;
            }

            if (tarefa.Situacao == SituacaoTarefaEnum.CONCLUIDA)
            {
                mensagem = $"Aviso: A tarefa #{id} já foi concluída anteriormente.";
                return false;
            }

            tarefa.Situacao = SituacaoTarefaEnum.CONCLUIDA;
            this.historicoConcluidas.Push(tarefa.Id);
            mensagem = $"Sucesso: Tarefa #{id} marcada como concluída.";
            return true;
        }

        public bool DesfazerUltimaConclusao(out string mensagem)
        {
            if (this.historicoConcluidas.Count == 0)
            {
                mensagem = "Aviso: Não há nenhuma conclusão registrada na pilha para desfazer.";
                return false;
            }

            int ultimoIdConcluido = this.historicoConcluidas.Pop();
            Tarefa tarefa = BuscarTarefa(ultimoIdConcluido);

            if (tarefa != null)
            {
                tarefa.Situacao = SituacaoTarefaEnum.PENDENTE;
                mensagem = $"Sucesso: Conclusão desfeita! A tarefa #{tarefa.Id} (\"{tarefa.Descricao}\") voltou para PENDENTE.";
                return true;
            }

            mensagem = "Erro: Tarefa correspondente não localizada.";
            return false;
        }
    }
}
