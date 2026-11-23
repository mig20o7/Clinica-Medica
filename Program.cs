using System;
using System.Collections.Generic;

namespace ClinicaMedica
{
    public class Program
    {
        private enum Ordenacao
        {
            Prioridade,
            Nome
        }

        // Estrutura linear: lista de espera da clinica.
        private static readonly List<Paciente> listaDeEspera = new List<Paciente>();

        // Contador unico de senhas: quem chega depois recebe um numero maior.
        private static int proximaSenha = 1;

        // Estado da tela: como a lista esta ordenada e filtrada.
        private static Ordenacao ordenacaoAtual = Ordenacao.Prioridade;
        private static TipoServico? filtroAtual = null;

        // Resultado da ultima acao, exibido abaixo da lista.
        private static string mensagem = "";
        private static bool mensagemDeErro = false;

        private const int Largura = 62;
        private static readonly string LinhaDupla = new string('=', Largura);
        private static readonly string LinhaSimples = new string('-', Largura);

        public static void Main()
        {
            Console.Title = "Clinica Medica";
            int opcao;

            do
            {
                DesenharTela();
                opcao = LerInteiro(" Opcao: ");

                // A mensagem anterior ja foi exibida; a nova acao define a proxima.
                mensagem = "";
                mensagemDeErro = false;

                try
                {
                    switch (opcao)
                    {
                        case 1: RetirarSenha(); break;
                        case 2: ordenacaoAtual = Ordenacao.Prioridade; break;
                        case 3: ordenacaoAtual = Ordenacao.Nome; break;
                        case 4: BuscarPorSenha(); break;
                        case 5: BuscarPorNome(); break;
                        case 6: FiltrarPorServico(); break;
                        case 7: ChamarProximo(); break;
                        case 0: break;
                        default: Avisar("Opcao invalida.", true); break;
                    }
                }
                catch (NotImplementedException)
                {
                    Avisar("Funcao ainda nao implementada.", true);
                }
            } while (opcao != 0);

            LimparTela();
            Console.WriteLine("Sistema encerrado.");
        }

        // ---------------------------------------------------------------
        // TELA
        // ---------------------------------------------------------------

        // Limpa o console e redesenha tudo: cabecalho, lista, mensagem e menu.
        private static void DesenharTela()
        {
            // A lista e preparada antes de desenhar, pois pode alterar o estado
            // (por exemplo, se a ordenacao por nome ainda nao foi implementada).
            List<Paciente> visiveis = ObterListaVisivel();

            LimparTela();

            EscreverLinha(LinhaDupla, ConsoleColor.Cyan);
            EscreverLinha(Centralizar("CLINICA MEDICA - LISTA DE ESPERA"), ConsoleColor.Cyan);
            EscreverLinha(LinhaDupla, ConsoleColor.Cyan);

            string filtro = filtroAtual == null ? "Todos" : filtroAtual.Value.ToString();
            EscreverLinha(
                $" Ordenacao: {ordenacaoAtual} | Servico: {filtro} | Aguardando: {listaDeEspera.Count}",
                ConsoleColor.DarkGray);
            Console.WriteLine(LinhaSimples);

            Console.WriteLine($" {"SENHA",-7}{"NOME",-27}{"ATENDIMENTO",-14}{"SERVICO"}");
            Console.WriteLine(LinhaSimples);

            if (visiveis.Count == 0)
            {
                EscreverLinha(" (nenhum paciente)", ConsoleColor.DarkGray);
            }
            else
            {
                for (int i = 0; i < visiveis.Count; i++)
                    EscreverPaciente(visiveis[i]);
            }

            Console.WriteLine(LinhaSimples);

            if (mensagem != "")
            {
                EscreverLinha(" " + mensagem, mensagemDeErro ? ConsoleColor.Red : ConsoleColor.Green);
                Console.WriteLine(LinhaSimples);
            }

            Console.WriteLine(" 1 - Retirar senha            5 - Buscar por nome");
            Console.WriteLine(" 2 - Ordenar por prioridade   6 - Filtrar por servico");
            Console.WriteLine(" 3 - Ordenar por nome         7 - Chamar proximo");
            Console.WriteLine(" 4 - Buscar por senha         0 - Sair");
            Console.WriteLine(LinhaSimples);
        }

        // Aplica a ordenacao e o filtro atuais e devolve o que deve aparecer.
        private static List<Paciente> ObterListaVisivel()
        {
            if (ordenacaoAtual == Ordenacao.Nome)
            {
                try
                {
                    Algoritmos.OrdenarPorNome(listaDeEspera);
                }
                catch (NotImplementedException)
                {
                    ordenacaoAtual = Ordenacao.Prioridade;
                    Avisar("Ordenacao por nome ainda nao implementada.", true);
                }
            }

            if (ordenacaoAtual == Ordenacao.Prioridade)
                Algoritmos.OrdenarPorPrioridade(listaDeEspera);

            if (filtroAtual == null)
                return listaDeEspera;

            try
            {
                return Algoritmos.FiltrarPorServico(listaDeEspera, filtroAtual.Value);
            }
            catch (NotImplementedException)
            {
                filtroAtual = null;
                Avisar("Filtro por servico ainda nao implementado.", true);
                return listaDeEspera;
            }
        }

