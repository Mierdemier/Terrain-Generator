using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameObject OptionsMenu;
    [SerializeField]
    GameObject LoadMenu;
    [SerializeField]
    Image BackgroundFilter;

    [Space]

    [SerializeField]
    TMP_InputField xChunks;
    [SerializeField]
    TMP_InputField yChunks;

    public void NewCanvas()
    {
        PlayerPrefs.SetInt("xChunks", int.Parse(xChunks.text));
        PlayerPrefs.SetFloat("yChunks", int.Parse(yChunks.text));

        StartCoroutine(LoadMainScene());
    }

    public void ToggleOptions()
    {
        OptionsMenu.SetActive(!OptionsMenu.activeInHierarchy);
    }

    public void ToggleLoad()
    {
        LoadMenu.SetActive(!LoadMenu.activeInHierarchy);
    }

    public void OpenReadme()
    {
        Application.OpenURL(Application.persistentDataPath + "/Readme.md");
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator LoadMainScene()
    {
        //Set off spawn effect.

        while (BackgroundFilter.color.a < 1)
        {
            BackgroundFilter.color = new Color(BackgroundFilter.color.r, BackgroundFilter.color.g, BackgroundFilter.color.b,
            BackgroundFilter.color.a + 0.05f);

            yield return new WaitForSeconds(0.05f);
        }

        SceneManager.LoadSceneAsync(1);
    }

}
