using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RegionController : MonoBehaviour
{
    [Header("Region Info")]
    public string regionName;
    public int infectionLevel = 0;
    public int maxInfectionLevel = 3;
    public TextMeshProUGUI textMesh;
    public List<RegionController> adjacentRegions = new List<RegionController>();

    void Start()
    {
        if (string.IsNullOrEmpty(regionName)) { regionName = gameObject.name; }
        
        // Esta linha tenta encontrar o texto automaticamente se você não o arrastou no Inspector.
        // GetComponentInChildren é mais robusto, pois procura nos objetos filhos.
        if (textMesh == null) { textMesh = GetComponentInChildren<TextMeshProUGUI>(); }

        UpdateText();
    }

    public void AddCubeAndHandleOutbreak(GameManager gameManager, HashSet<RegionController> outbrokenRegionsInCurrentChain)
    {
        Debug.Log($" PASSO 9: `AddCubeAndHandleOutbreak` foi chamado na região '{this.regionName}'. Nível de infecção atual: {infectionLevel}.");
        
        if (outbrokenRegionsInCurrentChain.Contains(this))
        {
            Debug.Log($" AVISO: A região '{this.regionName}' já sofreu surto nesta cadeia de eventos. Ignorando.");
            return;
        }

        if (infectionLevel < maxInfectionLevel)
        {
            infectionLevel++;
            Debug.Log($" PASSO 10: Nível de infecção de '{this.regionName}' aumentado para {infectionLevel}. Chamando `UpdateText`.");
            
            // --- CÓDIGO DE INVESTIGAÇÃO ---
            // Ele checa se a referência ao texto é nula ANTES de tentar usá-la.
            if (this.textMesh == null)
            {
                Debug.LogError($"[INVESTIGAÇÃO] Na região '{this.regionName}', o campo 'textMesh' ESTÁ NULO exatamente agora!");
            }
            // --- FIM DO CÓDIGO DE INVESTIGAÇÃO ---

            UpdateText();
        }
        else
        {
            Debug.LogWarning($" PASSO 10.A: Nível de infecção de '{this.regionName}' já está no máximo. Disparando surto (outbreak)!");
            TriggerOutbreak(gameManager, outbrokenRegionsInCurrentChain);
        }
    }

    private void TriggerOutbreak(GameManager gameManager, HashSet<RegionController> outbrokenRegionsInCurrentChain)
    {
        Debug.LogWarning($"--- SURTO EM {regionName}! ---");
        outbrokenRegionsInCurrentChain.Add(this);
        gameManager.IncrementOutbreakCounter();

        foreach (RegionController neighbor in adjacentRegions)
        {
            if (neighbor != null)
            {
                Debug.Log($"  > Surto se espalhando de '{regionName}' para o vizinho '{neighbor.regionName}'.");
                neighbor.AddCubeAndHandleOutbreak(gameManager, outbrokenRegionsInCurrentChain);
            }
        }
    }

    void UpdateText()
    {
        if (textMesh != null)
        {
            Debug.Log($" PASSO 11: `UpdateText` na região '{this.regionName}'. Atualizando texto para '{infectionLevel}/{maxInfectionLevel}'.");
            textMesh.text = $"{infectionLevel}/{maxInfectionLevel}";
        }
        else
        {
            // Este log vai disparar se a referência for nula.
            Debug.LogError($"PROBLEMA: `UpdateText` na região '{this.regionName}' falhou porque a referência `textMesh` é NULA.");
        }
    }
    
    // Métodos placeholder que podem ser úteis no futuro
    public void ApplyEffect(string effect) { /* Lógica se precisar */ }
    void Infect() { /* Lógica se precisar */ }
    void Cure() { /* Lógica se precisar */ }
    void ResetInfection() { /* Lógica se precisar */ }
}