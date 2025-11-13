using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public string targetSceneName; 



    public void LoadTargetScene()
    {
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogError("El nombre de la escena de destino no está asignado.");
            return;
        }
        
        SceneManager.LoadScene(targetSceneName);
    }
    
    /// <summary>
    /// Función para salir del juego (solo funciona en compilaciones).
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
        
        // Código para detener el editor (solo para pruebas)
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}