using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenuPanel;
    [SerializeField] private GameObject StartNextWave;
    [SerializeField] private LevelSaver levelSaver;
    [SerializeField] private GameObject playerController;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject loosePanel;
    private InputSystem_Actions inputActions;
    private InputAction pauseAction;

    private void Awake()
    {
        inputActions = new();
        pauseAction = inputActions.Player.Pause;
        pauseAction.performed += StartPause;
    }

    private void OnEnable()
    {
        pauseAction.Enable();
    }

    private void OnDisable()
    {
        pauseAction.Disable();
    }

    private void StartPause(InputAction.CallbackContext ctx)
    {
        if (winPanel.activeInHierarchy || loosePanel.activeInHierarchy) { return; }
        if (Time.timeScale == 0)
        {
            Resume();
            return;
        }
        Time.timeScale = 0;
        playerController.SetActive(false);
        PauseMenuPanel.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        PauseMenuPanel.SetActive(false);
        playerController.SetActive(true);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnMenu(bool saving)
    {
        if (saving)
        {
            levelSaver.SaveActualLevel();
        }
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
}
