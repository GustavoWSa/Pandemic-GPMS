// Arquivo: Card.cs
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    public string cardName;
    public abstract void ActivateEffect(GameManager gameManager);
}