using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

//This class is a collection of utility functions to be called by buttons in the main menu.
public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameObject OptionsMenu;
    [SerializeField]
    GameObject LoadMenu;
    [SerializeField]
    GameObject Readme;
    [SerializeField]
    Image Filter;

    [Space]

    [SerializeField]
    TMP_InputField xChunks;
    [SerializeField]
    TMP_InputField yChunks;

    public void NewCanvas()
    {
        PlayerPrefs.SetInt("xChunks", int.Parse(xChunks.text)); //This communicates to the other scene what size canvas we want.
        PlayerPrefs.SetInt("zChunks", int.Parse(yChunks.text));
        
        PlayerPrefs.DeleteKey("save");  //Don't load any saves if we're making a new canvas.

        //A coroutine is a function that executes over time.
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

    public void ToggleReadme()
    {
        Readme.SetActive(!Readme.activeInHierarchy);
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator LoadMainScene()
    {
        //Slowly fade out the menu.
        while (Filter.color.a < 1)
        {
            Filter.color = new Color(Filter.color.r, Filter.color.g, Filter.color.b,
            Filter.color.a + 0.05f);

            yield return new WaitForSeconds(0.05f);
        }

        //Load main scene.
        SceneManager.LoadSceneAsync(1);
        yield break;
    }

}
