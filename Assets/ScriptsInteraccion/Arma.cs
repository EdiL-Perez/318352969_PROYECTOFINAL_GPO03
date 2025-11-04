using UnityEngine;

public class arma : MonoBehaviour
{
    // El prefab del arma que se va a sostener
    [Header("Prefabs de Armas Disponibles")]
    // Referencias que debes asignar en el Inspector
    public GameObject espadaBasicaPrefab; 
    public GameObject varitaMagicaPrefab;

    public string defaultWeaponID = "EspadaBasica";

    // La variable que guardará el arma instanciada
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
            
            // Lógica para encontrar el Prefab (deberás tener esta función FindWeaponPrefab)
            GameObject weaponPrefabToMount = FindWeaponPrefab(currentWeaponID); 
            
            if (weaponPrefabToMount != null && weaponMountPoint != null)
            {
                MountWeapon(weaponPrefabToMount);
            }
        }
        // Si tienes un arma predefinida, móntala al iniciar
        //if (weaponPrefab != null && weaponMountPoint != null)
        //{
            //MountWeapon(weaponPrefab);
        //}
    }



    private void InitializeDefaultWeaponID()
    {
        if (DataManager.Instance == null) return;

        // Solo inicializa el ID del arma si el DataManager aún no tiene uno (ej., está en "Ninguna" o vacío)
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

    /// <summary>
    /// Coloca un arma específica en el punto de montaje.
    /// </summary>
    /// <param name="newWeaponPrefab">El GameObject/Prefab de la nueva arma.</param>
    public void MountWeapon(GameObject newWeaponPrefab)
    {
        if (weaponMountPoint == null)
        {
            Debug.LogError("Weapon Mount Point no está asignado. ¡Revisa la configuración!");
            return;
        }

        // 1. Eliminar el arma anterior si existe
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }

        // 2. Instanciar el nuevo Prefab del arma
        currentWeapon = Instantiate(newWeaponPrefab, weaponMountPoint); 
        
        // 3. Establecer la posición y rotación a cero LOCAL
        // Esto hace que el arma se alinee perfectamente con el objeto 'WeaponMount'.
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity; 
        
        Debug.Log($"Arma '{newWeaponPrefab.name}' montada en el personaje.");
    }

    public void UpdateWeaponVisual()
    {
        if (DataManager.Instance == null) return;

        string currentWeaponID = DataManager.Instance.armaActualID;
    
        // 1. Encontrar el prefab que debe montar
        GameObject weaponPrefabToMount = FindWeaponPrefab(currentWeaponID); 
    
        // 2. Si se encuentra un prefab, montarlo
        if (weaponPrefabToMount != null && weaponMountPoint != null)
        {
            MountWeapon(weaponPrefabToMount);
            Debug.Log($"Visual del arma actualizado a: {currentWeaponID}");
        }
    }
}