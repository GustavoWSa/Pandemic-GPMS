// Arquivo: GameManager.cs
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [Header("Referências")]
    public CardManager cardManager;

    [Header("Estado do Jogo")]
    public int infectionRateValue = 2;
    public int[] infectionRateTrack = { 2, 2, 2, 3, 3, 4, 4 };
    public int infectionRateTrackIndex = 0;
    public int outbreakCounter = 0;

    private Dictionary<string, RegionController> regioesMap;

    void Start()
    {
        // ... seu código Start ...
        RegionController[] regioes = FindObjectsByType<RegionController>(FindObjectsSortMode.None);
        regioesMap = regioes.ToDictionary(r => r.regionName, r => r);
    }
    
    public RegionController FindRegionByName(string name)
    {
        regioesMap.TryGetValue(name, out RegionController region);
        return region;
    }
    
    public void IncrementOutbreakCounter()
    {
        outbreakCounter++;
        Debug.LogWarning($"[GameManager] Contador de Surtos aumentado para: {outbreakCounter}");
        if (outbreakCounter >= 8) { Debug.LogError("LIMITE DE SURTOS ATINGIDO! FIM DE JOGO."); }
    }

    public void PassarRodada()
    {   
        Debug.LogError("--- TESTE DO BOTÃO: PassarRodada() FOI CHAMADO! ---");
        Debug.Log(" PASSO 1: O botão foi clicado e `PassarRodada` no GameManager foi chamado.");

        if (cardManager == null)
        {
            Debug.LogError("PROBLEMA: A referência ao `cardManager` no GameManager está NULA. Arraste o objeto no Inspector.");
            return;
        }

        Debug.Log($" PASSO 2: Iniciando fase de infecção. Taxa de Infecção é {infectionRateValue}. O loop vai rodar {infectionRateValue} vezes.");
        
        for (int i = 0; i < infectionRateValue; i++)
        {
            Debug.Log($" PASSO 3 (Loop {i + 1}/{infectionRateValue}): Chamando `PlayNextInfectionCard` no CardManager.");
            cardManager.PlayNextInfectionCard(this);
            Debug.Log($" PASSO 3.1 (Loop {i + 1}/{infectionRateValue}): Retornou da chamada ao CardManager.");
        }
        
        Debug.Log("FIM: Fase de Infecção no GameManager concluída.");
    }
}