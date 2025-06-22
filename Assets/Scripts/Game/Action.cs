using UnityEngine;
using System.Collections.Generic;

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

// ===== AÇÕES ESPECÍFICAS DO PANDEMIC =====

// Ação: Voo Direto (mover para qualquer cidade usando carta da cidade)
public class DirectFlightAction : Action
{
    [Header("Direct Flight Action")]
    public City targetCity;
    public Card cityCard;
    
    void Awake()
    {
        actionName = "Voo Direto";
        description = "Mover para qualquer cidade usando carta da cidade de destino";
        actionCost = 1;
        requiresTarget = true;
    }
    
    protected override bool CanExecute()
    {
        if (!base.CanExecute())
            return false;
        
        if (targetCity == null)
        {
            Debug.LogWarning("[DirectFlightAction] Cidade alvo não definida!");
            return false;
        }
        
        if (cityCard == null)
        {
            Debug.LogWarning("[DirectFlightAction] Carta da cidade não definida!");
            return false;
        }
        
        // Verificar se o jogador tem a carta da cidade de destino
        if (!currentPlayer.hand.Contains(cityCard))
        {
            Debug.LogWarning($"[DirectFlightAction] Jogador não tem a carta da cidade {targetCity.cityName}!");
            return false;
        }
        
        return true;
    }
    
    protected override void ExecuteActionLogic()
    {
        Debug.Log($"[DirectFlightAction] Voo direto para {targetCity.cityName} usando carta {cityCard.cardName}");
        
        // Descartar a carta da cidade
        currentPlayer.DiscardCard(cityCard);
        
        // Mover para a cidade
        currentPlayer.MoveToCity(targetCity);
    }
}

// Ação: Voo Fretado (mover para qualquer cidade descartando carta da cidade atual)
public class CharterFlightAction : Action
{
    [Header("Charter Flight Action")]
    public City targetCity;
    public Card currentCityCard;
    
    void Awake()
    {
        actionName = "Voo Fretado";
        description = "Mover para qualquer cidade descartando carta da cidade atual";
        actionCost = 1;
        requiresTarget = true;
    }
    
    protected override bool CanExecute()
    {
        if (!base.CanExecute())
            return false;
        
        if (targetCity == null)
        {
            Debug.LogWarning("[CharterFlightAction] Cidade alvo não definida!");
            return false;
        }
        
        if (currentCityCard == null)
        {
            Debug.LogWarning("[CharterFlightAction] Carta da cidade atual não definida!");
            return false;
        }
        
        // Verificar se o jogador tem a carta da cidade atual
        if (!currentPlayer.hand.Contains(currentCityCard))
        {
            Debug.LogWarning($"[CharterFlightAction] Jogador não tem a carta da cidade atual!");
            return false;
        }
        
        return true;
    }
    
    protected override void ExecuteActionLogic()
    {
        Debug.Log($"[CharterFlightAction] Voo fretado para {targetCity.cityName} descartando carta {currentCityCard.cardName}");
        
        // Descartar a carta da cidade atual
        currentPlayer.DiscardCard(currentCityCard);
        
        // Mover para a cidade
        currentPlayer.MoveToCity(targetCity);
    }
}

// Ação: Ponte Aérea (mover para cidade com estação de pesquisa)
public class ShuttleFlightAction : Action
{
    [Header("Shuttle Flight Action")]
    public City targetCity;
    
    void Awake()
    {
        actionName = "Ponte Aérea";
        description = "Mover para cidade com estação de pesquisa";
        actionCost = 1;
        requiresTarget = true;
    }
    
    protected override bool CanExecute()
    {
        if (!base.CanExecute())
            return false;
        
        if (targetCity == null)
        {
            Debug.LogWarning("[ShuttleFlightAction] Cidade alvo não definida!");
            return false;
        }
        
        // Verificar se a cidade atual tem estação de pesquisa
        if (currentPlayer.currentLocation == null || !currentPlayer.currentLocation.hasResearchStation)
        {
            Debug.LogWarning("[ShuttleFlightAction] Cidade atual não tem estação de pesquisa!");
            return false;
        }
        
        // Verificar se a cidade alvo tem estação de pesquisa
        if (!targetCity.hasResearchStation)
        {
            Debug.LogWarning($"[ShuttleFlightAction] Cidade {targetCity.cityName} não tem estação de pesquisa!");
            return false;
        }
        
        return true;
    }
    
    protected override void ExecuteActionLogic()
    {
        Debug.Log($"[ShuttleFlightAction] Ponte aérea para {targetCity.cityName}");
        
        currentPlayer.MoveToCity(targetCity);
    }
}

// Ação: Balsa/Automóvel (mover para cidade conectada)
public class DriveFerryAction : Action
{
    [Header("Drive/Ferry Action")]
    public City targetCity;
    
    void Awake()
    {
        actionName = "Balsa/Automóvel";
        description = "Mover para cidade conectada";
        actionCost = 1;
        requiresTarget = true;
    }
    