        private static void EscreverPaciente(Paciente p)
        {
            string nome = p.Nome.Length > 25 ? p.Nome.Substring(0, 25) : p.Nome;
            string linha = $" {p.Senha.ToString("D3"),-7}{nome,-27}{p.Atendimento,-14}{p.Servico}";

            if (p.Atendimento == TipoAtendimento.Preferencial)
                EscreverLinha(linha, ConsoleColor.Yellow);
            else
                Console.WriteLine(linha);
        }

        private static void EscreverLinha(string texto, ConsoleColor cor)
        {
            Console.ForegroundColor = cor;
            Console.WriteLine(texto);
            Console.ResetColor();
        }

        private static string Centralizar(string texto)
        {
            int espacos = (Largura - texto.Length) / 2;
            return espacos > 0 ? new string(' ', espacos) + texto : texto;
        }

        private static void LimparTela()
        {
            try
            {
                Console.Clear();
            }
            catch (System.IO.IOException)
            {
                // Sem console real (saida redirecionada): segue sem limpar.
            }
        }

        private static void Avisar(string texto, bool erro)
        {
            mensagem = texto;
            mensagemDeErro = erro;
        }

        private static string Descrever(Paciente p)
        {
            return $"senha {p.Senha:D3} - {p.Nome} ({p.Atendimento}, {p.Servico})";
        }

        // ---------------------------------------------------------------
        // ACOES DO MENU
        // ---------------------------------------------------------------

        private static void RetirarSenha()
        {
            Console.Write(" Nome: ");
            string nome = (Console.ReadLine() ?? "").Trim();

            if (nome == "")
            {
                Avisar("Nome obrigatorio. Nenhum paciente registrado.", true);
                return;
            }

            int a = LerInteiro(" Atendimento (0 - Normal, 1 - Preferencial): ");
            if (a != 0 && a != 1)
            {
                Avisar("Atendimento invalido. Nenhum paciente registrado.", true);
                return;
            }

            int s = LerInteiro(" Servico (1 - Consulta, 2 - Retorno, 3 - Procedimento): ");
            if (s < 1 || s > 3)
            {
                Avisar("Servico invalido. Nenhum paciente registrado.", true);
                return;
            }

            Paciente p = new Paciente(proximaSenha, nome, (TipoAtendimento)a, (TipoServico)s);
            proximaSenha++;
            listaDeEspera.Add(p);

            Avisar("Registrado: " + Descrever(p), false);
        }

        private static void BuscarPorSenha()
        {
            int senha = LerInteiro(" Senha: ");
            Paciente? p = Algoritmos.BuscarPorSenha(listaDeEspera, senha);

            if (p == null)
                Avisar($"Senha {senha:D3} nao encontrada.", true);
            else
                Avisar("Encontrado: " + Descrever(p), false);
        }

        private static void BuscarPorNome()
        {
            Console.Write(" Nome: ");
            string nome = (Console.ReadLine() ?? "").Trim();
            Paciente? p = Algoritmos.BuscarPorNome(listaDeEspera, nome);

            if (p == null)
                Avisar($"Paciente \"{nome}\" nao encontrado.", true);
            else
                Avisar("Encontrado: " + Descrever(p), false);
        }

        private static void FiltrarPorServico()
        {
            int s = LerInteiro(" Servico (0 - Todos, 1 - Consulta, 2 - Retorno, 3 - Procedimento): ");

            if (s < 0 || s > 3)
            {
                Avisar("Servico invalido.", true);
                return;
            }

            if (s == 0)
                filtroAtual = null;
            else
                filtroAtual = (TipoServico)s;
        }

        // Chama o paciente de maior prioridade. Se houver filtro de servico
        // ativo, chama o de maior prioridade dentro daquele servico.
        private static void ChamarProximo()
        {
            List<Paciente> candidatos = filtroAtual == null
                ? listaDeEspera
                : Algoritmos.FiltrarPorServico(listaDeEspera, filtroAtual.Value);

            int indice = Algoritmos.SelecionarProximo(candidatos);

            if (indice < 0)
            {
                Avisar("Nao ha pacientes aguardando.", true);
                return;
            }

            Paciente p = candidatos[indice];
            RemoverDaLista(p);

            Avisar("CHAMANDO: " + Descrever(p), false);
        }

        private static void RemoverDaLista(Paciente p)
        {
            for (int i = 0; i < listaDeEspera.Count; i++)
            {
                if (listaDeEspera[i].Senha == p.Senha)
                {
                    listaDeEspera.RemoveAt(i);
                    return;
                }
            }
        }

        private static int LerInteiro(string texto)
        {
            while (true)
            {
                Console.Write(texto);
                if (int.TryParse(Console.ReadLine(), out int valor))
                    return valor;

                EscreverLinha(" Digite um numero inteiro.", ConsoleColor.Red);
            }
        }
    }
}
