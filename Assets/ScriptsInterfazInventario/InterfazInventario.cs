using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InterfazInventario : MonoBehaviour
{
    [Header("Componentes UI")]
    public Image armaSlotImage;             // Slot 1 Arma (Espada/Varita)
    public List<Image> curativoSlotImages;  //Slots 2 al 5 Items Curativos

    //DEFINICIÓN DE IDS
    //
    private const string ID_ESPADA_BASICA = "EspadaBasica";
    private const string ID_VARITA_MAGICA = "VaritaMagica";
    private const string ID_POCION = "Pocion";

    [Header("Assets (Iconos de Items)")]
    // Sprites MANTENER ORDEN
    public Sprite[] itemIconSprites; 
    public Sprite iconoSlotVacio;


    /* ORDEN ESPERADO EN EL ARRAY 
     * 0: Sprite de Espada Basica
     * 1: Sprite de Varita Magica
     * 2: Sprite de Pocion
     */
    
    void Start()
    {
        // 
        // 
        UpdateUI(); 
    }

    // Esta función debe ser llamada cada vez que el inventario cambie (al recoger algo)
    public void UpdateUI()
    {
        if (DataManager.Instance == null) return;
        
        // ACTUALIZAR SLOT DEL ARMA 
        string armaID = DataManager.Instance.armaActualID;
        //Asignamos el sprite que la función de mapeO
        armaSlotImage.sprite = GetIconById(armaID);

        //ACTUALIZAR SLOTS CURATIVOS (4 espacios)
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
            // sprite se repite
            if (itemIconSprites.Length > 2) return itemIconSprites[2]; 
        }

        return iconoSlotVacio; // Devuelve el sprite vacío si no hay coincidencia

    }
}