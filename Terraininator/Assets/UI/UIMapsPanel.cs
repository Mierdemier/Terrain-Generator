using UnityEngine;

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
