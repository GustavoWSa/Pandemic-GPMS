using UnityEngine;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour
{
    [Header("Turn Settings")]
    public int actionsPerTurn = 4;
    public int currentTurn = 1;
    public int currentActionsRemaining;
    
    [Header("Game State")]
    public bool isPlayerTurn = true;
    public bool isGameOver = false;
    
    [Header("References")]
    public Player currentPlayer;
    public List<City> allCities = new List<City>();
    
    private GameManager gameManager;
    
    void Start()
    {
        Debug.Log("[TurnManager] Iniciando TurnManager...");
        
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("[TurnManager] GameManager não encontrado na cena!");
        }
        
        InitializeCities();
        InitializePlayer();
        StartNewTurn();
    }
    
    void InitializeCities()
    {
        Debug.Log("[TurnManager] Inicializando 15 cidades...");
        
        // Criar as 15 cidades numeradas de 1 a 15
        for (int i = 1; i <= 15; i++)
        {
            City newCity = new City(i.ToString(), i);
            allCities.Add(newCity);
            Debug.Log($"[TurnManager] Cidade '{newCity.cityName}' criada com ID {newCity.cityId}");
        }
        
        Debug.Log($"[TurnManager] {allCities.Count} cidades inicializadas com sucesso.");
    }
    
    void InitializePlayer()
    {
        if (currentPlayer == null)
        {
            currentPlayer = new Player();
            currentPlayer.playerName = "Jogador";
            currentPlayer.actionsRemaining = actionsPerTurn;
            Debug.Log("[TurnManager] Jogador inicializado automaticamente.");
        }
        
        Debug.Log($"[TurnManager] Jogador '{currentPlayer.playerName}' inicializado com {actionsPerTurn} ações por rodada.");
    }
    
    public void StartNewTurn()
    {
        if (isGameOver)
        {
            Debug.LogWarning("[TurnManager] Tentativa de iniciar nova rodada com jogo finalizado!");
            return;
        }
        
        Debug.Log($"[TurnManager] Iniciando rodada {currentTurn}...");
        
        currentActionsRemaining = actionsPerTurn;
        currentPlayer.actionsRemaining = actionsPerTurn;
        isPlayerTurn = true;
        
        Debug.Log($"[TurnManager] Rodada {currentTurn} iniciada. Ações restantes: {currentActionsRemaining}");
    }
    
    public void EndPlayerTurn()
    {
        if (!isPlayerTurn)
        {
            Debug.LogWarning("[TurnManager] Tentativa de finalizar rodada do jogador quando não é sua vez!");
            return;
        }
        
        Debug.Log("[TurnManager] Finalizando rodada do jogador...");
        isPlayerTurn = false;
        
        // Executar fase de infecção
        ExecuteInfectionPhase();
        
        // Verificar condições de vitória/derrota
        CheckGameConditions();
        
        // Iniciar próxima rodada se o jogo não acabou
        if (!isGameOver)
        {
            currentTurn++;
            StartNewTurn();
        }
    }
    
    public bool CanPerformAction()
    {
        return isPlayerTurn && currentActionsRemaining > 0 && !isGameOver;
    }
    
    public void UseAction()
    {
        if (!CanPerformAction())
        {
            Debug.LogWarning("[TurnManager] Tentativa de usar ação quando não é permitido!");
            return;
        }
        
        currentActionsRemaining--;
        currentPlayer.actionsRemaining--;
        
        Debug.Log($"[TurnManager] Ação usada. Ações restantes: {currentActionsRemaining}");
        
        // Verificar se acabaram as ações
        if (currentActionsRemaining <= 0)
        {
            Debug.Log("[TurnManager] Todas as ações foram usadas. Finalizando rodada do jogador...");
            EndPlayerTurn();
        }
    }
    
    void ExecuteInfectionPhase()
    {
        Debug.Log("[TurnManager] Executando fase de infecção...");
        
        // Simular infecção em cidade aleatória
        if (allCities.Count > 0)
        {
            int randomIndex = Random.Range(0, allCities.Count);
            City infectedCity = allCities[randomIndex];
            
            if (infectedCity.infectionLevel < infectedCity.maxInfectionLevel)
            {
                infectedCity.Infect();
                
                // Se a cidade teve um outbreak, infectar cidades conectadas
                if (infectedCity.isOutbreak)
                {
                    InfectConnectedCities(infectedCity);
                }
            }
            else
            {
                Debug.Log($"[TurnManager] Cidade {infectedCity.cityName} já está no nível máximo de infecção.");
            }
        }
    }
    
    void InfectConnectedCities(City outbreakCity)
    {
        Debug.Log($"[TurnManager] Infectando cidades conectadas à {outbreakCity.cityName}...");
        
        List<int> connectedIds = outbreakCity.GetConnectedCityIds();
        foreach (int connectedId in connectedIds)
        {
            City connectedCity = GetCityById(connectedId);
            if (connectedCity != null)
            {
                connectedCity.Infect();
                Debug.Log($"[TurnManager] Cidade conectada {connectedCity.cityName} foi infectada por outbreak.");
            }
            else
            {
                Debug.LogWarning($"[TurnManager] Cidade conectada com ID {connectedId} não encontrada!");
            }
        }
    }
    
    void CheckGameConditions()
    {
        // Verificar se alguma cidade atingiu o nível máximo de infecção
        foreach (City city in allCities)
        {
            if (city.infectionLevel >= city.maxInfectionLevel)
            {
                Debug.LogWarning($"[TurnManager] Cidade {city.cityName} atingiu o nível máximo de infecção! Jogo perdido.");
                isGameOver = true;
                return;
            }
        }
        
        // Verificar se todas as cidades estão curadas (vitória)
        bool allCitiesCured = true;
        foreach (City city in allCities)
        {
            if (city.infectionLevel > 0)
            {
                allCitiesCured = false;
                break;
            }
        }
        
        if (allCitiesCured)
        {
            Debug.Log("[TurnManager] Todas as cidades foram curadas! Vitória!");
            isGameOver = true;
        }
    }
    
    public City GetCityByName(string cityName)
    {
        foreach (City city in allCities)
        {
            if (city.cityName.Equals(cityName, System.StringComparison.OrdinalIgnoreCase))
            {
                return city;
            }
        }
        return null;
    }
    
    public City GetCityById(int cityId)
    {
        foreach (City city in allCities)
        {
            if (city.cityId == cityId)
            {
                return city;
            }
        }
        return null;
    }
} 