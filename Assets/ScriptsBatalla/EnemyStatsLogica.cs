using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class EnemyStatsLogica : MonoBehaviour
{
    public int VidaActualHP;
    public int VidaMaxHP;
    public int Ataque;

    public TextMeshProUGUI enemyHPText;
    

    private Animator anim;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        anim = GetComponentInChildren<Animator>();
        if(DataManager.Instance == null){
            Debug.LogError("DataManager no encontrado");
        }
        VidaMaxHP=DataManager.Instance.enemigoHPInicial;
        VidaActualHP=VidaMaxHP;
        Ataque=DataManager.Instance.enemigoAtaque;
        UpdateEnemyStatsUI();
        
        Debug.Log($"DATOS INSTANCIADOS ENEMIGO, HP: {VidaActualHP}/{VidaMaxHP}, Ataque: {Ataque}");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DañoRecibido(int damage){
        VidaActualHP -= damage;

            if(VidaActualHP >0){
                if (anim != null){
                    anim.SetTrigger("Daño");
                }
            }
            else{
                if (anim != null){
                    anim.SetBool("Muerte",true);
                }
            }
        UpdateEnemyStatsUI(); 
    }


    public void AnimarAtaque()
    {
        if (anim != null) anim.SetTrigger("Ataque");
    }


    public void UpdateEnemyStatsUI()
    {

        // 2. ACTUALIZAR TEXTO DE VIDA (NUMÉRICO)
        if (enemyHPText != null)
        {
            enemyHPText.text = $"HP: {VidaActualHP} / {VidaMaxHP}";
        }
    }

}
