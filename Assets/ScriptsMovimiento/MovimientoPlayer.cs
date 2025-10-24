using UnityEngine;

public class MovimientoPlayer : MonoBehaviour
{
    [SerializeField] private float velocidad = 2;
    private Vector3 forward, right;
    Rigidbody rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        forward = Camera.main.transform.forward;
        forward.y=0;
        forward = Vector3.Normalize(forward);

        right = Camera.main.transform.right;
        right.y=0;
        right = Vector3.Normalize(right);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direccion = (horizontal * right + vertical * forward).normalized;
        rb.MovePosition(transform.position + direccion * velocidad * Time.fixedDeltaTime);

        
    }
}
