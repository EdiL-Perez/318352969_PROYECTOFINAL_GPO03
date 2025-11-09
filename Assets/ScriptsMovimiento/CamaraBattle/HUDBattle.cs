using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HUDBattle : MonoBehaviour
{
    public GameObject  Interfaz;
    public BattleManager BattleManager; 
    public Button Botonataque;
    public Button BotonDefender;



    public TextMeshProUGUI playerHPText;   
    public TextMeshProUGUI playerAttackText;


    [Header("Conexión con Inventario")]
    public BattleInventoryUI battleInventoryUI;

    public Button BotonItems;

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

        if(BotonItems != null)
        {
            BotonItems.interactable = true;
        }

        if(BotonDefender != null)
        {
            BotonDefender.interactable = true;
        }
        UpdatePlayerStatsUI();

    }
    public void ocultaropciones(){
        if(Botonataque != null){
            Botonataque.interactable = false;
        }

        if(BotonItems != null){
            BotonItems.interactable = false;
        }

        if(BotonDefender != null)
        {
            BotonDefender.interactable = false;
        }


    }


    public void PresionarBotonAtacar(){
        if (BattleManager != null)
        {
            // Esta función está en BattleManager y continúa el flujo de turnos
            BattleManager.Ataquejugador(); 
        }
    }

    public void PresionarBotonDefender()
    {
        if (BattleManager != null)
        {
        // 2. Notificamos al BattleManager para que ejecute la defensa
            BattleManager.Defenderjugador(); 
        }
    }


    public void PresionarBotonItems()
    {
        if (battleInventoryUI != null)
        {
            // 1. Ocultamos el HUD principal (Botones de Ataque, etc.)
            ocultaropciones();

            // 2. Mostramos el panel de slots de inventario
            battleInventoryUI.ShowInventoryPanel();
        }
    }

    public void UpdatePlayerStatsUI()
    {
        // El DataManager debe ser un Singleton y ya debe existir
        if (DataManager.Instance == null) return;
        
        int currentHP = DataManager.Instance.jugadorHPActual;
        int maxHP = DataManager.Instance.jugadorMaxHP;
        int attack = DataManager.Instance.jugadorAtaque;

        // . ACTUALIZAR TEXTO DE VIDA (NUMÉRICO)
        if (playerHPText != null)
        {
            playerHPText.text = $"HP: {currentHP} / {maxHP}";
        }

        // . ACTUALIZAR TEXTO DE ATAQUE
        if (playerAttackText != null)
        {
            playerAttackText.text = $"ATAQUE: {attack.ToString()}";
        }
    }
}
