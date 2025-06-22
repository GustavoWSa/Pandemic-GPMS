using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Player
{
    [Header("Player Info")]
    public string playerName = "Jogador";
    public int actionsRemaining = 4;
    public int maxActionsPerTurn = 4;
    
    [Header("Player State")]
    public City currentLocation;
    public List<Card> hand = new List<Card>();
    public int maxHandSize = 7;
    
    [Header("Player Stats")]
    public int turnsPlayed = 0;
    public int totalActionsUsed = 0;
    public int citiesCured = 0;
    
    public Player()
    {
        Debug.Log($"[Player] Jogador '{playerName}' criado com {maxActionsPerTurn} ações por rodada.");
    }
    
    public bool CanPerformAction()
    {
        return actionsRemaining > 0;
    }
    
    public void UseAction()
    {
        if (actionsRemaining > 0)
        {
            actionsRemaining--;
            totalActionsUsed++;
            Debug.Log($"[Player] Ação usada por '{playerName}'. Ações restantes: {actionsRemaining}");
        }
        else
        {
            Debug.LogWarning($"[Player] Tentativa de usar ação sem ações disponíveis para '{playerName}'!");
        }
    }
    
    public void ResetActions()
    {
        actionsRemaining = maxActionsPerTurn;
        Debug.Log($"[Player] Ações de '{playerName}' resetadas para {maxActionsPerTurn}.");
    }
    
    public void MoveToCity(City destination)
    {
        if (destination == null)
        {
            Debug.LogWarning($"[Player] Tentativa de mover '{playerName}' para cidade nula!");
            return;
        }
        
        currentLocation = destination;
        Debug.Log($"[Player] '{playerName}' moveu-se para a cidade {destination.cityName}.");
    }
    
    public bool CanDrawCard()
    {
        return hand.Count < maxHandSize;
    }
    
    public void DrawCard(Card card)
    {
        if (card == null)
        {
            Debug.LogWarning($"[Player] Tentativa de comprar carta nula para '{playerName}'!");
            return;
        }
        
        if (CanDrawCard())
        {
            hand.Add(card);
            Debug.Log($"[Player] '{playerName}' comprou a carta '{card.cardName}'. Cartas na mão: {hand.Count}/{maxHandSize}");
        }
        else
        {
            Debug.LogWarning($"[Player] '{playerName}' não pode comprar mais cartas. Mão cheia ({hand.Count}/{maxHandSize})!");
        }
    }
    
    public void DiscardCard(Card card)
    {
        if (hand.Contains(card))
        {
            hand.Remove(card);
            Debug.Log($"[Player] '{playerName}' descartou a carta '{card.cardName}'. Cartas na mão: {hand.Count}/{maxHandSize}");
        }
        else
        {
            Debug.LogWarning($"[Player] Tentativa de descartar carta que não está na mão de '{playerName}'!");
        }
    }
    
    public void CureCity(City city)
    {
        if (city == null)
        {
            Debug.LogWarning($"[Player] Tentativa de curar cidade nula por '{playerName}'!");
            return;
        }
        
        if (city.infectionLevel > 0)
        {
            city.infectionLevel--;
            citiesCured++;
            Debug.Log($"[Player] '{playerName}' curou a cidade {city.cityName}. Novo nível: {city.infectionLevel}/{city.maxInfectionLevel}");
        }
        else
        {
            Debug.Log($"[Player] '{playerName}' tentou curar a cidade {city.cityName}, mas ela já está curada.");
        }
    }
    
    public void EndTurn()
    {
        turnsPlayed++;
        Debug.Log($"[Player] '{playerName}' finalizou a rodada {turnsPlayed}. Total de ações usadas: {totalActionsUsed}");
    }
    
    public string GetPlayerStatus()
    {
        return $"Jogador: {playerName} | Ações: {actionsRemaining}/{maxActionsPerTurn} | Localização: {(currentLocation != null ? currentLocation.cityName : "Nenhuma")} | Cartas: {hand.Count}/{maxHandSize}";
    }
} 