    protected override bool CanExecute()
    {
        if (!base.CanExecute())
            return false;
        
        if (targetCity == null)
        {
            Debug.LogWarning("[DriveFerryAction] Cidade alvo não definida!");
            return false;
        }
        
        // Verificar se a cidade alvo está conectada à cidade atual
        if (currentPlayer.currentLocation != null)
        {
            if (!currentPlayer.currentLocation.IsConnectedTo(targetCity.cityId))
            {
                Debug.LogWarning($"[DriveFerryAction] Cidade {targetCity.cityName} não está conectada à cidade atual {currentPlayer.currentLocation.cityName}!");
                return false;
            }
        }
        
        return true;
    }
    
    protected override void ExecuteActionLogic()
    {
        Debug.Log($"[DriveFerryAction] Movendo de {currentPlayer.currentLocation.cityName} para {targetCity.cityName}");
        
        currentPlayer.MoveToCity(targetCity);
    }
}

// Ação: Tratar Doenças (remover 1 cubo de infecção)
public class TreatDiseaseAction : Action
{
    [Header("Treat Disease Action")]
    public City targetCity;
    
    void Awake()
    {
        actionName = "Tratar Doenças";
        description = "Remover 1 cubo de infecção da cidade atual";
        actionCost = 1;
        requiresTarget = false;
    }
    
    protected override bool CanExecute()
    {
        if (!base.CanExecute())
            return false;
        
        if (currentPlayer.currentLocation == null)
        {
            Debug.LogWarning("[TreatDiseaseAction] Jogador não está em nenhuma cidade!");
            return false;
        }
        
        // Verificar se a cidade tem infecção para tratar
        if (currentPlayer.currentLocation.infectionLevel <= 0)
        {
            Debug.LogWarning($"[TreatDiseaseAction] Cidade {currentPlayer.currentLocation.cityName} não tem infecção para tratar!");
            return false;
        }
        
        return true;
    }
    
    protected override void ExecuteActionLogic()
    {
        Debug.Log($"[TreatDiseaseAction] Tratando doença em {currentPlayer.currentLocation.cityName}");
        
        currentPlayer.CureCity(currentPlayer.currentLocation);
    }
}

// Ação: Construir Estação de Pesquisa
public class BuildResearchStationAction : Action
{
    [Header("Build Research Station Action")]
    public Card cityCard;
    
    void Awake()
    {
        actionName = "Construir Estação de Pesquisa";
        description = "Construir estação de pesquisa na cidade atual usando carta da cidade";
        actionCost = 1;
        requiresTarget = false;
    }
    
    protected override bool CanExecute()
    {
        if (!base.CanExecute())
            return false;
        
        if (currentPlayer.currentLocation == null)
        {
            Debug.LogWarning("[BuildResearchStationAction] Jogador não está em nenhuma cidade!");
            return false;
        }
        
        if (cityCard == null)
        {
            Debug.LogWarning("[BuildResearchStationAction] Carta da cidade não definida!");
            return false;
        }
        
        // Verificar se o jogador tem a carta da cidade atual
        if (!currentPlayer.hand.Contains(cityCard))
        {
            Debug.LogWarning($"[BuildResearchStationAction] Jogador não tem a carta da cidade atual!");
            return false;
        }
        
        // Verificar se a cidade já tem estação de pesquisa
        if (currentPlayer.currentLocation.hasResearchStation)
        {
            Debug.LogWarning($"[BuildResearchStationAction] Cidade {currentPlayer.currentLocation.cityName} já tem estação de pesquisa!");
            return false;
        }
        
        return true;
    }
    
    protected override void ExecuteActionLogic()
    {
        Debug.Log($"[BuildResearchStationAction] Construindo estação de pesquisa em {currentPlayer.currentLocation.cityName}");
        
        // Descartar a carta da cidade
        currentPlayer.DiscardCard(cityCard);
        
        // Construir estação de pesquisa
        currentPlayer.currentLocation.BuildResearchStation();
    }
}

// Ação: Descobrir Cura
public class DiscoverCureAction : Action
{
    [Header("Discover Cure Action")]
    public List<Card> cardsToDiscard = new List<Card>();
    public string diseaseType;
    
    void Awake()
    {
        actionName = "Descobrir Cura";
        description = "Descobrir cura para uma doença usando 5 cartas da mesma cor";
        actionCost = 1;
        requiresTarget = false;
    }
    
