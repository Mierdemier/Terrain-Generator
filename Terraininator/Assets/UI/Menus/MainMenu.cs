using System.Collections;
using System.Reflection;
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
    Image Filter;

    [Space]

    [SerializeField]
    TMP_InputField xChunks;
    [SerializeField]
    TMP_InputField yChunks;

    public void NewCanvas()
    {
        PlayerPrefs.SetInt("xChunks", int.Parse(xChunks.text));
        PlayerPrefs.SetFloat("yChunks", int.Parse(yChunks.text));
        PlayerPrefs.DeleteKey("save");

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
        string path = Assembly.GetEntryAssembly().Location + "/Readme.md";
        Application.OpenURL(Assembly.GetEntryAssembly().Location + "/Readme.md");
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator LoadMainScene()
    {
        //Set off spawn effect.

        while (Filter.color.a < 1)
        {
            Filter.color = new Color(Filter.color.r, Filter.color.g, Filter.color.b,
            Filter.color.a + 0.05f);

            yield return new WaitForSeconds(0.05f);
        }

        SceneManager.LoadSceneAsync(1);
        yield break;
    }

}
