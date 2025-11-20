using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OverworldUI : MonoBehaviour
{
    // Referencias UI para el Overworld
    [Header("Estadísticas del Jugador UI")]
    public TextMeshProUGUI playerHPText;    // Texto para VIDA
    public TextMeshProUGUI playerAttackText; // Texto para el valor de ataque

    void Start()
    {
        // valores se carguen tan pronto como la escena inicie
        UpdatePlayerStatsUI();
    }

    void Update()
    {
        // reflejar el HP actual, especialmente al regresar de combate
        UpdatePlayerStatsUI();
    }

 
    public void UpdatePlayerStatsUI()
    {
        if (DataManager.Instance == null) return;
        
        int currentHP = DataManager.Instance.jugadorHPActual;
        int maxHP = DataManager.Instance.jugadorMaxHP;
        int attack = DataManager.Instance.jugadorAtaque;



        //ACTUALIZAR TEXTO DE VIDA
        if (playerHPText != null)
        {
            playerHPText.text = $"HP: {currentHP} / {maxHP}";
        }

        //ACTUALIZAR TEXTO DE ATAQUE
        if (playerAttackText != null)
        {
            playerAttackText.text = $"DAÑO ATAQUE: {attack.ToString()}";
        }
    }
}