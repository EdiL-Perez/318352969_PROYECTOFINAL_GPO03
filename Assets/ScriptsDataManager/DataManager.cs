using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class DataManager : MonoBehaviour
{
    
    public static DataManager Instance;

    //  DATOS PERSISTENTES DEL JUGADOR 
    [Header("Datos del Jugador")]
    // HP actual que lleva el jugador de regreso al Overworld
    public int jugadorHPActual;
    // HP máximo 
    public int jugadorMaxHP;
    //Ataque base 
    public int jugadorAtaque;


    private const int baseAtaqueSinArma = 15;

    [Header("Inventario del Jugador")]
    [Header("Inventario y Estado")]
    // ID del arma actual "EspadaBasica", "VaritaMagica")
    public string armaActualID; 
    // Lista de IDs de los objetos curativos ( 4 slots)
    public List<string> objetosCurativosIDs = new List<string>();



    // DATOS TEMPORALES DEL ENEMIGO 
    [Header("Datos del Enemigo en Combate")]
    // HP del enemigo( inicializar el combate)
    public int enemigoHPInicial;
    // Ataque del enemigo
    public int enemigoAtaque;

    [Header("Persistencia de Escena")]
    // Lista de identificadores de objetos que deben estar inactivos/destruidos.
    public List<string> objetosDestruidos = new List<string>();
    
    // BANDERA DE ESTADO
    [Header("Estado del Juego")]
    
    public Vector3 posicionRegresoOverworld;
    //public string escenaOverworld = "OverWorld";


    //DATOS BASE PARA REINICIAR
    private const int BASE_ATAQUE_INICIAL = 15;
    private const int MAX_HP_INICIAL = 100;
    private const string ARMA_INICIAL_ID = "EspadaBasica";

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
            // Si ya existe una instancia (porque volvemos a cargar la escena inicial) la destruye.
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
    
        // BONIFICACIÓN POR ARMA
        if (nuevaArmaID == "EspadaBasica")
        {
        // La espada inicial no da bono
            bonoDeArma = 0; 
        }
        else if (nuevaArmaID == "VaritaMagica") 
        {
        //La varita magica da un bono de +10 
        bonoDeArma = 10;
        }

        this.jugadorAtaque = baseAtaqueSinArma + bonoDeArma;
    
        Debug.Log($"Ataque actualizado a: {this.jugadorAtaque}");

    }


    public void PrepararCombate(int hpEnemigo, int ataqueEnemigo)
    {
        // Guardar la posición actual del jugador para regresar después
        
        GameObject jugadorOverworld = GameObject.FindGameObjectWithTag("Player"); 
        if (jugadorOverworld != null)
        {
            posicionRegresoOverworld = jugadorOverworld.transform.position;
        }

        //Guardar los datos del enemigo 
        this.enemigoHPInicial = hpEnemigo;
        this.enemigoAtaque = ataqueEnemigo;
        
        //Cargar la escena de combate
        SceneManager.LoadScene("Battle"); 
    }
    public void RegistrarDestruccion(string objectID)
    {
    // Solo añade el ID si aún no está en la lista ( evitar duplicados)
        if (!objetosDestruidos.Contains(objectID))
        {
        objetosDestruidos.Add(objectID);
        Debug.Log($"Objeto registrado para destrucción: {objectID}");
        }
    }
    
    public void ResetGameData()
    {
        Debug.Log("Reiniciando todos los datos del juego (Cuando hay Game Over).");

        //Datos del Jugador
        jugadorHPActual = MAX_HP_INICIAL;
        jugadorMaxHP = MAX_HP_INICIAL;
        jugadorAtaque = BASE_ATAQUE_INICIAL;

        // Inventario y Equipamiento
        armaActualID = ARMA_INICIAL_ID;
        objetosCurativosIDs.Clear(); // Limpia la lista de pociones

        //Persistencia de Escena (Objetos Destruidos)
        objetosDestruidos.Clear(); // Hace qe enemigos reaparezcan
    
        // Posición
        posicionRegresoOverworld = Vector3.zero; // Reinicia la posicion de retorno
    }


    public void RegresarAlOverworld()
    {
        // 
        //
        SceneManager.LoadScene("OverWorld");
    }
}