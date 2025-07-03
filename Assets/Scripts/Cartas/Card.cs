using System.Collections.Generic; // Adicione esta linha se não estiver lá
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    public string cardName;

    // A assinatura do método agora inclui o HashSet para controle de surtos.
    // Este é o novo "contrato" que todas as cartas filhas devem seguir.
    public abstract void ActivateEffect(GameManager gameManager, HashSet<RegionController> outbrokenThisTurn);
}