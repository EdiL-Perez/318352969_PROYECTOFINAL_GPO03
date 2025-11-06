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


    private const int baseAtaqueSinArma = 15;

    [Header("Inventario del Jugador")]
    [Header("Inventario y Estado")]
    // ID del arma actual (ej: "EspadaBasica", "VaritaMagica")
    public string armaActualID; 
    // Lista de ID's de los objetos curativos (máximo 4 slots)
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


    public void ActualizarAtaque(string nuevaArmaID)
    {
        int bonoDeArma = 0;
    
        // LÓGICA DE BONIFICACIÓN POR ARMA
        if (nuevaArmaID == "EspadaBasica")
        {
        // La espada inicial no da bono, o su bono es 0.
            bonoDeArma = 0; 
        }
        else if (nuevaArmaID == "VaritaMagica") 
        {
        // La varita mágica da un bono de +10 al ataque base
        bonoDeArma = 10;
        }

        this.jugadorAtaque = baseAtaqueSinArma + bonoDeArma;
    
        Debug.Log($"Ataque actualizado a: {this.jugadorAtaque}");

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