using UnityEngine;
using UnityEngine.UI;


public class HUDBattle : MonoBehaviour
{
    public GameObject  Interfaz;
    public BattleManager BattleManager; 
    public Button Botonataque;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Interfaz.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void mostrarHUD(){
        if(Interfaz != null){
            Interfaz.SetActive(true);

        }
    }
    public void mostraropciones(){
        if(Botonataque != null){
            Botonataque.interactable = true;
        }

    }
    public void ocultaropciones(){
        if(Botonataque != null){
            Botonataque.interactable = false;
        }
    }


    public void PresionarBotonAtacar(){
    // Ocultar las opciones para evitar que el jugador presione dos veces
        //OcultarOpciones();
        if (BattleManager != null)
        {
            // Esta función está en BattleManager y continúa el flujo de turnos
            BattleManager.Ataquejugador(); 
        }
    }
}
