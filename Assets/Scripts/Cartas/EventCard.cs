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
                // C�digo para dar uma a��o extra ao jogador
                Debug.Log("Jogador recebeu uma a��o extra!");
                break;

            case "stopOutbreak":
                // C�digo para prevenir o pr�ximo surto
                Debug.Log("Pr�ximo surto ser� prevenido!");
                break;

            case "removeCubes":
                // C�digo para remover cubos de uma regi�o (pode precisar de uma vari�vel extra para a regi�o)
                Debug.Log("Removendo cubos de uma regi�o...");
                // Pode precisar pedir uma regi�o, ou aplicar efeito global
                break;

            default:
                Debug.LogWarning("Efeito desconhecido na carta de evento");
                break;
        }
    }
}
*/