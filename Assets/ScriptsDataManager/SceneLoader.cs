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

    public void GoToMainMenu()
    {
        if (DataManager.Instance != null)
        {
            DataManager.Instance.ResetGameData();
        }
        
        // Cargar la escena de inicio
        SceneManager.LoadScene(targetSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
        
        // Código para detener el editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}