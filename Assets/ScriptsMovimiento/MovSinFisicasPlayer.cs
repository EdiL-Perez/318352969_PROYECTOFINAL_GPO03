using UnityEngine;

public class MovSinFisicasPlayer : MonoBehaviour
{
    [SerializeField] private float velocidad = 3f;
    private Vector3 forward, right;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

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
    
        //W
        if (vertical > 0) 
        {
            direccion = (forward + right).normalized;
        }
        //A
        else if (horizontal < 0)
        {
            direccion = (forward - right).normalized;
        }
        //S
        else if (vertical < 0)
        {
            direccion = (-forward - right).normalized;
        }
        //D
        else if (horizontal > 0)
        {
            direccion = (-forward + right).normalized;
        }


        // 
        controller.Move(direccion * velocidad * Time.deltaTime);
    }
}
