using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InterfazInventario : MonoBehaviour
{
    [Header("Componentes UI")]
    public Image armaSlotImage;             // La imagen del slot del arma
    public List<Image> curativoSlotImages;  // Las 4 imágenes de los slots curativos

    [Header("Assets (Iconos)")]
    public Sprite iconoArmaDefault;
    public Sprite iconoSlotVacio;
    
    // Lista de todos los sprites/iconos disponibles (Asignar en el Inspector)
    public List<Sprite> availableItemIcons; 
    
    // Un diccionario para mapear ID (string) a Sprite (visual)
    private Dictionary<string, Sprite> iconMap = new Dictionary<string, Sprite>();

    void Start()
    {
        // Inicializar el mapeo de iconos (debes crear esta lógica si es necesario)
        // Por ahora, solo nos aseguraremos de que todo esté limpio.
        UpdateUI(); 
    }

    // Esta función debe ser llamada cada vez que el inventario cambie (al recoger algo).
    public void UpdateUI()
    {
        if (DataManager.Instance == null) return;
        
        // --- 1. ACTUALIZAR SLOT DEL ARMA ---
        string armaID = DataManager.Instance.armaActualID;
        
        // Por simplicidad, usamos el iconoDefault. En un juego real, usarías iconMap.
        if (armaID == "EspadaLarga")
        {
            armaSlotImage.sprite = iconoArmaDefault;
        }
        else
        {
            // Lógica para encontrar el icono del arma (por ejemplo, en un Dictionary)
            armaSlotImage.sprite = GetIconById(armaID); 
        }

        // --- 2. ACTUALIZAR SLOTS CURATIVOS ---
        List<string> curativos = DataManager.Instance.objetosCurativosIDs;

        for (int i = 0; i < curativoSlotImages.Count; i++)
        {
            if (i < curativos.Count)
            {
                // Mostrar el icono del objeto curativo
                curativoSlotImages[i].sprite = GetIconById(curativos[i]); 
                curativoSlotImages[i].color = Color.white; // Icono visible
            }
            else
            {
                // Slot vacío
                curativoSlotImages[i].sprite = iconoSlotVacio;
                curativoSlotImages[i].color = new Color(1f, 1f, 1f, 0.3f); // Transparente o gris
            }
        }
    }
    
    // Función de ejemplo para obtener un icono
    private Sprite GetIconById(string id)
    {
        // Lógica real: buscar el sprite en un diccionario o base de datos.
        // Por ahora, solo devuelve un icono de ejemplo:
        if (id == "Pocion") return availableItemIcons[0]; // Asume que la poción es el primer sprite en la lista
        if (id == "EspadaLarga") return availableItemIcons[1];
        
        return iconoArmaDefault; // Fallback
    }
}