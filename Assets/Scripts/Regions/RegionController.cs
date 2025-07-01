// Arquivo: RegionController.cs
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
        if (textMesh == null) { textMesh = GetComponent<TextMeshProUGUI>(); }
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
            Debug.LogError($"PROBLEMA: `UpdateText` na região '{this.regionName}' falhou porque a referência `textMesh` é NULA.");
        }
    }
    
    // Assegure que os outros métodos estejam aqui também
    public void ApplyEffect(string effect) { /* Lógica se precisar */ }
    void Infect() { /* Lógica se precisar */ }
    void Cure() { /* Lógica se precisar */ }
    void ResetInfection() { /* Lógica se precisar */ }
}