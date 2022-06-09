using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIPauseMenu : MonoBehaviour
{
    [SerializeField]
    GameObject PauseMenu;
    bool paused;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        paused = !paused;
        Time.timeScale = paused ? 0 : 1;

        PauseMenu.SetActive(paused);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
