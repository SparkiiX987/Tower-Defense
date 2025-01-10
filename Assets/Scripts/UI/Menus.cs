using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ChangeScene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }
}
