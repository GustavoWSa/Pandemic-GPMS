// Arquivo: InfectionCard.cs
using System.Collections.Generic;
using UnityEngine;

public class InfectionCard : Card
{
    [Header("Infection Card Settings")]
    public string regionName;
    public string effectType = "infect";

    public override void ActivateEffect(GameManager gameManagerInstance)
    {
        Debug.Log($" PASSO 7: `ActivateEffect` da carta '{cardName}' foi chamado. Tentando encontrar a região '{this.regionName}'.");

        RegionController targetRegion = gameManagerInstance.FindRegionByName(this.regionName);

        if (targetRegion != null)
        {
            Debug.Log($" PASSO 8: Região '{this.regionName}' encontrada! Chamando `AddCubeAndHandleOutbreak`.");
            HashSet<RegionController> outbrokenThisAction = new HashSet<RegionController>();
            targetRegion.AddCubeAndHandleOutbreak(gameManagerInstance, outbrokenThisAction);
            Debug.Log(" PASSO 8.1: Retornou de `AddCubeAndHandleOutbreak`.");
        }
        else
        {
            Debug.LogWarning($"PROBLEMA: A região '{this.regionName}' não foi encontrada pelo GameManager.");
        }
    }
}