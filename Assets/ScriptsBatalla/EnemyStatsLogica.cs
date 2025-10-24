using UnityEngine;

public class EnemyStatsLogica : MonoBehaviour
{
    public int VidaActualHP;
    public int VidaMaxHP;
    public int Ataque;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(DataManager.Instance == null){
            Debug.LogError("DataManager no encontrado");
        }
        VidaMaxHP=DataManager.Instance.enemigoHPInicial;
        VidaActualHP=VidaMaxHP;
        Ataque=DataManager.Instance.enemigoAtaque;
        
        Debug.Log($"DATOS INSTANCIADOS ENEMIGO, HP: {VidaActualHP}/{VidaMaxHP}, Ataque: {Ataque}");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DañoRecibido(int damage){
        VidaActualHP -= damage;
    }
}
