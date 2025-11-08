using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public enum BattleState { START, PLAYER_TURN, ENEMY_TURN, WON, LOST }
    public BattleState EstadoActual;

    public PlayerStatsLogica playerStats;
    public EnemyStatsLogica EnemyStats;

    public GameObject Jugadorprefab; // Prefab del Jugador de Combate
    public GameObject Enemigoprefab;  // Prefab del Enemigo de Combate
    public Transform PlayerSpawn; // Punto donde aparece el jugador
    public Transform EnemySpawn;

    private GameObject jugadorinstanciado;
    private GameObject enemigoinstanciado;

    public BattleInventoryUI battleInventoryUI;
    public TextMeshProUGUI enemyHPTextUI;

    public HUDBattle hudManager;

    public void InstanciarParticipantes()
    {
        // 1. Instanciar Jugador
        jugadorinstanciado = Instantiate(Jugadorprefab, PlayerSpawn.position, PlayerSpawn.rotation);
        // 2. Instanciar Enemigo
        enemigoinstanciado = Instantiate(Enemigoprefab, EnemySpawn.position, EnemySpawn.rotation);

        // 3. Asignar las estadísticas al BattleManager (¡Crucial!)
        // Los scripts CombatPlayerStats y CombatEnemyStats se ejecutan en Start/Awake del objeto instanciado.
        // Solo necesitamos que el BattleManager tenga la referencia:
        playerStats = jugadorinstanciado.GetComponent<PlayerStatsLogica>();
        if (playerStats != null)
        {   
            playerStats.hudManager = hudManager;
        }

        if (battleInventoryUI != null)
        {
            battleInventoryUI.battleManager = this; 
        }
        EnemyStats = enemigoinstanciado.GetComponent<EnemyStatsLogica>();
        if (EnemyStats != null)
        {
            EnemyStats.enemyHPText = enemyHPTextUI;
        }
        
        //Debug.Log("Participantes instanciados y referencias actualizadas.");
        // El resto de la lógica de carga de stats se maneja en el Start/Awake de los prefabs.
    }




    public void IniciarBatalla()
    {
        if (EstadoActual == BattleState.START){
        EstadoActual = BattleState.START;
        StartCoroutine(InicioBatalla());
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void PlayerTurn()
    {
        Debug.Log("Turno del Jugador. Mostrando opciones del HUD.");
        
        // Muestra las opciones de Ataquee tc.
        if (hudManager != null)
        {
            hudManager.mostraropciones(); // 
        }

        
    }


    IEnumerator InicioBatalla()
    {
        Debug.Log("INCIANDO LA SECUENCIA DE BATALLA");
        
        // 
        // Espera un momento después de la transición de cámara y aparición del HUD (opcional)
        yield return new WaitForSeconds(1f); 
        // Decidir quién va primero (por simplicidad, siempre el jugador)
        EstadoActual = BattleState.PLAYER_TURN;
        PlayerTurn();
    }

    public void Ataquejugador()
    {
        // 1. Ocultar las opciones del HUD
        if (hudManager != null)
        {
            hudManager.ocultaropciones();
        }
        
        // 2. Ejecutar la acción
        Debug.Log("Jugador ataca");
        playerStats.AnimarAtaque();
        
        // Aplicar daño (Asume que el daño es fijo por simplicidad)
        EnemyStats.DañoRecibido(playerStats.Ataque);

        // 3. Revisar el estado y pasar al siguiente turno
        EndPlayerAction();
    }

    public void EndPlayerAction()
    {
        // Ocultar cualquier panel de sub-opciones (Inventario) si está visible
        if (battleInventoryUI != null)
        {
            battleInventoryUI.HideInventoryPanel(); 
        }

        // Inicia el proceso de finalización del turno (chequeo de estado y transición)
        StartCoroutine(FinalTurno());
    }
    IEnumerator AtaqueEnemigo()
    {
        EstadoActual = BattleState.ENEMY_TURN;
        Debug.Log("Turno del Enemigo");

        // Espera un momento para que el jugador vea que el turno cambió
        yield return new WaitForSeconds(1.5f); 
        
        // Simular el ataque del enemigo
        Debug.Log("Enemigo ataca!");
        EnemyStats.AnimarAtaque();
        playerStats.DañoRecibido(EnemyStats.Ataque);
        // Pasa a chequear el estado y el fin del turno
        StartCoroutine(FinalTurno());
    }
    IEnumerator FinalTurno()
    {
        // Esperar un momento para que las animaciones de daño terminen
        yield return new WaitForSeconds(1.5f); 

        // 1. Chequeo de Victoria/Derrota
        if (EnemyStats.VidaActualHP <= 0)
        {
            EstadoActual = BattleState.WON;
            Debug.Log("¡Victoria! Lógica de fin de batalla...");
            DataManager.Instance.RegresarAlOverworld();
            yield break; // Detiene la corutina
        }
        if (playerStats.VidaActualHP <= 0)
        {
            EstadoActual = BattleState.LOST;
            Debug.Log("Derrota. Game Over...");
            yield break; // Detiene la corutina
        }

        // 2. Si nadie muere, pasar al siguiente turno
        if (EstadoActual == BattleState.PLAYER_TURN)
        {
            StartCoroutine(AtaqueEnemigo()); // Si era el turno del jugador, pasa al enemigo
        }
        else // Era el turno del enemigo
        {
            EstadoActual = BattleState.PLAYER_TURN;
            PlayerTurn(); // Vuelve al turno del jugador
        }
    }



}
