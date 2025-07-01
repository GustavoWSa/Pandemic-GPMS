// Arquivo: CardManager.cs
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [Header("Deck Settings")]
    public List<Card> deck = new List<Card>();
    public bool shuffleOnStart = true;

    private void Start()
    {
        if (shuffleOnStart) { ShuffleDeck(); }
    }

    public void PlayNextInfectionCard(GameManager gameManagerInstance)
    {
        Debug.Log(" PASSO 4: `PlayNextInfectionCard` no CardManager foi chamado.");

        if (deck.Count == 0)
        {
            Debug.LogWarning("PROBLEMA: O baralho de infecção está vazio. Não há cartas para comprar.");
            return;
        }
        Debug.Log($" PASSO 5: O baralho tem {deck.Count} cartas. Pegando a do topo.");

        Card topCard = deck[0];
        deck.RemoveAt(0);
        
        if (topCard == null)
        {
            Debug.LogError("PROBLEMA: A carta pega do baralho é NULA. Verifique se os slots no deck no Inspector estão preenchidos.");
            return;
        }

        Debug.Log($" PASSO 6: A carta '{topCard.cardName}' foi pega. Chamando `ActivateEffect` nela.");
        topCard.ActivateEffect(gameManagerInstance);
        Debug.Log(" PASSO 6.1: Retornou da chamada `ActivateEffect`.");
    }
    
    public void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Card temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }
}