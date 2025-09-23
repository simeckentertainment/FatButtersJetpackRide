using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneOnStart : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(FindAnyObjectByType<SaveManager>().gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
}
