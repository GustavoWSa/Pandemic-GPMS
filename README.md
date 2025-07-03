# Pandemic - Jogo de Tabuleiro Digital

Repositório para o desenvolvimento do jogo de tabuleiro digital *Pandemic*, como parte da avaliação da disciplina de Gerência de Projetos de Modelagem de Software (GPMS) da Universidade Federal Fluminense (UFF), período 2025.1.

## Escopo do Projeto e Funcionalidades

O objetivo deste projeto é criar uma versão digital do jogo cooperativo *Pandemic*, implementando suas principais mecânicas e regras.

### Funcionalidades Planejadas:

* **Ações do Jogador:** Cada jogador poderá realizar até 4 ações por turno, incluindo:
    * Movimentar-se entre cidades.
    * Tratar uma doença em sua cidade atual.
    * Descobrir a cura para uma das doenças.
    * Compartilhar conhecimento (cartas) com outro jogador.
* **Mecânica de Infecção:** O avanço das doenças será controlado por um baralho de cartas de infecção, que determinará onde novos surtos ocorrem.
* **Condições de Vitória e Derrota:** O jogo terá regras claras para a vitória (erradicação de todas as doenças) e para a derrota (surtos excessivos ou esgotamento do baralho de jogadores).
* **Modo Cooperativo:** Todas as mecânicas serão projetadas para um ambiente de jogo totalmente cooperativo, onde os jogadores trabalham juntos para alcançar um objetivo comum.
* **Mapa e Doenças:** O jogo contará com um mapa global dividido em regiões, cada uma suscetível a doenças específicas. A velocidade de infecção e as epidemias aumentarão a dificuldade progressivamente.
* **Interface Visual:** Será desenvolvida uma interface gráfica para o usuário, permitindo uma interação clara e intuitiva com o tabuleiro e as ações do jogo.

## Equipe e Responsabilidades

O desenvolvimento do projeto foi dividido entre os seguintes membros da equipe:

| Responsabilidade                                    | Membro Responsável    |
| --------------------------------------------------- | --------------------- |
| Ações do Jogador (movimentar, tratar, curar, etc.)  | Caio                  |
| Mecânica de Infecção por Cartas                     | Kenji D. Odate        |
| Regras de Vitória e Derrota                         | Daniel Fontoura       |
| Lógica do Modo Cooperativo                          | Vinicius Gomes        |
| Interface Visual (UI/UX)                            | João Otogali          |
| Mapa, Doenças, Epidemias e Velocidade de Infecção   | Gustavo Wermelinger Sá|

---
**Universidade Federal Fluminense (UFF)**
**Disciplina:** GPMS
**Período:** 2025.1

## Como Rodar o Projeto

1. Certifique-se de ter o [Unity Hub](https://unity.com/download) instalado.
2. Abra o Unity Hub e clique em "Adicionar" para selecionar a pasta deste repositório.
3. Abra o projeto com a versão recomendada do Unity (ex: 2022.x ou superior).
4. No editor do Unity, abra a cena desejada em `Assets/Scenes/` (ex: `MenuScene.unity` ou `SampleScene.unity`).
5. Clique em "Play" para iniciar o jogo.

> **Dica:** Certifique-se de que todas as dependências do projeto estão instaladas (TextMeshPro, etc).

## Controles Básicos

- Use o mouse para interagir com os botões do menu.
- Cada jogador pode realizar até 4 ações por turno.
- Clique nas regiões do mapa para tratar doenças ou realizar ações específicas.
- Use o botão "Passar Rodada" para avançar o turno.
- O botão "Sobre" no menu leva a uma tela com explicações sobre o jogo.
