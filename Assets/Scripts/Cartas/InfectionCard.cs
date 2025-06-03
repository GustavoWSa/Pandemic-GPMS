using UnityEngine;

public class InfectionCard : Card
{
    [Header("Infection Card Settings")]
    public string regionName;
    public string effectType = "infect";

    public override void ActivateEffect()
    {
        Debug.Log($"[InfectionCard] Carta '{cardName}' - ActivateEffect chamado. Alvo: '{regionName}', Efeito: '{effectType}'.");

        Debug.Log($"[InfectionCard] Procurando todos os RegionControllers na cena...");
        // RegionController[] allRegions = FindObjectsOfType<RegionController>(); // Linha obsoleta
        RegionController[] allRegions = FindObjectsByType<RegionController>(FindObjectsSortMode.None); // API Atualizada
        Debug.Log($"[InfectionCard] Encontrados {allRegions.Length} RegionControllers.");

        bool regionFoundAndEffectApplied = false;
        foreach (RegionController region in allRegions)
        {
            if (region == null) continue;

            if (region.regionName.Equals(this.regionName, System.StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log($"[InfectionCard] Região alvo '{this.regionName}' encontrada (Objeto: {region.gameObject.name}). Aplicando efeito '{effectType}'.");
                region.ApplyEffect(effectType);
                Debug.Log($"[InfectionCard] Carta '{cardName}' - Efeito '{effectType}' aplicado com sucesso à região '{this.regionName}'.");
                regionFoundAndEffectApplied = true;
                return;
            }
        }

        if (!regionFoundAndEffectApplied)
        {
            Debug.LogWarning($"[InfectionCard] Carta '{cardName}' - Região alvo '{this.regionName}' não encontrada na cena para aplicar o efeito '{effectType}'.");
        }
    }
}