// Arquivo: CardManager.cs
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [Header("Deck Settings")]
    [Tooltip("Agora, esta lista funciona como um CATÁLOGO de tipos de cartas possíveis.")]
    public List<Card> deck = new List<Card>(); // Esta é a sua lista de "tipos"
    public bool shuffleOnStart = true;

    private void Start()
    {
        // Embaralhar o catálogo pode ser útil se você for pegar em sequência, mas para sorteio não faz diferença.
        if (shuffleOnStart) { ShuffleDeck(); }
    }

    /// <summary>
    /// Este método foi MODIFICADO. Agora ele sorteia um TIPO de carta do deck,
    /// ativa seu efeito, mas NÃO remove a carta da lista.
    /// </summary>
    // Dentro de CardManager.cs
// ATUALIZE A ASSINATURA DO MÉTODO para receber o HashSet
public void PlayNextInfectionCard(GameManager gameManagerInstance, HashSet<RegionController> outbrokenThisTurn)
{
    if (deck.Count == 0)
    {
        Debug.LogWarning("PROBLEMA: O catálogo de cartas está vazio.");
        return;
    }
    
    int randomIndex = Random.Range(0, deck.Count);
    Card randomCardType = deck[randomIndex];

    if (randomCardType != null)
    {
        // PASSA A MESMA LISTA PARA O MÉTODO DA CARTA
        randomCardType.ActivateEffect(gameManagerInstance, outbrokenThisTurn);
    }
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