    protected override bool CanExecute()
    {
        if (!base.CanExecute())
            return false;
        
        if (currentPlayer.currentLocation == null)
        {
            Debug.LogWarning("[DiscoverCureAction] Jogador não está em nenhuma cidade!");
            return false;
        }
        
        // Verificar se o jogador está em uma cidade com estação de pesquisa
        if (!currentPlayer.currentLocation.hasResearchStation)
        {
            Debug.LogWarning($"[DiscoverCureAction] Jogador deve estar em cidade com estação de pesquisa!");
            return false;
        }
        
        // Verificar se tem 5 cartas para descartar
        if (cardsToDiscard.Count != 5)
        {
            Debug.LogWarning($"[DiscoverCureAction] É necessário descartar exatamente 5 cartas! ({cardsToDiscard.Count} selecionadas)");
            return false;
        }
        
        // Verificar se todas as cartas são da mesma cor
        if (string.IsNullOrEmpty(diseaseType))
        {
            Debug.LogWarning("[DiscoverCureAction] Tipo de doença não definido!");
            return false;
        }
        
        // Verificar se o jogador tem todas as cartas
        foreach (Card card in cardsToDiscard)
        {
            if (!currentPlayer.hand.Contains(card))
            {
                Debug.LogWarning($"[DiscoverCureAction] Carta {card.cardName} não está na mão do jogador!");
                return false;
            }
        }
        
        return true;
    }
    
    protected override void ExecuteActionLogic()
    {
        Debug.Log($"[DiscoverCureAction] Descobrindo cura para doença {diseaseType}");
        
        // Descartar as 5 cartas
        foreach (Card card in cardsToDiscard)
        {
            currentPlayer.DiscardCard(card);
        }
        
        // Marcar doença como curada (será implementado no GameManager)
        // gameManager.CureDisease(diseaseType);
    }
}

// Ação: Compartilhar Conhecimento
public class ShareKnowledgeAction : Action
{
    [Header("Share Knowledge Action")]
    public Player targetPlayer;
    public Card cardToShare;
    
    void Awake()
    {
        actionName = "Compartilhar Conhecimento";
        description = "Dar ou receber carta de cidade de outro jogador na mesma cidade";
        actionCost = 1;
        requiresTarget = true;
    }
    
    protected override bool CanExecute()
    {
        if (!base.CanExecute())
            return false;
        
        if (targetPlayer == null)
        {
            Debug.LogWarning("[ShareKnowledgeAction] Jogador alvo não definido!");
            return false;
        }
        
        if (cardToShare == null)
        {
            Debug.LogWarning("[ShareKnowledgeAction] Carta para compartilhar não definida!");
            return false;
        }
        
        // Verificar se ambos os jogadores estão na mesma cidade
        if (currentPlayer.currentLocation != targetPlayer.currentLocation)
        {
            Debug.LogWarning("[ShareKnowledgeAction] Ambos os jogadores devem estar na mesma cidade!");
            return false;
        }
        
        // Verificar se o jogador atual tem a carta
        if (!currentPlayer.hand.Contains(cardToShare))
        {
            Debug.LogWarning($"[ShareKnowledgeAction] Jogador atual não tem a carta {cardToShare.cardName}!");
            return false;
        }
        
        // Verificar se o jogador alvo pode receber a carta
        if (!targetPlayer.CanDrawCard())
        {
            Debug.LogWarning($"[ShareKnowledgeAction] Jogador alvo não pode receber mais cartas!");
            return false;
        }
        
        return true;
    }
    
    protected override void ExecuteActionLogic()
    {
        Debug.Log($"[ShareKnowledgeAction] Compartilhando carta {cardToShare.cardName} com {targetPlayer.playerName}");
        
        // Remover carta do jogador atual
        currentPlayer.DiscardCard(cardToShare);
        
        // Adicionar carta ao jogador alvo
        targetPlayer.DrawCard(cardToShare);
    }
}

// Ação: Remover Doença Máxima (remover todos os cubos de uma cor)
public class RemoveAllDiseaseAction : Action
{
    [Header("Remove All Disease Action")]
    public City targetCity;
    public string diseaseType;
    
    void Awake()
    {
        actionName = "Remover Doença Máxima";
        description = "Remover todos os cubos de uma cor da cidade atual";
        actionCost = 1;
        requiresTarget = false;
    }
    
    protected override bool CanExecute()
    {
        if (!base.CanExecute())
            return false;
        
        if (currentPlayer.currentLocation == null)
        {
            Debug.LogWarning("[RemoveAllDiseaseAction] Jogador não está em nenhuma cidade!");
            return false;
        }
        
        if (string.IsNullOrEmpty(diseaseType))
        {
            Debug.LogWarning("[RemoveAllDiseaseAction] Tipo de doença não definido!");
            return false;
        }
        
        // Verificar se a cidade tem infecção do tipo especificado
        if (currentPlayer.currentLocation.infectionLevel <= 0)
        {
            Debug.LogWarning($"[RemoveAllDiseaseAction] Cidade {currentPlayer.currentLocation.cityName} não tem infecção para remover!");
            return false;
        }
        
        return true;
    }
    
    protected override void ExecuteActionLogic()
    {
        Debug.Log($"[RemoveAllDiseaseAction] Removendo toda infecção {diseaseType} de {currentPlayer.currentLocation.cityName}");
        
        // Remover toda a infecção da cidade
        currentPlayer.currentLocation.ResetInfection();
    }
} 