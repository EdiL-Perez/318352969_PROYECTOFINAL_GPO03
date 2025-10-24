using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class RayCastEnemigoPelea : MonoBehaviour
{
    public Transform jugador;
    private float rango = 1.5f;
    public LayerMask Layerjugador;
    private bool detectado = false;
    private NavMeshAgent agente; 
    private float distancia= 0.5f;
    public string Escena="Battle";

    public int HpBase;
    public int damageBase;

    [Header("Persistencia")]
    // unica para cada enemigo en la escena.
    public string IDunico; 



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        if(agente != null){
            agente.stoppingDistance = distancia;
        }

        if (DataManager.Instance != null && !string.IsNullOrEmpty(IDunico))
        {
            if (DataManager.Instance.objetosDestruidos.Contains(IDunico))
            {
                Destroy(this.gameObject); // El enemigo desaparece
                return;
            }
        }


        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 origin = transform.position;
        Vector3 direction = (jugador.position - origin).normalized;
        RaycastHit hit;
        
        if(Physics.Raycast(origin,direction,out hit,rango,Layerjugador)){

            if(hit.collider.transform == jugador){
                
                if(!detectado){
                detectado = true;
                Debug.Log("JUGADOR DETECTADO");
                }
                if (agente != null)
                {
                    agente.SetDestination(jugador.position);

                }


            }

        }
        else{
            if(detectado){
                detectado = false;
                Debug.Log("JUGADOR ESCAPADO, FUERA DE RANGO");
                
            }
            if (agente != null)
            {
                    agente.SetDestination(transform.position);
            }

        }
        if(detectado && agente != null && agente.enabled){
            if(!agente.pathPending && agente.remainingDistance <= agente.stoppingDistance && agente.velocity.sqrMagnitude < 0.1f)
            EscenaCombate();

        }
        
    }
    void OnDrawGizmos()
    {
        if (jugador != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, (jugador.position - transform.position).normalized * rango);
        }
    }
    void EscenaCombate(){
        if(agente != null && detectado){
            agente.isStopped = true;
            detectado = false;

            if(DataManager.Instance != null && !string.IsNullOrEmpty(IDunico)){
                DataManager.Instance.RegistrarDestruccion(IDunico);
                DataManager.Instance.PrepararCombate(HpBase,damageBase);



            }else
            {
                SceneManager.LoadScene(Escena);
            }
        }
    }
}
