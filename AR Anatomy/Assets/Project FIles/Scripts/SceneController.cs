using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadScene(int sceneNo)=>SceneManager.LoadScene(sceneNo);
    public void LoadScene(string sceneName)=>SceneManager.LoadScene(sceneName);
    
}
