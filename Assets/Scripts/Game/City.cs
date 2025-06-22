using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class City
{
    [Header("City Info")]
    public string cityName;
    public int cityId;
    
    [Header("Infection Status")]
    public int infectionLevel = 0;
    public int maxInfectionLevel = 3;
    
    [Header("City Properties")]
    public bool isCured = false;
    public bool isOutbreak = false;
    public int outbreakCount = 0;
    
    [Header("Connections")]
    public List<int> connectedCityIds = new List<int>();
    
    // Construtor principal
    public City(string name, int id)
    {
        cityName = name;
        cityId = id;
        infectionLevel = 0;
        maxInfectionLevel = 3;
        isCured = false;
        isOutbreak = false;
        outbreakCount = 0;
        
        Debug.Log($"[City] Cidade '{cityName}' criada com ID {cityId}");
    }
    
    // Construtor padrão para serialização
    public City()
    {
        cityName = "";
        cityId = 0;
        infectionLevel = 0;
        maxInfectionLevel = 3;
        isCured = false;
        isOutbreak = false;
        outbreakCount = 0;
        connectedCityIds = new List<int>();
    }
    
    public void Infect()
    {
        if (infectionLevel < maxInfectionLevel)
        {
            infectionLevel++;
            isCured = false; // Resetar status de curada
            
            Debug.Log($"[City] Cidade {cityName} infectada. Novo nível: {infectionLevel}/{maxInfectionLevel}");
            
            // Verificar se atingiu o nível máximo (outbreak)
            if (infectionLevel >= maxInfectionLevel)
            {
                TriggerOutbreak();
            }
        }
        else
        {
            Debug.Log($"[City] Cidade {cityName} já está no nível máximo de infecção.");
        }
    }
    
    public void Cure()
    {
        if (infectionLevel > 0)
        {
            infectionLevel--;
            Debug.Log($"[City] Cidade {cityName} curada. Novo nível: {infectionLevel}/{maxInfectionLevel}");
            
            // Verificar se foi completamente curada
            if (infectionLevel == 0)
            {
                isCured = true;
                isOutbreak = false; // Resetar status de outbreak
                Debug.Log($"[City] Cidade {cityName} foi completamente curada!");
            }
        }
        else
        {
            Debug.Log($"[City] Cidade {cityName} já está sem infecção.");
        }
    }
    
    public void ResetInfection()
    {
        infectionLevel = 0;
        isCured = false;
        isOutbreak = false;
        outbreakCount = 0;
        Debug.Log($"[City] Cidade {cityName} teve sua infecção resetada.");
    }
    
    private void TriggerOutbreak()
    {
        isOutbreak = true;
        outbreakCount++;
        Debug.LogWarning($"[City] OUTBREAK na cidade {cityName}! Contagem de outbreaks: {outbreakCount}");
    }
    
    // Método para infectar cidades conectadas (será chamado pelo GameManager)
    public List<int> GetConnectedCityIds()
    {
        return new List<int>(connectedCityIds);
    }
    
    public void AddConnection(int cityId)
    {
        if (cityId <= 0)
        {
            Debug.LogError($"[City] ID de cidade inválido: {cityId}");
            return;
        }
        
        if (cityId == this.cityId)
        {
            Debug.LogWarning($"[City] Tentativa de conectar cidade {cityName} a si mesma!");
            return;
        }
        
        if (!connectedCityIds.Contains(cityId))
        {
            connectedCityIds.Add(cityId);
            Debug.Log($"[City] Cidade {cityName} conectada à cidade {cityId}");
        }
        else
        {
            Debug.LogWarning($"[City] Tentativa de conectar cidade {cityName} à cidade {cityId} que já está conectada!");
        }
    }
    
    public void RemoveConnection(int cityId)
    {
        if (connectedCityIds.Contains(cityId))
        {
            connectedCityIds.Remove(cityId);
            Debug.Log($"[City] Conexão removida entre cidade {cityName} e cidade {cityId}");
        }
        else
        {
            Debug.LogWarning($"[City] Tentativa de remover conexão inexistente entre cidade {cityName} e cidade {cityId}!");
        }
    }
    
    public bool IsConnectedTo(int cityId)
    {
        return connectedCityIds.Contains(cityId);
    }
    
    public bool IsFullyInfected()
    {
        return infectionLevel >= maxInfectionLevel;
    }
    
    public bool IsCompletelyCured()
    {
        return infectionLevel == 0;
    }
    
    public float GetInfectionPercentage()
    {
        return (float)infectionLevel / maxInfectionLevel;
    }
    
    public string GetStatusText()
    {
        string status = $"Cidade {cityName}";
        
        if (isOutbreak)
        {
            status += " [OUTBREAK!]";
        }
        else if (isCured)
        {
            status += " [CURADA]";
        }
        
        status += $" - Infecção: {infectionLevel}/{maxInfectionLevel}";
        
        if (connectedCityIds.Count > 0)
        {
            status += $" - Conectada a {connectedCityIds.Count} cidades";
        }
        
        return status;
    }
    
    // Método para obter informações resumidas da cidade
    public string GetShortStatus()
    {
        if (isOutbreak)
            return $"{cityName} [OUTBREAK]";
        else if (isCured)
            return $"{cityName} [CURADA]";
        else
            return $"{cityName} ({infectionLevel}/{maxInfectionLevel})";
    }
} 