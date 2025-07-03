using UnityEngine;
using System.Collections.Generic;
using System.Linq; 

public class TurnManager : MonoBehaviour
{
    [Header("Configurações de Turno")]
    public int actionsPerTurn = 4;
    public int currentTurn = 1;
    public int currentActionsRemaining;

    [Header("Referências do Jogo")]
    public Player currentPlayer;
    public List<City> allCities = new List<City>();
    public CardManager baralhoDeJogadores; // Arraste o objeto do CardManager dos jogadores aqui no Inspector do Unity
    public CardManager baralhoDeInfeccao; // Arraste o objeto do CardManager de infecção aqui

    [Header("Condições de Vitória/Derrota")]
    public bool isGameOver = false;

    [Tooltip("Contador de surtos que ocorreram no jogo.")]
    public int contadorDeSurtos = 0;
    [Tooltip("O jogo acaba se o número de surtos atingir este valor.")]
    public int limiteDeSurtos = 8;

    [Tooltip("Dicionário que rastreia as curas descobertas.")]
    public Dictionary<string, bool> curasDescobertas;
    // ===================================================================

    void Start()
    {
        Debug.Log("[TurnManager] Iniciando TurnManager...");
        InitializeCities();
        InitializePlayer();
        
        // Inicializa as variáveis de condição de jogo
        InicializarCondicoesDeJogo();

        StartNewTurn();
    }

    // Função para inicializar suas variáveis
    void InicializarCondicoesDeJogo()
    {
        contadorDeSurtos = 0;
        isGameOver = false;
        curasDescobertas = new Dictionary<string, bool>()
        {
            { "Amarelo", false },
            { "Vermelho", false },
            { "Azul", false },
            { "Preto", false }
            // IMPORTANTE: As chaves aqui ("Amarelo", "Vermelho", etc.) devem ser
            // exatamente as mesmas strings usadas no campo "diseaseType" da DiscoverCureAction
        };
        Debug.Log("[TurnManager] Condições de Vitória/Derrota inicializadas.");
    }

    public void EndPlayerTurn()
    {
        if (!isPlayerTurn) return;

        Debug.Log("[TurnManager] Finalizando rodada do jogador...");
        isPlayerTurn = false;

        // --- FASE DE COMPRA DE CARTAS ---
 
        if (baralhoDeJogadores.deck.Count < 2)
        {
            FinalizarJogo("DERROTA! O tempo acabou! Não há cartas suficientes para comprar.");
            return; 
        }
        
        Debug.Log("[TurnManager] Jogador compraria 2 cartas.");


        // --- FASE DE INFECÇÃO ---
        ExecuteInfectionPhase();

        
        if (!isGameOver)
        {
            currentTurn++;
            StartNewTurn();
        }
    }

    public void CheckGameConditions()
    {
        if (isGameOver) return; 

        // 1. CONDIÇÃO DE DERROTA: Surtos
        if (contadorDeSurtos >= limiteDeSurtos)
        {
            FinalizarJogo("DERROTA! O número de surtos saiu de controle!");
            return;
        }

        // CONDIÇÃO DE VITÓRIA: Todas as curas descobertas
        if (curasDescobertas.Values.All(status => status == true))
        {
            FinalizarJogo("VITÓRIA! Todas as curas foram descobertas! O Rio de Janeiro está salvo!");
            return;
        }
    }

    public bool ChecarEstoqueDeCubos(string corDoCubo, int quantidade)
    {
        return false;
    }


    private void FinalizarJogo(string mensagem)
    {
        if (isGameOver) return; // Garante que a mensagem de fim de jogo apareça só uma vez

        isGameOver = true;
        Debug.LogError("--- FIM DE JOGO ---");
        Debug.LogError(mensagem);
        Time.timeScale = 0; // Pausa o jogo
    }
    // ===================================================================

    // Funções de notificação que serão chamadas por outros scripts
    public void NotificarSurto()
    {
        contadorDeSurtos++;
        Debug.LogWarning($"[TurnManager] SURTO NOTIFICADO! Contagem atual: {contadorDeSurtos}/{limiteDeSurtos}");
        CheckGameConditions(); // Verifica imediatamente se o limite foi atingido
    }

    public void NotificarCuraDescoberta(string corDaDoenca)
    {
        if (curasDescobertas.ContainsKey(corDaDoenca))
        {
            curasDescobertas[corDaDoenca] = true;
            Debug.Log($"[TurnManager] CURA NOTIFICADA: {corDaDoenca}!");
            CheckGameConditions(); // Verifica imediatamente se a condição de vitória foi atingida
        }
    }

    #region Funções Originais do TurnManager
    public bool isPlayerTurn = true;

    void InitializeCities()
    {
        Debug.Log("[TurnManager] Inicializando 15 cidades...");
        for (int i = 1; i <= 15; i++) { allCities.Add(new City(i.ToString(), i)); }
        Debug.Log($"[TurnManager] {allCities.Count} cidades inicializadas com sucesso.");
    }

    void InitializePlayer()
    {
        if (currentPlayer == null) { currentPlayer = new Player(); }
    }

    public void StartNewTurn()
    {
        if (isGameOver) return;
        Debug.Log($"[TurnManager] Iniciando rodada {currentTurn}...");
        currentActionsRemaining = actionsPerTurn;
        currentPlayer.actionsRemaining = actionsPerTurn;
        isPlayerTurn = true;
    }

    public bool CanPerformAction()
    {
        return isPlayerTurn && currentActionsRemaining > 0 && !isGameOver;
    }

    public void UseAction()
    {
        if (!CanPerformAction()) return;
        currentActionsRemaining--;
        currentPlayer.actionsRemaining--;
        if (currentActionsRemaining <= 0) { EndPlayerTurn(); }
    }

    void ExecuteInfectionPhase()
    {
        Debug.Log("[TurnManager] Executando fase de infecção...");
        if (allCities.Count > 0)
        {
            int randomIndex = Random.Range(0, allCities.Count);
            City infectedCity = allCities[randomIndex];
            if (infectedCity.infectionLevel < infectedCity.maxInfectionLevel)
            {
                infectedCity.Infect();
                if (infectedCity.isOutbreak) { InfectConnectedCities(infectedCity); }
            }
        }
    }

    void InfectConnectedCities(City outbreakCity)
    {
        List<int> connectedIds = outbreakCity.GetConnectedCityIds();
        foreach (int connectedId in connectedIds)
        {
            City connectedCity = GetCityById(connectedId);
            if (connectedCity != null) { connectedCity.Infect(); }
        }
    }

    public City GetCityByName(string cityName)
    {
        return allCities.Find(city => city.cityName.Equals(cityName, System.StringComparison.OrdinalIgnoreCase));
    }

    public City GetCityById(int cityId)
    {
        return allCities.Find(city => city.cityId == cityId);
    }
    #endregion
}