using UnityEngine;

public class arma : MonoBehaviour
{
    // El prefab del arma que se va a sostener
    [Header("Armas")]
    public GameObject weaponPrefab;

    // La variable que guardará el arma instanciada
    private GameObject currentWeapon; 

    // Referencia al punto de montaje que creaste en el hueso de la mano
    [Header("Punto de Montaje")]
    public Transform weaponMountPoint;

    void Start()
    {
        // Si tienes un arma predefinida, móntala al iniciar
        if (weaponPrefab != null && weaponMountPoint != null)
        {
            MountWeapon(weaponPrefab);
        }
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
}