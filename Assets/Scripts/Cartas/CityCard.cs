using UnityEngine;

public class CityCard : Card
{
    [Header("City Card Settings")]
    public string regionName;
    public string effectType = "city"; // pode ser "travel", "cure", etc.

    public override void ActivateEffect(GameManager gameManagerInstance)
    {
        Debug.Log($"Ativando carta de cidade '{cardName}' para regi�o '{regionName}' com efeito '{effectType}'");

        RegionController targetRegion = gameManagerInstance.FindRegionByName(this.regionName);

        if (targetRegion != null)
        {
            if (effectType == "travel")
            {
                // C�digo para permitir que o jogador viaje para essa regi�o
                Debug.Log($"Jogador pode viajar para {regionName}");
                // Aqui voc� implementa a l�gica de viagem
            }
            else if (effectType == "cure")
            {
                // Se o jogador tiver 2 cartas da mesma cidade, pode curar 1 cubo na regi�o
                Debug.Log($"Jogador pode curar 1 cubo na regi�o {regionName}");
                targetRegion.Cure();  // Implemente esse m�todo na RegionController
            }
            else
            {
                Debug.LogWarning("Tipo de efeito desconhecido na carta de cidade");
            }
        }
        else
        {
            Debug.LogWarning($"Regi�o '{regionName}' n�o encontrada no GameManager");
        }
    }
}
