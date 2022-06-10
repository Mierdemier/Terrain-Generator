using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;

//This class should be attached to the menu where you load saves.
//At the Start() it creates UI elements for every save file it detects.
//It can also delete and load saves, using methods that should be called from UI buttons.
public class LoadMenu : MonoBehaviour
{
    [SerializeField]
    MainMenu Main;
    [SerializeField]
    GameObject SaveTemplate;
    [SerializeField]
    Transform   LayoutObject;

    FileInfo[] saves;


    void Start()
    {
        string path = Application.persistentDataPath + "/Saves"; 
        if (Directory.Exists(path))
        {
            //Saves is an array of FileInfo's.
            saves = new DirectoryInfo(path).GetFiles();
            foreach (FileInfo save in saves)
            {
                //Create a UI element to display the save, and set it's text to the save name.
                GameObject saveDisplay = Instantiate(SaveTemplate, Vector3.zero, Quaternion.identity, LayoutObject);
                saveDisplay.GetComponent<TMP_Text>().text = save.Name;

                //Delete button.
                saveDisplay.transform.GetChild(0).GetComponent<Button>().onClick.
                AddListener(() => {DeleteSave(save);} ); //This syntax makes the button do something on being clicked.
                //Load button.
                saveDisplay.transform.GetChild(1).GetComponent<Button>().onClick.
                AddListener(() => {LoadCanvas(save);} ); //This syntax makes the button do something on being clicked.
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
        PlayerPrefs.SetString("save", file.Name);   //This communicates to the other scene what save we want to load.
        Main.StartCoroutine("LoadMainScene");
    }

}
