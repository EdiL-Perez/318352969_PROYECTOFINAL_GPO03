using UnityEngine;
using System.Collections;

public class CameraZoomZigzag : MonoBehaviour
{
    [Header("Zoom y Tiempo")]
    public float zoomStartDistance; // Posición Z inicial (más lejos)
    public float zoomEndDistance;    // Posición Z final (el zoom)
    public float zoomDuration;      // Duración lenta del zoom (4 segundos)

    [Header("Efecto Zigzag")]
    public float zigzagAmplitude;   // Amplitud de la oscilación lateral (cuánto se mueve lateralmente)
    public float zigzagFrequency;   // Frecuencia de la oscilación (cuántas veces oscila)
    
    [Header("Rotación Final")]
    public float rotationDuration;

    [Header("Vista Final 45°")]
    public float finalTransitionDuration; // Duración de la transición a 45°
    public float finalRotationAngleX;     // Ángulo de inclinación (X)
    public float finalRotationAngleY;     // Ángulo de rotación lateral (Y)
    public Vector3 finalOffset;

    public HUDBattle hud;
    public BattleManager BattleManager;

    private float timer = 0f;
    private Vector3 initialPosition;
    private bool zoom = true;

    void Start()
    {
        // Almacena la posición inicial (X, Y y Z iniciales)
        initialPosition = transform.position;
        // Reinicia la posición Z para empezar el zoom desde zoomStartDistance
        transform.position = new Vector3(initialPosition.x, initialPosition.y, zoomStartDistance);
    }

    void Update()
    {
        if(zoom){
        
        timer += Time.deltaTime;
        
        // El tiempo normalizado (0 a 1) para el avance lineal
        float t = timer / zoomDuration;
        
        // 1. Movimiento Lineal en Z (Avance Lento)
        float newZ = Mathf.Lerp(zoomStartDistance, zoomEndDistance, t);
        
        // 2. Movimiento Zigzag Senoidal en X (Oscilación)
        // Usamos Time.time para una oscilación continua.
        float offsetX = Mathf.Sin(Time.time * zigzagFrequency) * zigzagAmplitude;
        float offsetY = Mathf.Cos(Time.time * zigzagFrequency) * zigzagAmplitude;
        
        // Aplicar la nueva posición
        transform.position = new Vector3(
            initialPosition.x + offsetX, // Posición X base más la oscilación
            initialPosition.y + offsetY,           // Mantiene la posición Y inicial
            newZ                         // Posición Z lineal (el zoom)
        );
        if (timer >= zoomDuration)
        {
            // Asegura que termina en la posición exacta final
            transform.position = new Vector3(initialPosition.x, initialPosition.y, zoomEndDistance);
            zoom = false;
            StartCoroutine(RotacionCamera());
        }
        }

    }

    IEnumerator RotacionCamera(){

        yield return new WaitForSeconds(0.1f); 
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0, 180, 0);
        
        float timeElapsed = 0f;
        while (timeElapsed < rotationDuration)
        {
            // Interpola la rotación suavemente (Spherical Linear Interpolation)
            float t = timeElapsed / rotationDuration;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t*t);
            
            timeElapsed += Time.deltaTime;
            yield return null; // Espera al siguiente frame
        }
        transform.rotation = targetRotation;
        yield return new WaitForSeconds(0.5f); 
        StartCoroutine(Vistafinal());

    }
    IEnumerator Vistafinal(){
        Vector3 startPosition = transform.position;
        
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(finalRotationAngleX, finalRotationAngleY, 0f);
        
        Vector3 pivot = new Vector3(initialPosition.x, initialPosition.y, zoomEndDistance);

        Vector3 targetPosition = pivot + (targetRotation * finalOffset);

        float timeElapsed = 0f;

        while (timeElapsed < finalTransitionDuration)
        {
            float t = timeElapsed / finalTransitionDuration;
            // Opcional: Easing para que la transición sea suave (SmoothStep)
            float easedT = t * t * (3f - 2f * t);

            // Mueve la posición y rota simultáneamente
            transform.position = Vector3.Lerp(startPosition, targetPosition, easedT);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, easedT);
            
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        transform.rotation = targetRotation;

        if(hud != null){
            hud.mostrarHUD();
            
        }
        if (BattleManager != null){
        BattleManager.IniciarBatalla();
        }
        
        enabled = false;

    }

}