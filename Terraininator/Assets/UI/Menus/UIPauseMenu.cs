using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIPauseMenu : MonoBehaviour
{
    [SerializeField]
    GameObject PauseMenu;
    bool paused = false;

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
        if (paused)
            TogglePause(); //TIL setting the timescale to 0 is permanent even if you load another scene.
        SceneManager.LoadSceneAsync(0);
    }
}
