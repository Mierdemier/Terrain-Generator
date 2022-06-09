using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

//Not SaveLoader, Save*r*Loader. It saves and loads.
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

    void Start()
    {
        if (PlayerPrefs.HasKey("save"))
        {
            LoadSave(PlayerPrefs.GetString("save"));
        }
    }

    public void LoadSave(string saveName)
    {
        string path = Application.persistentDataPath + "/Saves/" + saveName;
        if (File.Exists(path))
        {
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



            file.Close();
        }
        else
            Debug.LogError("The save you tried to load does not exist!");
    }

    public void Save(string saveName)
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves");

        BinaryFormatter formatter = new BinaryFormatter();
        //We can use whatever .something we want, because this will be a binary file.
        FileStream file = File.Create(Application.persistentDataPath + "/Saves/" + saveName + ".terrain");

        Save savedata = new Save(chunksystem.numChunks.x, chunksystem.numChunks.y,
            chunksystem.GlobalHM, chunksystem.GlobalColours, Ocean.transform.position.y,
        Ocean.GetComponent<Renderer>().material.GetColor("_ShallowColour"),
        Ocean.GetComponent<Renderer>().material.GetColor("_DeepColour"),
        Ocean.GetComponent<Renderer>().material.GetFloat("_Depth"),
        SkySelector.GetIndex());

        formatter.Serialize(file, savedata);
        file.Close();
    }
}
