using UnityEngine;
using System.IO;
using TMPro;

public class UISaveMenu : MonoBehaviour
{
    [SerializeField]
    SaverLoader SaveSystem;
    [SerializeField]
    TMP_Text ResultText;
    [SerializeField]
    TMP_InputField NameInput;

    static readonly char[] forbiddenChars = {'\\', '/', '<', '>', '*', ':', '?', '|', '\"'};
    static readonly string[] forbiddenNames = {"CON", "PRN", "AUX", "NUL", 
    "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
    "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9"};

    public void Start()
    {
        if (PlayerPrefs.HasKey("save"))
        {
            string name = PlayerPrefs.GetString("save");
            NameInput.text = name.Remove(name.Length - 8); //Remove file extension
        }
    }

    public void AttemptSave()
    {
        string name = NameInput.text;

        //Validate input.
        foreach (char dontuse in forbiddenChars)
        {
            if (name.Contains(dontuse))
            {
                ResultText.text = "Please don't use any of these characters:\n / \\ : * ? \" < > | ";
                return;
            }
        }
        foreach (string dontuse in forbiddenNames)
        {
            if (name == dontuse)
            {
                ResultText.text = "Nice try but we thought of that. Please use a different name.";
            }
        }

        bool alreadyExists = File.Exists(Application.persistentDataPath + "/Saves/" + name + ".terrain");
        
        SaveSystem.Save(name);

        if (alreadyExists)
            ResultText.text = "Successfully overwrote save!";
        else
            ResultText.text = "Successfully saved!";
    }
}
