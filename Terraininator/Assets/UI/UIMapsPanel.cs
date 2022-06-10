using UnityEngine;

//This class is used to apply maps from the MapTab.
public class UIMapsPanel : MonoBehaviour
{
    [SerializeField]
    ChunkSystem chunksystem;
    [SerializeField]
    UITextureSelector MapSelector;

    public void ApplyMap()
    {
        Texture2D map = MapSelector.GetSelected();
        chunksystem.GenerateFromMap(map);
    }
}
