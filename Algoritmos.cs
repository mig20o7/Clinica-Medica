using System;
using System.Collections.Generic;

namespace ClinicaMedica
{
    // Todos os algoritmos sao escritos manualmente, sem List.Sort, Find, LINQ etc.
    public static class Algoritmos
    {
        // ---------------------------------------------------------------
        // REGRAS DE COMPARACAO
        // ---------------------------------------------------------------

        // Regra de prioridade:
        // 1) Preferencial vem antes de Normal.
        // 2) Dentro do mesmo tipo, a menor senha (quem chegou primeiro) vem antes.
        // Retorna true se "a" deve ser chamado antes de "b".
        public static bool VemAntes(Paciente a, Paciente b)
        {
            if (a.Atendimento != b.Atendimento)
                return a.Atendimento > b.Atendimento;

            return a.Senha < b.Senha;
        }

        // Regra alfabetica: compara os nomes sem diferenciar maiusculas de
        // minusculas. Se os nomes forem iguais, a menor senha vem antes.
        // Retorna true se "a" deve aparecer antes de "b".
        public static bool NomeVemAntes(Paciente a, Paciente b)
        {
            int comparacao = string.Compare(a.Nome, b.Nome, StringComparison.CurrentCultureIgnoreCase);

            if (comparacao != 0)
                return comparacao < 0;

            return a.Senha < b.Senha;
        }

        // ---------------------------------------------------------------
        // ORDENACAO
        // ---------------------------------------------------------------

        // ORDENACAO 1: por prioridade, com insertion sort (ordenacao por insercao).
        // Cada paciente e "inserido" na posicao correta entre os anteriores.
        public static void OrdenarPorPrioridade(List<Paciente> lista)
        {
            for (int i = 1; i < lista.Count; i++)
            {
                Paciente atual = lista[i];
                int j = i - 1;

                while (j >= 0 && VemAntes(atual, lista[j]))
                {
                    lista[j + 1] = lista[j];
                    j--;
                }

                lista[j + 1] = atual;
            }
        }

        // ORDENACAO 2: por nome, com selection sort (ordenacao por selecao).
        // A cada passo, seleciona o menor nome do trecho ainda nao ordenado
        // e o troca com o primeiro elemento desse trecho.
        public static void OrdenarPorNome(List<Paciente> lista)
        {
            for (int i = 0; i < lista.Count - 1; i++)
            {
                int menor = i;

                for (int j = i + 1; j < lista.Count; j++)
                {
                    if (NomeVemAntes(lista[j], lista[menor]))
                        menor = j;
                }

                if (menor != i)
                {
                    Paciente temporario = lista[i];
                    lista[i] = lista[menor];
                    lista[menor] = temporario;
                }
            }
        }

        // ---------------------------------------------------------------
        // BUSCA
        // ---------------------------------------------------------------

        // BUSCA 1: por senha, com busca binaria.
        // A busca binaria exige a lista ordenada pela chave procurada. Como a
        // lista de espera fica ordenada por prioridade ou por nome, a busca e
        // feita sobre uma copia ordenada por senha, sem alterar a original.
        public static Paciente? BuscarPorSenha(List<Paciente> lista, int senha)
        {
            List<Paciente> copia = new List<Paciente>();
            for (int i = 0; i < lista.Count; i++)
                copia.Add(lista[i]);

            OrdenarPorSenha(copia);

            int inicio = 0;
            int fim = copia.Count - 1;

            while (inicio <= fim)
            {
                int meio = inicio + (fim - inicio) / 2;

                if (copia[meio].Senha == senha)
                    return copia[meio];

                if (copia[meio].Senha < senha)
                    inicio = meio + 1;
                else
                    fim = meio - 1;
            }

            return null;
        }

        // Apoio da busca binaria: insertion sort pela senha (ordem crescente).
        private static void OrdenarPorSenha(List<Paciente> lista)
        {
            for (int i = 1; i < lista.Count; i++)
            {
                Paciente atual = lista[i];
                int j = i - 1;

                while (j >= 0 && lista[j].Senha > atual.Senha)
                {
                    lista[j + 1] = lista[j];
                    j--;
                }

                lista[j + 1] = atual;
            }
        }

        // BUSCA 2: por nome, com busca linear (sequencial).
        // Percorre a lista do inicio ao fim e devolve o primeiro paciente cujo
        // nome contem o texto digitado (ex.: "joao" encontra "Joao Silva").
        public static Paciente? BuscarPorNome(List<Paciente> lista, string nome)
        {
            if (nome == "")
                return null;

            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].Nome.IndexOf(nome, StringComparison.CurrentCultureIgnoreCase) >= 0)
                    return lista[i];
            }

            return null;
        }

        // ---------------------------------------------------------------
        // SELECAO
        // ---------------------------------------------------------------

        // SELECAO 1: seleciona os pacientes de um tipo de servico.
        // Devolve uma nova lista, mantendo a ordem da lista original.
        public static List<Paciente> FiltrarPorServico(List<Paciente> lista, TipoServico servico)
        {
            List<Paciente> selecionados = new List<Paciente>();

            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].Servico == servico)
                    selecionados.Add(lista[i]);
            }

            return selecionados;
        }

        // SELECAO 2: seleciona o proximo a ser chamado (maior prioridade).
        // Percorre a lista uma unica vez, guardando o melhor candidato visto.
        // Devolve o indice do paciente, ou -1 se a lista estiver vazia.
        public static int SelecionarProximo(List<Paciente> lista)
        {
            if (lista.Count == 0)
                return -1;

            int melhor = 0;

            for (int i = 1; i < lista.Count; i++)
            {
                if (VemAntes(lista[i], lista[melhor]))
                    melhor = i;
            }

            return melhor;
        }
    }
}
