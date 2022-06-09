using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;

public class LoadMenu : MonoBehaviour
{
    [SerializeField]
    MainMenu Main;
    [SerializeField]
    GameObject SaveTemplate;
    [SerializeField]
    Transform   LayoutObject;

    FileInfo[] saves;
    GameObject[] saveUIElements;


    void Start()
    {
        string path = Application.persistentDataPath + "/Saves"; 
        if (Directory.Exists(path))
        {
            saves = new DirectoryInfo(path).GetFiles();
            saveUIElements = new GameObject[saves.Length];
            for (int i = 0; i < saves.Length; i++)
            {
                FileInfo save = saves[i];
                GameObject saveDisplay = Instantiate(SaveTemplate, Vector3.zero, Quaternion.identity, LayoutObject);
                saveDisplay.GetComponent<TMP_Text>().text = save.Name;

                //Delete button.
                saveDisplay.transform.GetChild(0).GetComponent<Button>().onClick.
                AddListener(() => {DeleteSave(save);} );
                //Load button.
                saveDisplay.transform.GetChild(1).GetComponent<Button>().onClick.
                AddListener(() => {LoadCanvas(save);} );
            }
        }
        else
            Debug.Log("No saves to load.");
    }

    public void DeleteSave(FileInfo file)
    {
        file.Delete();
    }

    public void LoadCanvas(FileInfo file)
    {

        PlayerPrefs.SetString("save", file.Name);
        Main.StartCoroutine("LoadMainScene");
    }

}
