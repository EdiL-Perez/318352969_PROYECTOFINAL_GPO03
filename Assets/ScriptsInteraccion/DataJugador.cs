using UnityEngine;

public class Datajugador : MonoBehaviour
{
    [Header("Estadísticas Base del Jugador")]
    public int baseMaxHP = 100;
    public int baseAttack = 15;

    
    // Bandera para asegurar que la inicialización solo pase una vez
    private bool dataInitialized = false; 
    private CharacterController controller;

    void Awake(){

        controller = GetComponent<CharacterController>();

    }

    void Start()
    {
        // Esta función se encarga de transferir los valores iniciales al DataManager
        InicioDataManager();
        RestaurarDatos();
    }

    private void InicioDataManager ()
    {
        // Siempre verifica que el Singleton exista
        if (DataManager.Instance == null)
        {
            Debug.LogError("DataManager no encontrado. No se pueden guardar las estadísticas.");
            return;
        }
        
        // La inicialización solo debe ocurrir si el juego comienza de cero 
        // y no si regresa de una batalla.
        if (!dataInitialized) 
        {
            // Solo inicializa las variables del DataManager si aún no tienen valores (ej., 0)
            if (DataManager.Instance.jugadorMaxHP == 0) 
            {
                DataManager.Instance.jugadorMaxHP = baseMaxHP;
                DataManager.Instance.jugadorHPActual = baseMaxHP; // La vida actual inicia al máximo
                DataManager.Instance.jugadorAtaque = baseAttack;
            }
            
            dataInitialized = true;
            Debug.Log("PlayerStats inicializó los datos del jugador en el DataManager.");
        }
    }

    // Opcional: Puedes agregar aquí lógica para el jugador al regresar de una batalla, 
    // como actualizar la animación o curarse.
    public void RestaurarDatos()
    {

        if (DataManager.Instance == null) return;

        Vector3 posicionGuardada = DataManager.Instance.posicionRegresoOverworld;
        if (posicionGuardada != Vector3.zero)
        {

            if (controller != null)
            {
                controller.enabled = false;
            }
            // Mover el objeto del jugador a la posición guardada
            // Nota: Si usas CharacterController, es mejor usar .enabled = false, 
            // mover y luego .enabled = true, pero transform.position es suficiente si no hay física compleja.
            transform.position = posicionGuardada;
            Debug.Log($"Posición del jugador restaurada a: {posicionGuardada}");
            Debug.Log("Regresando al Overworld con HP: " + DataManager.Instance.jugadorHPActual);
            // Opcional: Borrar la posición del DataManager después de usarla
            DataManager.Instance.posicionRegresoOverworld = Vector3.zero;
            if (controller != null)
            {
            // Nota: Podrías necesitar un pequeño retraso (yield return null) 
            // en un Coroutine si el CharacterController no se activa correctamente aquí, 
            // pero inténtalo directamente primero.
            controller.enabled = true; 
            }
        }
        // Aquí podrías desencadenar una animación o sonido de aparición.
    }
}