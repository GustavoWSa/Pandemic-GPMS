using UnityEngine;

public class CityCard : Card
{
    [Header("City Card Settings")]
    public string regionName;
    public string effectType = "city"; // pode ser "travel", "cure", etc.

    public override void ActivateEffect(GameManager gameManagerInstance)
    {
        Debug.Log($"Ativando carta de cidade '{cardName}' para região '{regionName}' com efeito '{effectType}'");

        RegionController targetRegion = gameManagerInstance.FindRegionByName(this.regionName);

        if (targetRegion != null)
        {
            if (effectType == "travel")
            {
                // Código para permitir que o jogador viaje para essa região
                Debug.Log($"Jogador pode viajar para {regionName}");
                // Aqui você implementa a lógica de viagem
            }
            else if (effectType == "cure")
            {
                // Se o jogador tiver 2 cartas da mesma cidade, pode curar 1 cubo na região
                Debug.Log($"Jogador pode curar 1 cubo na região {regionName}");
                targetRegion.Cure();  // Implemente esse método na RegionController
            }
            else
            {
                Debug.LogWarning("Tipo de efeito desconhecido na carta de cidade");
            }
        }
        else
        {
            Debug.LogWarning($"Região '{regionName}' não encontrada no GameManager");
        }
    }
}
