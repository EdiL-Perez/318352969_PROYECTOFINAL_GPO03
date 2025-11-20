using UnityEngine;

public class arma : MonoBehaviour
{
    // El prefab del arma que se va a sostener
    [Header("Prefabs de Armas Disponibles")]
    // Referencias que debes asignar en el Inspector
    public GameObject espadaBasicaPrefab; 
    public GameObject varitaMagicaPrefab;

    public string defaultWeaponID = "EspadaBasica";

    // La variable que guardara el arma instanciada
    private GameObject currentWeapon; 

    // Referencia al punto de montaje que creaste en el hueso de la mano
    [Header("Punto de Montaje")]
    public Transform weaponMountPoint;

    void Start()
    {

        InitializeDefaultWeaponID();
        if (DataManager.Instance != null)
        {
            string currentWeaponID = DataManager.Instance.armaActualID;
            
            // Logica para encontrar el Prefab
            GameObject weaponPrefabToMount = FindWeaponPrefab(currentWeaponID); 
            
            if (weaponPrefabToMount != null && weaponMountPoint != null)
            {
                MountWeapon(weaponPrefabToMount);
            }
        }
        // 
        //if (weaponPrefab != null && weaponMountPoint != null)
        //{
            //MountWeapon(weaponPrefab);
        //}
    }



    private void InitializeDefaultWeaponID()
    {
        if (DataManager.Instance == null) return;

        // Solo inicializa el ID del arma si el DataManager notiene
        if (string.IsNullOrEmpty(DataManager.Instance.armaActualID) || 
            DataManager.Instance.armaActualID == "Ninguna")
        {
            DataManager.Instance.armaActualID = defaultWeaponID;
            Debug.Log($"WeaponAttachment inicializó el DataManager con ID: {defaultWeaponID}");
        }
    }

    private GameObject FindWeaponPrefab(string id)
    {
        // Usa una estructura para devolver el prefab asignado
        if (id == "EspadaBasica")
        {
            return espadaBasicaPrefab;
        }
        else if (id == "VaritaMagica")
        {
            return varitaMagicaPrefab;
        }
        
        // Puedes devolver null si el ID no es reconocido o el prefab no está asignado
        Debug.LogError($"Weapon ID '{id}' no encontrado o no mapeado.");
        return null; 
    }


    public void MountWeapon(GameObject newWeaponPrefab)
    {
        if (weaponMountPoint == null)
        {
            Debug.LogError("Weapon Mount Point no está asignado. ¡Revisa la configuración!");
            return;
        }

        // Eliminar el arma anterior si existe
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }

        // Instanciar el nuevo Prefab del arma
        currentWeapon = Instantiate(newWeaponPrefab, weaponMountPoint); 
        
        // 
        
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity; 
        
        Debug.Log($"Arma '{newWeaponPrefab.name}' montada en el personaje.");
    }

    public void UpdateWeaponVisual()
    {
        if (DataManager.Instance == null) return;

        string currentWeaponID = DataManager.Instance.armaActualID;
    
        //Encontrar el prefab que debe montar
        GameObject weaponPrefabToMount = FindWeaponPrefab(currentWeaponID); 
    
        //Si se encuentra un prefab, montarlo
        if (weaponPrefabToMount != null && weaponMountPoint != null)
        {
            MountWeapon(weaponPrefabToMount);
            Debug.Log($"Visual del arma actualizado a: {currentWeaponID}");
        }
    }
}