using UnityEngine;
using TMPro; // Necessário para TextMeshPro e TextMeshProUGUI

public class RegionController : MonoBehaviour
{
    [Header("Region Info")]
    public string regionName;
    public int infectionLevel = 0;
    public int maxInfectionLevel = 3;
    public TextMeshProUGUI textMesh;

    void Start()
    {
        if (string.IsNullOrEmpty(regionName))
        {
            regionName = gameObject.name;
            Debug.LogWarning($"[RegionController] 'Region Name' não definido no Inspector para o GameObject '{gameObject.name}'. Usando o nome do GameObject como fallback: '{regionName}'. É recomendado definir um nome no Inspector.");
        }

        Debug.Log($"[RegionController] Região '{regionName}' iniciada. Nível de infecção inicial: {infectionLevel}/{maxInfectionLevel}.");

        if (textMesh == null)
        {
            textMesh = GetComponent<TextMeshProUGUI>();
            if (textMesh != null)
            {
                Debug.Log($"[RegionController] Componente TextMeshProUGUI atribuído automaticamente para a região '{regionName}'.");
            }
            else
            {
                Debug.LogError($"[RegionController] Falha ao encontrar o componente TextMeshProUGUI automaticamente na região '{regionName}'. Verifique se ele existe no GameObject e se o tipo está correto (TextMeshProUGUI para UI).");
            }
        }

        if (textMesh == null)
        {
            Debug.LogWarning($"[RegionController] TextMeshProUGUI não atribuído para a região '{regionName}' no Inspector E não pôde ser encontrado automaticamente. A interface de texto não será atualizada.");
        }

        UpdateText();
    }

    public void ApplyEffect(string effect) // <-- MÉTODO PÚBLICO ApplyEffect
    {
        Debug.Log($"[RegionController] Região '{regionName}' - ApplyEffect chamado com efeito: '{effect}'.");
        if (string.IsNullOrEmpty(effect))
        {
            Debug.LogWarning($"[RegionController] Região '{regionName}' - ApplyEffect chamado com um efeito nulo ou vazio.");
            return;
        }

        string effectLower = effect.ToLower();
        switch (effectLower)
        {
            case "infect":
                Infect();
                break;
            case "cure":
                Cure();
                break;
            case "reset":
                ResetInfection();
                break;
            default:
                Debug.LogWarning($"[RegionController] Região '{regionName}' - Efeito desconhecido recebido: '{effect}'. (Original: '{effect}')");
                break;
        }
    }

    void Infect()
    {
        Debug.Log($"[RegionController] Região '{regionName}' - Tentando infectar. Nível atual: {infectionLevel}.");
        if (infectionLevel < maxInfectionLevel)
        {
            infectionLevel++;
            Debug.Log($"[RegionController] Região '{regionName}' - Nível de infecção aumentado para: {infectionLevel}/{maxInfectionLevel}.");
            UpdateText();
        }
        else
        {
            Debug.Log($"[RegionController] Região '{regionName}' - Já está no nível máximo de infecção ({maxInfectionLevel}).");
        }
    }

    void Cure()
    {
        Debug.Log($"[RegionController] Região '{regionName}' - Tentando curar. Nível atual: {infectionLevel}.");
        if (infectionLevel > 0)
        {
            infectionLevel--;
            Debug.Log($"[RegionController] Região '{regionName}' - Nível de infecção diminuído para: {infectionLevel}/{maxInfectionLevel}.");
            UpdateText();
        }
        else
        {
            Debug.Log($"[RegionController] Região '{regionName}' - Já está sem infecção.");
        }
    }

    void ResetInfection()
    {
        Debug.Log($"[RegionController] Região '{regionName}' - Resetando infecção. Nível atual: {infectionLevel}.");
        infectionLevel = 0;
        Debug.Log($"[RegionController] Região '{regionName}' - Nível de infecção resetado para: {infectionLevel}/{maxInfectionLevel}.");
        UpdateText();
    }

    void UpdateText()
    {
        if (textMesh != null)
        {
            textMesh.text = $"{infectionLevel}/{maxInfectionLevel}";
        }
    }
}