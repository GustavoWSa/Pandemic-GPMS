using UnityEngine;

public abstract class Action : MonoBehaviour
{
    [Header("Action Info")]
    public string actionName;
    public string description;
    public int actionCost = 1;
    
    [Header("Action State")]
    public bool isAvailable = true;
    public bool requiresTarget = false;
    
    protected TurnManager turnManager;
    protected Player currentPlayer;
    
    void Start()
    {
        Debug.Log($"[Action] Ação '{actionName}' inicializada.");
        
        turnManager = FindObjectOfType<TurnManager>();
        if (turnManager == null)
        {
            Debug.LogError("[Action] TurnManager não encontrado na cena!");
        }
        else
        {
            currentPlayer = turnManager.currentPlayer;
        }
    }
    
    public virtual bool CanExecute()
    {
        if (turnManager == null || currentPlayer == null)
        {
            Debug.LogWarning("[Action] TurnManager ou Player não encontrados!");
            return false;
        }
        
        if (!turnManager.CanPerformAction())
        {
            Debug.Log($"[Action] Não é possível executar ação '{actionName}'. Não é a vez do jogador ou não há ações disponíveis.");
            return false;
        }
        
        if (!isAvailable)
        {
            Debug.Log($"[Action] Ação '{actionName}' não está disponível.");
            return false;
        }
        
        return true;
    }
    
    public virtual void Execute()
    {
        if (!CanExecute())
        {
            Debug.LogWarning($"[Action] Tentativa de executar ação '{actionName}' quando não é permitido!");
            return;
        }
        
        Debug.Log($"[Action] Executando ação '{actionName}'...");
        
        // Usar ação do turno
        turnManager.UseAction();
        currentPlayer.UseAction();
        
        // Executar lógica específica da ação
        ExecuteActionLogic();
        
        Debug.Log($"[Action] Ação '{actionName}' executada com sucesso.");
    }
    
    protected abstract void ExecuteActionLogic();
    
    public virtual void SetAvailable(bool available)
    {
        isAvailable = available;
        Debug.Log($"[Action] Ação '{actionName}' disponibilidade alterada para: {available}");
    }
    
    public virtual string GetActionDescription()
    {
        return $"{actionName}: {description} (Custo: {actionCost} ação)";
    }
}

// Ação específica: Mover para cidade conectada
public class MoveAction : Action
{
    [Header("Move Action")]
    public City targetCity;
    
    void Awake()
    {
        actionName = "Mover";
        description = "Mover para uma cidade conectada";
        actionCost = 1;
        requiresTarget = true;
    }
    
    protected override bool CanExecute()
    {
        if (!base.CanExecute())
            return false;
        
        if (targetCity == null)
        {
            Debug.LogWarning("[MoveAction] Cidade alvo não definida!");
            return false;
        }
        
        // Verificar se a cidade alvo está conectada à cidade atual
        if (currentPlayer.currentLocation != null)
        {
            if (!currentPlayer.currentLocation.IsConnectedTo(targetCity.cityId))
            {
                Debug.LogWarning($"[MoveAction] Cidade {targetCity.cityName} não está conectada à cidade atual {currentPlayer.currentLocation.cityName}!");
                return false;
            }
        }
        
        return true;
    }
    
    protected override void ExecuteActionLogic()
    {
        Debug.Log($"[MoveAction] Movendo jogador de {(currentPlayer.currentLocation != null ? currentPlayer.currentLocation.cityName : "nenhuma cidade")} para {targetCity.cityName}");
        
        currentPlayer.MoveToCity(targetCity);
    }
}

// Ação específica: Curar cidade
public class CureAction : Action
{
    [Header("Cure Action")]
    public City targetCity;
    
    void Awake()
    {
        actionName = "Curar";
        description = "Reduzir o nível de infecção de uma cidade em 1";
        actionCost = 1;
        requiresTarget = true;
    }
    
    protected override bool CanExecute()
    {
        if (!base.CanExecute())
            return false;
        
        if (targetCity == null)
        {
            Debug.LogWarning("[CureAction] Cidade alvo não definida!");
            return false;
        }
        
        // Verificar se o jogador está na cidade alvo
        if (currentPlayer.currentLocation != targetCity)
        {
            Debug.LogWarning($"[CureAction] Jogador deve estar na cidade {targetCity.cityName} para curá-la!");
            return false;
        }
        
        // Verificar se a cidade precisa ser curada
        if (targetCity.infectionLevel <= 0)
        {
            Debug.LogWarning($"[CureAction] Cidade {targetCity.cityName} já está sem infecção!");
            return false;
        }
        
        return true;
    }
    
    protected override void ExecuteActionLogic()
    {
        Debug.Log($"[CureAction] Curando cidade {targetCity.cityName}");
        
        currentPlayer.CureCity(targetCity);
        targetCity.Cure();
    }
}

// Ação específica: Comprar carta
public class DrawCardAction : Action
{
    [Header("Draw Card Action")]
    public Card cardToDraw;
    
    void Awake()
    {
        actionName = "Comprar Carta";
        description = "Comprar uma carta do baralho";
        actionCost = 1;
        requiresTarget = false;
    }
    
    protected override bool CanExecute()
    {
        if (!base.CanExecute())
            return false;
        
        if (cardToDraw == null)
        {
            Debug.LogWarning("[DrawCardAction] Carta para comprar não definida!");
            return false;
        }
        
        if (!currentPlayer.CanDrawCard())
        {
            Debug.LogWarning($"[DrawCardAction] Jogador não pode comprar mais cartas. Mão cheia ({currentPlayer.hand.Count}/{currentPlayer.maxHandSize})!");
            return false;
        }
        
        return true;
    }
    
    protected override void ExecuteActionLogic()
    {
        Debug.Log($"[DrawCardAction] Comprando carta {cardToDraw.cardName}");
        
        currentPlayer.DrawCard(cardToDraw);
    }
}

// Ação específica: Descartar carta
public class DiscardCardAction : Action
{
    [Header("Discard Card Action")]
    public Card cardToDiscard;
    
    void Awake()
    {
        actionName = "Descartar Carta";
        description = "Descartar uma carta da mão";
        actionCost = 1;
        requiresTarget = true;
    }
    
    protected override bool CanExecute()
    {
        if (!base.CanExecute())
            return false;
        
        if (cardToDiscard == null)
        {
            Debug.LogWarning("[DiscardCardAction] Carta para descartar não definida!");
            return false;
        }
        
        if (!currentPlayer.hand.Contains(cardToDiscard))
        {
            Debug.LogWarning($"[DiscardCardAction] Carta {cardToDiscard.cardName} não está na mão do jogador!");
            return false;
        }
        
        return true;
    }
    
    protected override void ExecuteActionLogic()
    {
        Debug.Log($"[DiscardCardAction] Descartando carta {cardToDiscard.cardName}");
        
        currentPlayer.DiscardCard(cardToDiscard);
    }
} 