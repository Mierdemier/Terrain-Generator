using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

//Not SaveLoader, Save*r*Loader. It saves and loads.
//  In Start(), it loads a save if it needs to.
//  LoadSave() loads a save. It takes in the name of the file.
//  Save() saves.
//This.start() needs to be executed *after* everything else!!!
public class SaverLoader : MonoBehaviour
{
    [SerializeField]
    ChunkSystem chunksystem;
    [SerializeField]
    GameObject Ocean;
    [SerializeField]
    UISkySelector SkySelector;
    [SerializeField]
    UIGraphicsPanel GraphicsPanel;

    //If the user has requested to load a save in the main menu, load it.
    void Start()
    {
        if (PlayerPrefs.HasKey("save"))
        {
            LoadSave(PlayerPrefs.GetString("save"));
        }
    }

    //saveName should include the ".terrain"!
    //We're assuming you take the name from a FileInfo.Name or something similar.
    public void LoadSave(string saveName)
    {
        string path = Application.persistentDataPath + "/Saves/" + saveName;
        if (File.Exists(path))
        {
            //Open file and decode it from binary.
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            Save savedata = formatter.Deserialize(file) as Save;

            //Set environment to align with save data.
            chunksystem.GenerateFromSave(savedata);

            Ocean.transform.position = new Vector3(0, savedata.seaLevel, 0);
            Ocean.GetComponent<Renderer>().material.SetColor("_ShallowColour", savedata.shallowColour.ToColour());
            Ocean.GetComponent<Renderer>().material.SetColor("_DeepColour", savedata.deepColour.ToColour());
            Ocean.GetComponent<Renderer>().material.SetFloat("_Depth", savedata.depthBlend);

            SkySelector.ChangeSelection(savedata.skyIndex);
            GraphicsPanel.SetSky();

            //Always close files when you're done.
            file.Close();
        }
        else
            Debug.LogError("The save you tried to load does not exist!");
    }

    //Here the saveName does not include the ".terrain"!
    //We're assuming you take the name directly from the UI.
    public void Save(string saveName)
    {
        //Create a folder for the saves to live in.
        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves");

        BinaryFormatter formatter = new BinaryFormatter();
        //We can use whatever ".something" we want, because this will be a binary file.
        FileStream file = File.Create(Application.persistentDataPath + "/Saves/" + saveName + ".terrain");

        Save savedata = new Save(chunksystem.numChunks.x, chunksystem.numChunks.y,
            chunksystem.GlobalHM, chunksystem.GlobalColours, Ocean.transform.position.y,
        Ocean.GetComponent<Renderer>().material.GetColor("_ShallowColour"),
        Ocean.GetComponent<Renderer>().material.GetColor("_DeepColour"),
        Ocean.GetComponent<Renderer>().material.GetFloat("_Depth"),
        SkySelector.GetIndex());

        //Encode in binary format. Always close files after you're done.
        formatter.Serialize(file, savedata);
        file.Close();
    }
}
