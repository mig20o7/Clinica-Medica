namespace ClinicaMedica
{
    public enum TipoAtendimento
    {
        Normal = 0,
        Preferencial = 1
    }

    public enum TipoServico
    {
        Consulta = 1,
        Retorno = 2,
        Procedimento = 3
    }

    public class Paciente
    {
        public int Senha { get; }
        public string Nome { get; }
        public TipoAtendimento Atendimento { get; }
        public TipoServico Servico { get; }

        public Paciente(int senha, string nome, TipoAtendimento atendimento, TipoServico servico)
        {
            Senha = senha;
            Nome = nome;
            Atendimento = atendimento;
            Servico = servico;
        }

        public override string ToString()
        {
            return $"Senha {Senha,3} | {Nome,-25} | {Atendimento,-12} | {Servico}";
        }
    }
}
