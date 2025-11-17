using UnityEngine;
using UnityEngine.SceneManagement;

public class FindelNivel : MonoBehaviour
{


    public string SceneName = "GameOver"; 
    private const string PlayerTag = "Player";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(PlayerTag))
        {
            Debug.Log("SE ALCANZO LA META");
            DataManager.Instance.ResetGameData(); 
            SceneManager.LoadScene(SceneName);
        }
    }






}
