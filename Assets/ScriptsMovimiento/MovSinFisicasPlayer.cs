using UnityEngine;

public class MovSinFisicasPlayer : MonoBehaviour
{
    [SerializeField] private float velocidad = 3f;
    private Vector3 forward, right;
    private CharacterController controller;
    private Animator animacion; 
    private float velocidadRotacion = 15f;

    void Start()
    {
        controller = GetComponentInChildren<CharacterController>();
        animacion = GetComponentInChildren<Animator>();

        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward.Normalize();

        right = Camera.main.transform.right;
        right.y = 0;
        right.Normalize();
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direccion = Vector3.zero;
        bool movimiento = false; 
    
        //W
        if (vertical > 0) 
        {
            direccion = (forward + right).normalized;
            movimiento = true;
        }
        //A
        else if (horizontal < 0)
        {
            direccion = (forward - right).normalized;
            movimiento = true;
        }
        //S
        else if (vertical < 0)
        {
            direccion = (-forward - right).normalized;
            movimiento = true;
        }
        //D
        else if (horizontal > 0)
        {
            direccion = (-forward + right).normalized;
            movimiento = true;
        }
        // 
        controller.Move(direccion * velocidad * Time.deltaTime);


        if (movimiento)
        {
            // Calcula la rotación que haría que el modelo mire en la dirección de movimiento
            Quaternion rotacionDeseada = Quaternion.LookRotation(direccion);
            // Aplica la rotación suavemente (Slerp) para evitar que gire de golpe
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDeseada, velocidadRotacion * Time.deltaTime);
        }

        if(animacion != null){
            animacion.SetBool("caminar",movimiento);
        }


    }
}
