using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance = null;

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        //
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

  
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        // Si la escena cargada es el Overworld, la batalla, o el Game Over, detener y destruir la musica
        if (scene.name == "OverWorld" || scene.name == "Battle" || scene.name == "GameOver")
        {
            StopAndDestroy();
        }
    }

    private void StopAndDestroy()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            
            audioSource.Stop(); 
        }
        Destroy(gameObject);
    }
}