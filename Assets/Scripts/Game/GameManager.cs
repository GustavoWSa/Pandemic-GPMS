using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement; // --- ADIÇÃO DE UI --- Importante para trocar de cena
using TMPro;                       // --- ADIÇÃO DE UI --- Importante para controlar o texto

public class GameManager : MonoBehaviour
{
    [Header("Referências")]
    public CardManager cardManager;
    
    // --- ADIÇÃO DE UI 1: REFERÊNCIAS VISUAIS ---
    [Header("Referências de UI")]
    [Tooltip("Arraste o objeto de texto do contador de surtos para cá.")]
    public TextMeshProUGUI outbreakCounterText;
    [Tooltip("Arraste o painel de Fim de Jogo para cá.")]
    public GameObject gameOverPanel;
    // --- FIM DA ADIÇÃO DE UI 1 ---

    [Header("Estado do Jogo")]
    public int infectionRateValue = 2;
    public int[] infectionRateTrack = { 2, 2, 2, 3, 3, 4, 4 };
    public int infectionRateTrackIndex = 0;
    public int outbreakCounter = 0;

    [Header("High-Risk Pool")]
    [Tooltip("Regiões que foram infectadas recentemente e têm maior chance de serem sorteadas de novo.")]
    public List<RegionController> highRiskRegions = new List<RegionController>();

    private Dictionary<string, RegionController> regioesMap;

    void Start()
    {
        RegionController[] regioes = FindObjectsByType<RegionController>(FindObjectsSortMode.None);
        regioesMap = regioes.ToDictionary(r => r.regionName, r => r);

        // --- ADIÇÃO DE UI 2: INICIALIZAÇÃO DA INTERFACE ---
        // Garante que a tela de Fim de Jogo comece escondida e o contador com o valor certo.
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        Time.timeScale = 1f; // Garante que o jogo não comece pausado.
        UpdateOutbreakUI();
        // --- FIM DA ADIÇÃO DE UI 2 ---
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

        // --- ADIÇÃO DE UI 3: ATUALIZA O VISUAL ---
        UpdateOutbreakUI();

        if (outbreakCounter >= 8) 
        {
            ShowGameOverScreen(); // Mostra a tela de Fim de Jogo
        }
    }
    
    public void PassarRodada()
    {
        if (cardManager == null)
        {
            Debug.LogError("PROBLEMA: A referência ao `cardManager` no GameManager está NULA.");
            return;
        }

        HashSet<RegionController> outbrokenThisTurn = new HashSet<RegionController>();

        for (int i = 0; i < infectionRateValue; i++)
        {
            cardManager.PlayNextInfectionCard(this, outbrokenThisTurn);
        }
    }
    
    public RegionController GetRegionToInfectByWeight()
    {
        if (regioesMap.Count == 0) return null;

        List<RegionController> drawPool = new List<RegionController>();
        drawPool.AddRange(regioesMap.Values);
        drawPool.AddRange(highRiskRegions);
        drawPool.AddRange(highRiskRegions);

        int randomIndex = Random.Range(0, drawPool.Count);
        RegionController chosenRegion = drawPool[randomIndex];

        highRiskRegions.Add(chosenRegion);
        Debug.Log($"[GameManager] A região {chosenRegion.regionName} foi sorteada e adicionada à piscina de alto risco.");

        return chosenRegion;
    }

    // --- ADIÇÃO DE UI 4: NOVOS MÉTODOS PARA CONTROLAR O VISUAL ---
    void UpdateOutbreakUI()
    {
        if (outbreakCounterText == null) return;

        outbreakCounterText.text = outbreakCounter.ToString();

        if (outbreakCounter >= 6) { outbreakCounterText.color = Color.red; }
        else if (outbreakCounter >= 4) { outbreakCounterText.color = Color.yellow; }
        else { outbreakCounterText.color = Color.white; }
    }

    void ShowGameOverScreen()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        Time.timeScale = 0f; // Pausa o jogo
        Debug.LogError("LIMITE DE SURTOS ATINGIDO! FIM DE JOGO.");
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f; // Despausa o jogo antes de trocar de cena
        SceneManager.LoadScene("MenuScene"); // Lembre-se que o nome da cena deve ser exato
    }
    // --- FIM DA ADIÇÃO DE UI 4 ---
}