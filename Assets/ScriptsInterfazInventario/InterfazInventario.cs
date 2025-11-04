using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InterfazInventario : MonoBehaviour
{
    [Header("Componentes UI")]
    public Image armaSlotImage;             // Slot 1 Arma (Espada/Varita)
    public List<Image> curativoSlotImages;  //Slots 2 al 5 Items Curativos

    // --- DEFINICIÓN DE IDS ---
    // Usamos constantes para evitar errores tipográficos
    private const string ID_ESPADA_BASICA = "EspadaBasica";
    private const string ID_VARITA_MAGICA = "VaritaMagica";
    private const string ID_POCION = "Pocion";

    [Header("Assets (Iconos de Items)")]
    // Sprites que asignarás en el Inspector. ¡MANTÉN ESTE ORDEN!
    public Sprite[] itemIconSprites; 
    public Sprite iconoSlotVacio;


    /* ORDEN ESPERADO EN EL ARRAY itemIconSprites (Inspector):
     * 0: Sprite de Espada Basica
     * 1: Sprite de Varita Magica
     * 2: Sprite de Pocion
     * (Asegúrate de que tus 3 Sprites estén en este orden)
     */
    
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
        // Asignamos el sprite que la función de mapeo nos dé
        armaSlotImage.sprite = GetIconById(armaID);

        // 2. ACTUALIZAR SLOTS CURATIVOS (4 espacios)
        List<string> curativos = DataManager.Instance.objetosCurativosIDs;
    
        for (int i = 0; i < curativoSlotImages.Count; i++)
        {
            if (i < curativos.Count)
            {
                // Mostrar el icono del objeto curativo
                string itemID = curativos[i];
                curativoSlotImages[i].sprite = GetIconById(itemID); 
                curativoSlotImages[i].color = Color.white; // Icono visible (sin transparencia)
            }
            else
            {
                // Slot vacío
                curativoSlotImages[i].sprite = iconoSlotVacio;
                curativoSlotImages[i].color = new Color(1f, 1f, 1f, 0.3f); // Gris/transparente
            }
        }
    }
    
    // Función de ejemplo para obtener un icono
    private Sprite GetIconById(string id)
    {
        if (id == ID_ESPADA_BASICA)
        {
            if (itemIconSprites.Length > 0) return itemIconSprites[0]; 
        }
        else if (id == ID_VARITA_MAGICA)
        {
            if (itemIconSprites.Length > 1) return itemIconSprites[1]; 
        }
        else if (id == ID_POCION)
        {
            // Solo tienes un item curativo, por lo que este sprite se repite
            if (itemIconSprites.Length > 2) return itemIconSprites[2]; 
        }

        return iconoSlotVacio; // Devuelve el sprite vacío si no hay coincidencia (ej: ID incorrecto o "Ninguna")

    }
}