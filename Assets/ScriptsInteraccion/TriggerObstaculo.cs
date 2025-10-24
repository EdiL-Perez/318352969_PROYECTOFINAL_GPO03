using UnityEngine;
using UnityEngine.UI;

public class TriggerObstaculo : MonoBehaviour
{

    public GameObject ObjetoRomper;
    private bool jugador = false;

    [Header("Sistema de Vida")]
    public int VidaActual;
    public int VidaMaxima = 3;

    public Image[] corazonesUI;
    //public Sprite corazonVacio;

    [Header("Persistencia")]
    public string IDunico;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        VidaActual = VidaMaxima;

         // 1. Verificar el DataManager
        if (DataManager.Instance != null && !string.IsNullOrEmpty(IDunico))
        {
            // 2. Si este ID ya fue destruido antes, haz que desaparezca ahora.
            if (DataManager.Instance.objetosDestruidos.Contains(IDunico))
            {
                // Destruye el objeto visual y el trigger de forma inmediata al cargar la escena
                Destroy(ObjetoRomper);
                return; 
            }
        }
        
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other){

        if(other.CompareTag("Player")){
            jugador = true;
            Debug.Log("Presiona la tecla k para destruir");
        }

    }
    private void OnTriggerExit(Collider other){
        if(other.CompareTag("Player")){
            jugador = false;
            Debug.Log("Demasiado lejos");
        }
    }
    void Update()
    {




        if (jugador && Input.GetKeyDown(KeyCode.K))
        {
            VidaActual--;
             ActualizarCorazonesUI();
             Debug.Log("VIDA RESTANTE" + VidaActual);

             if(VidaActual <= 0){

                if(DataManager.Instance != null && !string.IsNullOrEmpty(IDunico)){
                    DataManager.Instance.RegistrarDestruccion(IDunico);
                }
                Debug.Log("Obstaculo destruido");
                Destroy(ObjetoRomper);
            }

        }
    }
    void ActualizarCorazonesUI(){

        for(int i=0 ;i<VidaMaxima;i++){

            if(i >= VidaActual){
                if(i < corazonesUI.Length){
                    corazonesUI[i].color=Color.white;

                }
            }

        }
    }
}
