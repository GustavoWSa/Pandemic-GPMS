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

    // --- NOVA ADIÇÃO 1: A LISTA DE ALTO RISCO ---
    [Header("High-Risk Pool")]
    [Tooltip("Regiões que foram infectadas recentemente e têm maior chance de serem sorteadas de novo.")]
    public List<RegionController> highRiskRegions = new List<RegionController>();
    // --- FIM DA NOVA ADIÇÃO 1 ---

    private Dictionary<string, RegionController> regioesMap;

    void Start()
    {
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

    if (cardManager == null)
    {
        Debug.LogError("PROBLEMA: A referência ao `cardManager` no GameManager está NULA.");
        return;
    }

    // CRIA A LISTA DE CONTROLE AQUI, UMA VEZ POR RODADA!
    HashSet<RegionController> outbrokenThisTurn = new HashSet<RegionController>();

    Debug.Log($" PASSO 2: Iniciando fase de infecção. Taxa de Infecção é {infectionRateValue}.");

    for (int i = 0; i < infectionRateValue; i++)
    {
        // PASSA A LISTA DE CONTROLE PARA O MÉTODO
        cardManager.PlayNextInfectionCard(this, outbrokenThisTurn);
    }

    Debug.Log("FIM: Fase de Infecção no GameManager concluída.");
}

    // --- NOVA ADIÇÃO 2: O MÉTODO DE SORTEIO INTELIGENTE ---
    /// <summary>
    /// Sorteia uma região para ser infectada, dando mais chance para as que estão na lista de alto risco.
    /// </summary>
    public RegionController GetRegionToInfectByWeight()
    {
        // Se não houver nenhuma região no mapa, retorna nulo para evitar erros.
        if (regioesMap.Count == 0) return null;

        // 1. Cria uma "piscina de sorteio" temporária.
        List<RegionController> drawPool = new List<RegionController>();

        // 2. Adiciona TODAS as regiões do mapa na piscina. Usamos .Values do seu dicionário.
        drawPool.AddRange(regioesMap.Values);

        // 3. Adiciona as regiões de ALTO RISCO na piscina NOVAMENTE, aumentando o peso delas.
        drawPool.AddRange(highRiskRegions);
        drawPool.AddRange(highRiskRegions); // Adicionando mais uma vez para um efeito mais forte.

        // 4. Sorteia um "ticket" da piscina.
        int randomIndex = Random.Range(0, drawPool.Count);
        RegionController chosenRegion = drawPool[randomIndex];

        // 5. Simula colocar a carta na "pilha de descarte" para aumentar a chance futura.
        highRiskRegions.Add(chosenRegion);
        Debug.Log($"[GameManager] A região {chosenRegion.regionName} foi sorteada e adicionada à piscina de alto risco.");

        return chosenRegion;
    }
    // --- FIM DA NOVA ADIÇÃO 2 ---
}