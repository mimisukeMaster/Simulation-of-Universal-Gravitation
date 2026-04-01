using UnityEngine;

public class SceneController : MonoBehaviour
{
    // Do not destroy this object when loading a new scene
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update() 
    {
        // Change scene when the space key is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int nextSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1;
            if (nextSceneIndex >= UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
            {
                // Back to the first scene if we are at the last scene
                nextSceneIndex = 0; 
            }
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
