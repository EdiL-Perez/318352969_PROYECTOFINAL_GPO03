using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class DataManager : MonoBehaviour
{
    
    public static DataManager Instance;

    // --- 1. DATOS PERSISTENTES DEL JUGADOR ---
    [Header("Datos del Jugador")]
    // HP actual que lleva el jugador de regreso al Overworld
    public int jugadorHPActual;
    // HP máximo (puede ser usado por la UI)
    public int jugadorMaxHP;
    // Ataque base (asumimos que no cambia en el Overworld)
    public int jugadorAtaque;

    [Header("Inventario del Jugador")]
    // String para guardar el NOMBRE o ID del sprite del arma actual
    public string armaActualID;

    // Lista para guardar los ID de los objetos curativos (máximo 4)
    public List<string> objetosCurativosIDs = new List<string>();



    // --- 2. DATOS TEMPORALES DEL ENEMIGO ---
    [Header("Datos del Enemigo en Combate")]
    // HP del enemigo que se encontró (para inicializar el combate)
    public int enemigoHPInicial;
    // Ataque del enemigo que se encontró
    public int enemigoAtaque;

    [Header("Persistencia de Escena")]
    // Lista de identificadores de objetos que deben estar inactivos/destruidos.
    public List<string> objetosDestruidos = new List<string>();
    
    // --- 3. BANDERA DE ESTADO (Opcional, pero útil) ---
    [Header("Estado del Juego")]
    // Podrías guardar la posición en el Overworld para regresar
    public Vector3 posicionRegresoOverworld;
    //public string escenaOverworld = "OverWorld"; // Nombre de tu escena Overworld

    void Awake()
    {
        //Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            // Si ya existe una instancia (porque volvemos a cargar la escena inicial), la destruye.
            Destroy(gameObject);
        }
    }

    public bool AddCurativo(string itemID)
{
    if (objetosCurativosIDs.Count < 4)
    {
        objetosCurativosIDs.Add(itemID);
        return true;
    }
    return false; // Inventario lleno
}

    /// <summary>
    /// Llamada desde el script del enemigo al iniciar el contacto.
    /// Guarda los datos del enemigo encontrado y carga la escena de combate.
    /// </summary>
    /// <param name="HpBase">Vida base del enemigo</param>
    /// <param name="damageBase">Ataque base del enemigo</param>
    public void PrepararCombate(int hpEnemigo, int ataqueEnemigo)
    {
        // Guardar la posición actual del jugador para regresar después
        // (Asume que tu jugador se llama "Player" o tiene un tag para encontrarlo)
        GameObject jugadorOverworld = GameObject.FindGameObjectWithTag("Player"); 
        if (jugadorOverworld != null)
        {
            posicionRegresoOverworld = jugadorOverworld.transform.position;
        }

        // 1. Guardar los datos del enemigo que vamos a enfrentar
        this.enemigoHPInicial = hpEnemigo;
        this.enemigoAtaque = ataqueEnemigo;
        
        // 2. Cargar la escena de combate
        SceneManager.LoadScene("Battle"); 
    }
    public void RegistrarDestruccion(string objectID)
    {
    // Solo añade el ID si aún no está en la lista (para evitar duplicados)
        if (!objetosDestruidos.Contains(objectID))
        {
        objetosDestruidos.Add(objectID);
        Debug.Log($"Objeto registrado para destrucción: {objectID}");
        }
    }
    
    /// <summary>
    /// Llamada desde el script de la escena de combate para volver al Overworld.
    /// </summary>
    public void RegresarAlOverworld()
    {
        // Asegúrate de guardar el estado actual del jugador antes de volver
        // Por ejemplo, si el jugadorHPActual fue modificado en combate.
        SceneManager.LoadScene("OverWorld");
    }
}