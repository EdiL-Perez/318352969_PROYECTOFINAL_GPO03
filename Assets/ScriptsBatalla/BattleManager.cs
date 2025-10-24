using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public enum BattleState { START, PLAYER_TURN, ENEMY_TURN, WON, LOST }
    public BattleState EstadoActual;

    public PlayerStatsLogica playerStats;
    public EnemyStatsLogica EnemyStats;

    public HUDBattle hudManager;




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
        
        // Muestra las opciones de Ataqueetc.
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
        
        // Aplicar daño (Asume que el daño es fijo por simplicidad)
        EnemyStats.DañoRecibido(playerStats.Ataque);

        // 3. Revisar el estado y pasar al siguiente turno
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
        playerStats.DañoRecibido(EnemyStats.Ataque);
        // Pasa a chequear el estado y el fin del turno
        StartCoroutine(FinalTurno());
    }
    IEnumerator FinalTurno()
    {
        // Esperar un momento para que las animaciones de daño terminen
        yield return new WaitForSeconds(1f); 

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
