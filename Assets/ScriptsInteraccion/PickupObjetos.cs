using UnityEngine;

public class PickupObjetos : MonoBehaviour
{
    public enum ItemType { Weapon, Healing }
    public ItemType tipoObjeto;
    public string itemID; // Ej: "Pocion", "EspadaLarga"
    
    [Header("Referencias UI")]
    // Referencia al script que debe actualizarse
    public InterfazInventario inventoryUI; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bool pickedUp = false;

            if (tipoObjeto == ItemType.Weapon)
            {
                // Lógica de reemplazo de arma
                string armaAnteriorID = DataManager.Instance.armaActualID;
                DataManager.Instance.armaActualID = itemID;
                pickedUp = true;
            }
            else if (tipoObjeto == ItemType.Healing)
            {
                // Lógica para añadir objeto curativo (verificar si hay espacio)
                pickedUp = DataManager.Instance.AddCurativo(itemID);
            }

            if (pickedUp)
            {
                // 1. Notificar a la UI para que se actualice
                if (inventoryUI != null)
                {
                    inventoryUI.UpdateUI();
                }
                
                // 2. Destruir el objeto del mundo
                Destroy(gameObject);
            }
        }
    }
}