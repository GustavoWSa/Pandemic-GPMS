using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [Header("Deck Settings")]
    public List<Card> deck = new List<Card>();
    public bool shuffleOnStart = true;

    private void Start()
    {
        Debug.Log("[CardManager] Iniciando CardManager...");
        if (shuffleOnStart)
        {
            ShuffleDeck(); // O método ShuffleDeck já tem seus logs.
        }
        else
        {
            Debug.Log("[CardManager] Embaralhamento inicial desativado.");
        }
        Debug.Log($"[CardManager] Contagem inicial de cartas no deck: {deck.Count}");
    }

    public void PlayNextCard()
    {   
        Debug.LogError("--- PlayNextCard() FOI CHAMADO! ---");
        Debug.Log("[CardManager] Método PlayNextCard() chamado.");
        if (deck.Count == 0)
        {
            Debug.LogWarning("[CardManager] Deck está vazio. Nenhuma carta para jogar.");
            return;
        }

        Card topCard = deck[0];
        if (topCard == null)
        {
            Debug.LogError("[CardManager] A carta no topo do deck é nula! Removendo-a para evitar erros.");
            deck.RemoveAt(0);
            return;
        }

        Debug.Log($"[CardManager] Jogando a carta: '{topCard.cardName}'. Ativando seu efeito.");
        topCard.ActivateEffect(); // O efeito da carta pode ter seus próprios logs.
        
        deck.RemoveAt(0);
        Debug.Log($"[CardManager] Carta '{topCard.cardName}' removida do deck. Cartas restantes: {deck.Count}");
    }

    public void ShuffleDeck()
    {
        Debug.Log($"[CardManager] Embaralhando o deck com {deck.Count} cartas.");
        for (int i = 0; i < deck.Count; i++)
        {
            Card temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
        Debug.Log("[CardManager] Deck embaralhado com sucesso.");
    }

    public void AddCard(Card card)
    {
        if (card == null)
        {
            Debug.LogWarning("[CardManager] Tentativa de adicionar uma carta nula ao deck.");
            return;
        }
        Debug.Log($"[CardManager] Adicionando carta '{card.cardName}' ao deck.");
        deck.Add(card);
        Debug.Log($"[CardManager] Carta '{card.cardName}' adicionada. Nova contagem do deck: {deck.Count}");
    }

    public void ResetDeck(List<Card> newDeck)
    {
        if (newDeck == null)
        {
            Debug.LogWarning("[CardManager] Tentativa de resetar o deck com uma lista nula. Operação cancelada.");
            return;
        }
        Debug.Log($"[CardManager] Resetando o deck com {newDeck.Count} novas cartas.");
        deck = new List<Card>(newDeck); // Cria uma nova lista para evitar modificar a original se ela for usada em outro lugar.
        Debug.Log($"[CardManager] Deck resetado. Contagem atual antes de embaralhar: {deck.Count}");
        ShuffleDeck();
    }
}