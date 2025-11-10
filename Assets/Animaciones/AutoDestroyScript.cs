using UnityEngine;

public class AutoDestroyScript : MonoBehaviour
{
    private ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();

        if (ps != null)
        {
            // Destruir el GameObject después de que el sistema de partículas termine.
            // La duración es el tiempo de vida del sistema de partículas.
            Destroy(gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        }
        else
        {
            // Si no tiene ParticleSystem, destrúyelo después de un tiempo fijo.
            Destroy(gameObject, 3f); 
        }
    }
}