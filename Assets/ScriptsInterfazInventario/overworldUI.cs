using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OverworldUI : MonoBehaviour
{
    // Referencias UI para el Overworld
    [Header("Estadísticas del Jugador UI")]
    public TextMeshProUGUI playerHPText;    // Texto para "85/100"
    public TextMeshProUGUI playerAttackText; // Texto para el valor de ataque (Opcional en Overworld)

    void Start()
    {
        // Asegura que los valores se carguen tan pronto como la escena inicie
        UpdatePlayerStatsUI();
    }

    void Update()
    {
        // Se llama constantemente para reflejar el HP actual, especialmente al regresar de combate
        UpdatePlayerStatsUI();
    }

    /// <summary>
    /// Lee el DataManager y actualiza la UI del Overworld.
    /// </summary>
    public void UpdatePlayerStatsUI()
    {
        if (DataManager.Instance == null) return;
        
        int currentHP = DataManager.Instance.jugadorHPActual;
        int maxHP = DataManager.Instance.jugadorMaxHP;
        int attack = DataManager.Instance.jugadorAtaque;



        // 2. ACTUALIZAR TEXTO DE VIDA (NUMÉRICO)
        if (playerHPText != null)
        {
            playerHPText.text = $"HP: {currentHP} / {maxHP}";
        }

        // 3. ACTUALIZAR TEXTO DE ATAQUE
        if (playerAttackText != null)
        {
            playerAttackText.text = $"DAÑO ATAQUE: {attack.ToString()}";
        }
    }
}