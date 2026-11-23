Clínica Médica – Sistema de Lista de Espera

Trabalho da disciplina Algoritmos 2 – Engenharia da Computação – Unisapiens Professora: ROSANGELA SILVA CUNHA – Quarto Período 

Integrantes:
Ana Julia
Vinicius Ferreira
Maria Luiza
Vinicius Casara
Miguel Shimizu

Aplicativo de console, em C#, que simula a lista de espera de uma clínica médica. O paciente retira uma senha informando o tipo de atendimento (normal ou preferencial) e o tipo de serviço (consulta, retorno ou procedimento). O atendente pode ordenar a lista, buscar pacientes, filtrar por serviço e chamar o próximo.

O objetivo é demonstrar a aplicação de algoritmos de ordenação, busca e seleção sobre uma estrutura de dados linear. Todos os algoritmos foram escritos manualmente, sem recursos prontos da linguagem (Sort, Find, LINQ).

Regra de prioridade
Paciente preferencial é chamado antes de paciente normal.
Entre pacientes do mesmo tipo, é chamado primeiro o de menor senha (quem chegou antes).
Estrutura de dados e algoritmos

Estrutura linear: lista de espera (List<Paciente>).

Operação	Parâmetro	Algoritmo	Função
Ordenação	Prioridade	Insertion sort	OrdenarPorPrioridade
Ordenação	Nome	Selection sort	OrdenarPorNome
Busca	Senha	Busca binária	BuscarPorSenha
Busca	Nome	Busca linear	BuscarPorNome
Seleção	Tipo de serviço	Filtro em uma passagem	FiltrarPorServico
Seleção	Maior prioridade	Seleção em uma passagem	SelecionarProximo
Funcionalidades
Retirar senha (cadastrar paciente)
Ordenar a lista por prioridade
Ordenar a lista por nome
Buscar paciente por senha
Buscar paciente por nome
Filtrar a lista por tipo de serviço
Chamar o próximo paciente
Organização do código
Arquivo	Conteúdo
Paciente.cs	Classe Paciente e os tipos de atendimento e serviço
Algoritmos.cs	Algoritmos de ordenação, busca e seleção
Program.cs	Tela, menu e ações do console
Como executar

Requisito: .NET SDK [VERSÃO] ou Visual Studio com o componente "Desenvolvimento para desktop com .NET".

Pelo Visual Studio: abrir o arquivo de projeto (.csproj ou .sln) e pressionar F5.

Pelo terminal, na pasta do projeto:

dotnet run
Exemplo de execução

Tela do sistema

Documentação

O relatório completo do trabalho (introdução, objetivo, desenvolvimento e conclusão) está em docs/Relatorio.pdf.
