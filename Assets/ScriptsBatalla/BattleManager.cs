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
    public Transform PlayerSpawn; // Punto donde aparecen
    public Transform EnemySpawn;

    private GameObject jugadorinstanciado;
    private GameObject enemigoinstanciado;

    public BattleInventoryUI battleInventoryUI;
    public TextMeshProUGUI enemyHPTextUI;


    [Header("Efectos Visuales")]
    public GameObject playerAttackEffectPrefab;
    public GameObject healingEffectPrefab;


    public AudioSource battleMusicSource;
    public HUDBattle hudManager;

    public void InstanciarParticipantes()
    {
        // Instanciar Jugador
        jugadorinstanciado = Instantiate(Jugadorprefab, PlayerSpawn.position, PlayerSpawn.rotation);
        //Instanciar Enemigo
        enemigoinstanciado = Instantiate(Enemigoprefab, EnemySpawn.position, EnemySpawn.rotation);

        //Asignar las estads al BattleManager
        // Los scripts CombatPlayerStats y CombatEnemyStats se ejecutan
        // Solo necesitamos que el BattleManager tenga la referencia
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
    }

    public void PlayHealingEffect()
    {
    if (healingEffectPrefab == null || jugadorinstanciado == null)
    {
        Debug.LogWarning("No se puede reproducir el efecto de curación: Falta el prefab o el jugador.");
        return;
    }
    
     //
    //Vector3 effectPosition = jugadorinstanciado.transform.position + Vector3.up * 1f;

    // Instanciar el prefab del efecto
    GameObject effectInstance = Instantiate(healingEffectPrefab, PlayerSpawn.position, Quaternion.identity);
    
    
    
    }

    public void PlayPlayerAttackEffect()
    {
        if (playerAttackEffectPrefab == null || enemigoinstanciado == null)
        {
            Debug.LogWarning("No se puede reproducir el efecto de ataque: Falta el prefab o el enemigo.");
            return;
        }
    
        // Calcula una posición ligeramente por encima del enemigo para el impacto
        Vector3 effectPositionBase = enemigoinstanciado.transform.position + Vector3.up * 1f; //

        Vector3 direccionHaciaJugador = (jugadorinstanciado.transform.position - enemigoinstanciado.transform.position).normalized;
    
        
        float desplazamientoAdelante = 0.5f; 
    
        // La posición final
        Vector3 posicionFinal = effectPositionBase + direccionHaciaJugador * desplazamientoAdelante;

        // Instanciar el prefab del efecto
        GameObject effectInstance = Instantiate(playerAttackEffectPrefab, posicionFinal, Quaternion.identity);
    
    
    }


    public void StopMusic()
    {
        if (battleMusicSource != null && battleMusicSource.isPlaying)
        {
            
            battleMusicSource.Stop();
        }
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
        if (playerStats != null)
        {
            playerStats.DesactivarDefensa();
        }
        Debug.Log("Turno del Jugador. Mostrando opciones del HUD.");
        
        // Muestra las opciones de Ataquee 
        if (hudManager != null)
        {
            hudManager.mostraropciones(); // 
        }

        
    }


    IEnumerator InicioBatalla()
    {
        Debug.Log("INCIANDO LA SECUENCIA DE BATALLA");
        
        // 
        // Espera un momento despues de la transición de camara
        yield return new WaitForSeconds(1.5f); 
        // 
        EstadoActual = BattleState.PLAYER_TURN;
        PlayerTurn();
    }

    public void Ataquejugador()
    {
        // Ocultar las opciones del HUD
        if (hudManager != null)
        {
            hudManager.ocultaropciones();
        }
        
        // Ejecutar la acción
        Debug.Log("Jugador ataca");
        playerStats.AnimarAtaque();
        PlayPlayerAttackEffect();
        
        // Aplicar daño
        EnemyStats.DañoRecibido(playerStats.Ataque);

        //Revisar el estado y pasar al siguiente turno
        EndPlayerAction();
    }


    public void Defenderjugador()
    {

        if (EstadoActual != BattleState.PLAYER_TURN || playerStats == null) 
        {
            return;
        }
        
        if (hudManager != null)
        {
        //deshabilita todos los botones 
            hudManager.ocultaropciones(); 
        }
        //El jugador activa el estado de defensa
        playerStats.ActivarDefensa();

    // Pasa el turno
        EndPlayerAction(); 
    }

    public void EndPlayerAction()
    {
        // Ocultar cualquier panel de opciones (Inventario) 
        if (battleInventoryUI != null)
        {
            battleInventoryUI.HideInventoryPanel(); 
        }

        //finalización del turno
        StartCoroutine(FinalTurno());
    }
    IEnumerator AtaqueEnemigo()
    {
        EstadoActual = BattleState.ENEMY_TURN;
        Debug.Log("Turno del Enemigo");

        //espera un momento para que el jugador vea que el turno cambio
        yield return new WaitForSeconds(1.5f); 
        
        //
        Debug.Log("Enemigo ataca!");
        EnemyStats.AnimarAtaque();
        playerStats.DañoRecibido(EnemyStats.Ataque);
        // Pasa a chequear el estado y el fin del turno
        StartCoroutine(FinalTurno());
    }
    IEnumerator FinalTurno()
    {
        // Esperar un momento para que las animaciones de daño terminen
        yield return new WaitForSeconds(1.0f); 

        //Chequeo de Victoria/Derrota
        if (EnemyStats.VidaActualHP <= 0)
        {
            EstadoActual = BattleState.WON;
            Debug.Log("¡Victoria! Lógica de fin de batalla...");
            EnemyStats.ActivarAnimacionMuerte();
            yield return new WaitForSeconds(1.5f); 
            StopMusic();
            DataManager.Instance.RegresarAlOverworld();
            yield break; // Detiene la corutina
        }
        if (playerStats.VidaActualHP <= 0)
        {
            EstadoActual = BattleState.LOST;
            Debug.Log("Derrota. Game Over...");
            playerStats.ActivarAnimacionMuerte();
            yield return new WaitForSeconds(1.5f);
            StopMusic();
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver"); 
            yield break; // Detiene la corutina
        }

        //Si nadie muere pasar al siguiente turno
        if (EstadoActual == BattleState.PLAYER_TURN)
        {
            StartCoroutine(AtaqueEnemigo()); // Si era el turno del jugador pasa al enemigo
        }
        else // Era el turno del enemigo
        {
            EstadoActual = BattleState.PLAYER_TURN;
            PlayerTurn(); // Vuelve al turno del jugador
        }
    }



}
