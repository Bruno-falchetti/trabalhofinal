namespace GerenciadorDeTarefas
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public SituacaoTarefaEnum Situacao { get; set; }

        public Tarefa(int id, string descricao)
        {
            Id = id;
            Descricao = descricao;
            Situacao = SituacaoTarefaEnum.PENDENTE;
        }
    }
}