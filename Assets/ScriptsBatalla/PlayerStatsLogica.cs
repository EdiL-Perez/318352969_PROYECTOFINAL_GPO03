using UnityEngine;

public class PlayerStatsLogica : MonoBehaviour
{
    public int VidaActualHP;
    public int VidaMaxHP;
    public int Ataque;
    private CharacterController controller;

    public bool estaDefendiendo = false;

    public HUDBattle hudManager;


    private Animator anim;


    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        if(DataManager.Instance == null){
            Debug.LogError("DataManager no encontrado");
        }
        VidaMaxHP=DataManager.Instance.jugadorMaxHP;
        VidaActualHP=DataManager.Instance.jugadorHPActual;
        Ataque=DataManager.Instance.jugadorAtaque;
        
        
        Debug.Log($"DATOS INSTANCIADOS JUGADOR, HP: {VidaActualHP}/{VidaMaxHP}, Ataque: {Ataque}");

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DañoRecibido(int damage){

        if (estaDefendiendo)
        {
            Debug.Log("El jugador se está defendiendo y anula el daño.");
        
            return; // Detiene la función, el HP no se reduce.
        }
        VidaActualHP -= damage;

        DataManager.Instance.jugadorHPActual = VidaActualHP;

        if (hudManager != null)
        {
            hudManager.UpdatePlayerStatsUI();
        }

        if(VidaActualHP >0){
            if (anim != null){
                anim.SetTrigger("Daño");
            } 
        }
    }


    public void SincronizarHP()
    {
    // Carga el valor curado que está guardado en el Singleton.
        VidaActualHP = DataManager.Instance.jugadorHPActual; 
    
        Debug.Log($"HP Sincronizado. Nuevo HP local: {VidaActualHP}");
    }

    public void ActivarDefensa()
    {
        estaDefendiendo = true;
    
        Debug.Log("Defensa Activada.");
    }

    public void DesactivarDefensa()
    {
        estaDefendiendo = false;
        Debug.Log("Defensa Desactivada.");
    }

    public void AnimarAtaque(){
        if (anim != null){
            anim.SetTrigger("Ataque");
        }
    }


    public void ActivarAnimacionMuerte()
    {
    // Solo si la vida realmente llegó a cero
        if (VidaActualHP <= 0)
        {
            if (anim != null)
            {
        
                anim.SetBool("Muerte", true); 
            }
        }
    }





}
