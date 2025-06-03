using UnityEngine;

public class GameManager : MonoBehaviour
{    private RegionController[] regioes;
    void Start()
    {
        Debug.Log("[GameManager] Iniciando GameManager...");
        // Captura todas as regiões na cena no início
        regioes = FindObjectsByType<RegionController>(FindObjectsSortMode.None);
        Debug.Log($"[GameManager] Encontradas {regioes.Length} regiões na cena.");
    }
    public void PassarRodada()
    {
        Debug.LogError("--- PassarRodada() FOI CHAMADO! ---");
        Debug.Log("[GameManager] Método PassarRodada() chamado.");
        if (regioes == null || regioes.Length == 0)
        {
            Debug.LogWarning("[GameManager] Nenhuma região encontrada para passar a rodada!");
            return;
        }

        // Escolhe uma região aleatória
        int indice = Random.Range(0, regioes.Length);
        RegionController regiaoEscolhida = regioes[indice];
        
        if (regiaoEscolhida == null)
        {
            Debug.LogError($"[GameManager] Região escolhida no índice {indice} é nula! Verifique a lista de regiões.");
            return;
        }
        
        Debug.Log($"[GameManager] Região aleatória escolhida: {regiaoEscolhida.regionName} (Índice: {indice})");

        // Aplica o efeito (Lembre-se da correção de "infectar" para "infect")
        string efeitoParaAplicar = "infect"; // Corrigido de "infectar"
        Debug.Log($"[GameManager] Aplicando efeito '{efeitoParaAplicar}' na região '{regiaoEscolhida.regionName}'.");
        regiaoEscolhida.ApplyEffect(efeitoParaAplicar);

        Debug.Log($"[GameManager] Rodada passou. Região afetada (tentativa): {regiaoEscolhida.regionName}");
    }
}