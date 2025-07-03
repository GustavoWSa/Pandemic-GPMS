# Pandemic - Jogo de Tabuleiro Digital

Repositório para o desenvolvimento do jogo de tabuleiro digital *Pandemic*, como parte da avaliação da disciplina de Gerência de Projetos de Modelagem de Software (GPMS) da Universidade Federal Fluminense (UFF), período 2025.1.

## Escopo do Projeto e Funcionalidades

O objetivo deste projeto é criar uma versão digital do jogo cooperativo *Pandemic*, implementando suas principais mecânicas e regras com algumas modificações para simplificar o desenvolvimento do trabalho.

### Tecnologias Utilizadas:

* **GIT:** Utilizado para realizar versionamento do projeto.
* **Github:** Plataforma para salvar repositório e modificações no código.
* **Unity:** Desenvolver inteface gráfica do jogo.
* **C#:** Linguagem usada para funciolidades do jogo.
* **Visual Studio:** IDE para codificar com C# e Unity.

### Funcionalidades Planejadas:

* **Ações do Jogador:** Cada jogador poderá realizar até 4 ações por turno, incluindo:
    * Movimentar-se entre cidades.
    * Tratar uma doença em sua cidade atual.
    * Descobrir a cura para uma das doenças.
    * Compartilhar conhecimento (cartas) com outro jogador.
* **Mecânica de Infecção:** O avanço das doenças será controlado por um baralho de cartas de infecção, que determinará onde novos surtos ocorrem.
* **Condições de Vitória e Derrota:** O jogo terá regras claras para a vitória (erradicação de todas as doenças) e para a derrota (surtos excessivos ou esgotamento do baralho de jogadores).
* **Modo Cooperativo:** Todas as mecânicas serão projetadas para um ambiente de jogo totalmente cooperativo, onde os jogadores trabalham juntos para alcançar um objetivo comum.
* **Mapa e Doenças:** O jogo contará com um mapa global dividido em regiões, cada uma suscetível a doenças específicas. A velocidade de infecção e as epidemias aumentarão a dificuldade progressivamente. O mapa e algumas mecânicas de infecção foram simplificadas para ajudar no desenvolvimento do projeto.
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
3. Abra o projeto com a versão recomendada do Unity (2022.3.x LTS ou superior).
4. No editor do Unity, abra a cena desejada em `Assets/Scenes/`:
   - `MenuScene.unity` - Tela principal do menu
   - `SampleScene.unity` - Cena principal do jogo
5. Clique em "Play" para iniciar o jogo.

> **Dica:** Certifique-se de que todas as dependências do projeto estão instaladas (TextMeshPro, etc).

## Controles Básicos

- Use o mouse para interagir com os botões do menu.
- Cada jogador pode realizar até 4 ações por turno.
- Clique nas regiões do mapa para tratar doenças ou realizar ações específicas.
- Use o botão "Passar Rodada" para avançar o turno.
- O botão "Sobre" no menu leva a uma tela com explicações sobre o jogo.

## Estrutura do Projeto

```
Assets/
├── Scenes/           # Cenas do jogo (Menu, Game)
├── Scripts/          # Códigos C#
│   ├── Game/         # Lógica principal do jogo
│   ├── Cartas/       # Sistema de cartas
│   └── Regions/      # Controle das regiões
├── TextMesh Pro/     # Sistema de texto
└── Settings/         # Configurações do Unity
```

## Status do Desenvolvimento

### ✅ Implementado
- Sistema básico de regiões e infecção
- Interface de menu principal
- Mecânica de passar rodada
- Sistema de cartas básico
- Controle de nível de infecção por região

### 🚧 Em Desenvolvimento
- Sistema completo de ações do jogador
- Mecânica de vitória e derrota
- Interface de jogo completa
- Sistema cooperativo multiplayer

### 📋 Planejado
- Sistema de epidemias
- Mecânica de cura de doenças
- Interface de cartas completa
- Sons e efeitos visuais

## Como Contribuir

1. Faça um fork deste repositório
2. Crie uma branch para sua feature: `git checkout -b nova-feature`
3. Commit suas alterações: `git commit -m 'Adiciona nova funcionalidade'`
4. Push para a branch: `git push origin nova-feature`
5. Abra um Pull Request

## Licença

Este projeto foi desenvolvido como parte da disciplina GPMS da UFF. Todos os direitos reservados aos autores.
