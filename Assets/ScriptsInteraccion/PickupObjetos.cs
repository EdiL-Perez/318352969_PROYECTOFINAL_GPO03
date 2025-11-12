using UnityEngine;

public class PickupObjetos : MonoBehaviour
{
    public enum ItemType { Weapon, Healing }
    public ItemType tipoObjeto;
    public string itemID; // Ej: "Pocion", "EspadaLarga"
    
    [Header("Referencias UI")]
    // Referencia al script que debe actualizarse
    public InterfazInventario inventoryUI; 
    public arma playerWeaponAttachment;


    [SerializeField] private float velocidadRotacion = 50f;


    private void Start()
    {
    // Buscar el script WeaponAttachment en el jugador al iniciar la escena
    GameObject playerObj = GameObject.FindGameObjectWithTag("Player"); 
        if (playerObj != null)
        {
            playerWeaponAttachment = playerObj.GetComponent<arma>();
        }
    }


    private void Update()
    {
        // Rotar el objeto alrededor del eje Y (vertical)
        // Time.deltaTime asegura que la rotación sea suave e independiente del framerate
        transform.Rotate(Vector3.up, velocidadRotacion * Time.deltaTime, Space.World);
    }

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


                if (DataManager.Instance != null)
                {
                    DataManager.Instance.ActualizarAtaque(itemID); 
                }
                if (playerWeaponAttachment != null)
                {
                    playerWeaponAttachment.UpdateWeaponVisual();
                }
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