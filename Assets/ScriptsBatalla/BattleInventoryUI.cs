using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class BattleInventoryUI : MonoBehaviour
{
    // Referencias para la integración con el sistema de turnos
    [Header("Integración de Combate")]
    public BattleManager battleManager; 
    public GameObject inventoryPanel;
    [Header("Slots Curativos")]
    public Button[] inventoryButtons = new Button[4]; 
    public TextMeshProUGUI feedbackText; 

    public Button BotonRegresar;
    
    // ... (Constantes y referencias) ...
    private const int CURACION_BASE = 30; 
    private const string ID_POCION = "Pocion"; 
    
    // Referencias visuales (Necesitas un script de mapeo o definir los Sprites aquí)
    public Sprite iconoPocion;
    public Sprite iconoSlotVacio;


    void Start()
    {
        // Al iniciar la batalla, el inventario debe estar oculto
        HideInventoryPanel(); 
        // Refrescar para configurar los listeners
        RefreshInventoryDisplay();
    }
    
    // --- MANEJO DEL PANEL ---
    public void ShowInventoryPanel()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(true);
            RefreshInventoryDisplay(); // Asegura que los iconos estén correctos
        }
    }

    public void HideInventoryPanel()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
        }
    }


    public void ExitInventory()
    {
        // 1. Ocultar el panel de inventario inmediatamente
        HideInventoryPanel();

        // 2. Reactivar las opciones principales (Ataque, Items, Defensa) del HUD.
        if (battleManager != null && battleManager.hudManager != null)
        {
            // El HUDManager debe tener una función para reactivar los botones principales.
            battleManager.hudManager.mostraropciones(); 
        }

        Debug.Log("Saliendo del inventario. Opciones de turno reactivadas.");
        
        // El estado del turno (PLAYER_TURN) no se toca, solo se cambia la interfaz.
    }

    // --- LÓGICA DE INVENTARIO ---

    public void RefreshInventoryDisplay()
    {
        if (DataManager.Instance == null) return;
        List<string> curativos = DataManager.Instance.objetosCurativosIDs;

        for (int i = 0; i < inventoryButtons.Length; i++)
        {
            Button slotButton = inventoryButtons[i];
            Image itemImage = slotButton.GetComponent<Image>();
            
            if (i < curativos.Count)
            {
                // Hay un item (asumimos que siempre es Poción)
                // Aquí podrías usar una función de mapeo (GetIconById) si tuvieras más items.
                itemImage.sprite = iconoPocion; 
                itemImage.color = Color.white; 

                slotButton.interactable = true;
                
                // Asignar el Listener (limpiar primero para evitar duplicados)
                slotButton.onClick.RemoveAllListeners();
                int index = i; 
                slotButton.onClick.AddListener(() => UseCurativeItem(index));
            }
            else
            {
                // El slot está vacío
                itemImage.sprite = iconoSlotVacio;
                itemImage.color = new Color(1f, 1f, 1f, 0.2f); 
                slotButton.interactable = false; 
            }
        }
    }

    public void UseCurativeItem(int slotIndex)
    {
        if (battleManager.EstadoActual != BattleManager.BattleState.PLAYER_TURN) return;

        List<string> curativos = DataManager.Instance.objetosCurativosIDs;
        
        if (slotIndex < curativos.Count && curativos[slotIndex] == ID_POCION)
        {
            // 1. Aplicar curación al jugador
            DataManager.Instance.jugadorHPActual += CURACION_BASE;
            if (DataManager.Instance.jugadorHPActual > DataManager.Instance.jugadorMaxHP)
            {
                DataManager.Instance.jugadorHPActual = DataManager.Instance.jugadorMaxHP;
            }

            if (battleManager.playerStats != null)
            {
                battleManager.playerStats.SincronizarHP(); 
            }

            if (battleManager != null)
            {
                battleManager.PlayHealingEffect();
            }
            // 2. Quitar el item de la lista (DataManager)
            curativos.RemoveAt(slotIndex);

            // 3. Actualizar la interfaz (HP del HUD y Slots)

            if (battleManager != null && battleManager.hudManager != null)
            {
                battleManager.hudManager.UpdatePlayerStatsUI(); // Actualiza la barra de vida
             }
            //battleManager.hudManager.UpdatePlayerStatsUI(); // Necesitas esta función en HUDBattle
            // Nota: Ya se oculta y refresca con la llamada a EndPlayerAction

            // 4. Mostrar mensaje y pasar el turno
            if (feedbackText != null)
            {
                feedbackText.text = $"¡Usaste una {ID_POCION}! +{CURACION_BASE} HP.";
            }

            //  LLAMADA CLAVE: Pide al BattleManager que finalice la acción
            battleManager.EndPlayerAction(); 
        }
    }
}