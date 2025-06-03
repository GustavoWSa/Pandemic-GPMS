using UnityEngine;

public abstract class Card : MonoBehaviour
{
    public string cardName;

    // Método abstrato: cada tipo de carta implementa sua lógica específica.
    public abstract void ActivateEffect();

    // Exemplo de log que poderia ser adicionado se houvesse um método comum:
    // protected virtual void Awake()
    // {
    //    if (string.IsNullOrEmpty(cardName))
    //    {
    //        Debug.LogWarning($"[Card] Uma carta (Objeto: {gameObject.name}) está iniciando sem um cardName definido no Inspector.");
    //    }
    //    else
    //    {
    //        Debug.Log($"[Card] Carta '{cardName}' (Objeto: {gameObject.name}) está iniciando (Awake).");
    //    }
    // }
}