using System.Collections.Generic;
using UnityEngine;

public class GenericInfectionCard : Card
{
    // Dentro de GenericInfectionCard.cs
// ATUALIZE A ASSINATURA DO MÉTODO para receber o HashSet
public override void ActivateEffect(GameManager gameManagerInstance, HashSet<RegionController> outbrokenThisTurn)
{
    Debug.Log($"CARTA GENÉRICA: Pedindo ao GameManager para sortear uma região com pesos...");

    RegionController targetRegion = gameManagerInstance.GetRegionToInfectByWeight();

    if (targetRegion != null)
    {
        Debug.Log($"O GameManager sorteou a região de alto risco: {targetRegion.regionName}.");

        // NÃO CRIE UM NOVO HASHSET AQUI. USE O QUE VEIO COMO PARÂMETRO.
        targetRegion.AddCubeAndHandleOutbreak(gameManagerInstance, outbrokenThisTurn);
    }
}
}