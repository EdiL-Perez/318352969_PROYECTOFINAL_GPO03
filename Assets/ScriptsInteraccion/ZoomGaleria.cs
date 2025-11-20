using UnityEngine;
using System.Collections;

public class ZoomGaleria : MonoBehaviour
{
    //  Escala final
    [Header("Configuración de Zoom y Movimiento")]
    public float zoomScale = 1.5f; 
    public float zoomDuration = 0.3f; 
    
    // Referencia al Panel o Canvas (para centrar)
    public RectTransform centerParent; 

    private Vector3 originalScale;
    private Vector3 originalPosition; //  Guarda la posición original de la imagen
    private int originalSiblingIndex;
    private bool isZoomed = false; 
    
    private RectTransform rectTransform; 

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
        originalPosition = rectTransform.anchoredPosition; // Guardar la posición en el Canvas/Parent
        originalSiblingIndex = transform.GetSiblingIndex();
    }

    
    public void ToggleZoom()
    {
        StopAllCoroutines(); 

        if (isZoomed)
        {
            //vuelve al tamaño y posición originales
            StartCoroutine(AnimateImage(originalScale, originalPosition, originalSiblingIndex));
        }
        else
        {
            // Si no está en zoom aplica el zoom y la posición central
            transform.SetAsLastSibling();
            // El centro del padre (CenterParent) es (0, 0)
            Vector3 centerPosition = Vector3.zero; 
            
            Vector3 targetScale = originalScale * zoomScale;
            StartCoroutine(AnimateImage(targetScale, centerPosition, -1));
        }
        
        isZoomed = !isZoomed;
    }

    IEnumerator AnimateImage(Vector3 targetScale, Vector3 targetPosition, int targetSiblingIndex)
    {
        Vector3 startScale = rectTransform.localScale;
        Vector3 startPosition = rectTransform.anchoredPosition; 
        float timeElapsed = 0f;

      

        while (timeElapsed < zoomDuration)
        {
            float t = timeElapsed / zoomDuration;
            
            
            rectTransform.localScale = Vector3.Lerp(startScale, targetScale, t);
            rectTransform.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, t); //  Interpolar Posición
            
            timeElapsed += Time.deltaTime;
            yield return null; 
        }

        //termine exactamente en la escala 
        rectTransform.localScale = targetScale;
        rectTransform.anchoredPosition = targetPosition;

        if (targetSiblingIndex != -1)
        {
            transform.SetSiblingIndex(targetSiblingIndex);
        }


    }
}