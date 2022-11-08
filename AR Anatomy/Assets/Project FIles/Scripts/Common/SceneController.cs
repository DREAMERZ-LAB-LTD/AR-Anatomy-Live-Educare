using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadScene(int sceneNo)=> Load_Scene(sceneNo);
    public static void Load_Scene(int sceneNo)=>SceneManager.LoadScene(sceneNo);
    public static void LoadScene(string sceneName)=>SceneManager.LoadScene(sceneName);

    public void OnClickBackFromARscene() => MainMenuActivityController.backFromARscene = true;


}
