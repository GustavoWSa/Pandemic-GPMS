/*
using UnityEngine;

public class EventCard : Card
{
    [Header("Event Card Settings")]
    public string effectType;  // Ex: "extraAction", "stopOutbreak", "removeCubes", etc.

    public override void ActivateEffect(GameManager gameManagerInstance)
    {
        Debug.Log($"Ativando carta de evento '{cardName}' com efeito '{effectType}'");

        switch (effectType)
        {
            case "extraAction":
                // Código para dar uma ação extra ao jogador
                Debug.Log("Jogador recebeu uma ação extra!");
                break;

            case "stopOutbreak":
                // Código para prevenir o próximo surto
                Debug.Log("Próximo surto será prevenido!");
                break;

            case "removeCubes":
                // Código para remover cubos de uma região (pode precisar de uma variável extra para a região)
                Debug.Log("Removendo cubos de uma região...");
                // Pode precisar pedir uma região, ou aplicar efeito global
                break;

            default:
                Debug.LogWarning("Efeito desconhecido na carta de evento");
                break;
        }
    }
}
*/