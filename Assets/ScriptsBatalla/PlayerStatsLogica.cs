using UnityEngine;

public class PlayerStatsLogica : MonoBehaviour
{
    public int VidaActualHP;
    public int VidaMaxHP;
    public int Ataque;
    private CharacterController controller;


    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        VidaActualHP -= damage;

        DataManager.Instance.jugadorHPActual = VidaActualHP;
    }
}
