using UnityEngine;
using System.Collections;

public class ZoomGaleria : MonoBehaviour
{
    //  Escala final a la que se agrandará la imagen
    [Header("Configuración de Zoom y Movimiento")]
    public float zoomScale = 1.5f; 
    public float zoomDuration = 0.3f; 
    
    // Referencia al Panel o Canvas (para centrar)
    public RectTransform centerParent; 

    private Vector3 originalScale;
    private Vector3 originalPosition; //  Guarda la posición original de la imagen
    private bool isZoomed = false; 
    
    private RectTransform rectTransform; 

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
        originalPosition = rectTransform.anchoredPosition; // Guardar la posición en el Canvas/Parent
    }

    /// <summary>
    /// Función pública llamada por el evento OnClick del botón.
    /// </summary>
    public void ToggleZoom()
    {
        StopAllCoroutines(); 

        if (isZoomed)
        {
            // Si está en zoom, vuelve al tamaño y posición originales
            StartCoroutine(AnimateImage(originalScale, originalPosition));
        }
        else
        {
            // Si no está en zoom, aplica el zoom y la posición central

            // 🔥 POSICIÓN CENTRAL: El centro del padre (CenterParent) es (0, 0)
            Vector3 centerPosition = Vector3.zero; 
            
            Vector3 targetScale = originalScale * zoomScale;
            StartCoroutine(AnimateImage(targetScale, centerPosition));
        }
        
        isZoomed = !isZoomed;
    }

    /// <summary>
    /// Corutina para interpolar suavemente la escala y la posición.
    /// </summary>
    IEnumerator AnimateImage(Vector3 targetScale, Vector3 targetPosition)
    {
        Vector3 startScale = rectTransform.localScale;
        Vector3 startPosition = rectTransform.anchoredPosition; // Usamos anchoredPosition para la UI
        float timeElapsed = 0f;

        //  Opcional: Cambiar el padre para que el centrado sea relativo al centro de la pantalla
        // Aunque usar Vector3.zero como targetPosition funciona si el anclaje es correcto.

        while (timeElapsed < zoomDuration)
        {
            float t = timeElapsed / zoomDuration;
            
            // Interpolación Suave (Lerp)
            rectTransform.localScale = Vector3.Lerp(startScale, targetScale, t);
            rectTransform.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, t); //  Interpolar Posición
            
            timeElapsed += Time.deltaTime;
            yield return null; 
        }

        // Asegura que termine exactamente en la escala y posición objetivo
        rectTransform.localScale = targetScale;
        rectTransform.anchoredPosition = targetPosition;
    }
}